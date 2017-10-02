using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System;
using System.Collections;

namespace MechSquad.Battle
{
	public class TitanFall : Ability
	{
		[Header("Fx")]
		[SerializeField]
		FxHandler _fallFx;

		[SerializeField]
		FxHandler _dropPointFxAlly;
		[SerializeField]
		FxHandler _dropPointFxEnemy;
		FxHandler _dropPointFxInstance;
		[SerializeField]
		SePlayer _fallSe;

		[Header("Timing")]
		[SerializeField]
		float _fromHeight;
		[SerializeField]
		float _playFallSeTiming = 1f;
		[SerializeField]
		float _enableFallStrikeTiming = 1f;
		[SerializeField]
		float _showFallFxTiming = 1f;
		[SerializeField]
		float _fallDuration = 1f;
		[SerializeField]
		float _stunDuration = 1f;
		[SerializeField]
		float _systemActivationDuration = 1f;

		[Header("Strike")]
		[SerializeField]
		float _fallStrikeRadius = 13f;
		[SerializeField]
		bool _showStrikeAreaGizmo;
		[SerializeField]
		LayerMask _fallCollideLayers;

		public enum StateEnum
		{
			None,
			Falling,
			FallStun,
			SystemActivation,
		}
		StateEnum _state = StateEnum.None;
		public StateEnum CurrentState { get { return _state; } }

		float _currentHeight;
		float _fallElapsedTime;

		Vector3 _targetPosition;

		GameObject _tempCameraFollow;
		
		public void StartFall(Vector3 targetPosition)
		{
			_state = StateEnum.Falling;

			_unit.Animator.SetBool("TitanFall", true);

			_targetPosition = targetPosition;
			_unit.Model.position = targetPosition + new Vector3(0, _fromHeight, 0);
			_currentHeight = _fromHeight;
			_fallElapsedTime = 0;

			if (MechSquadPreference.CheckLogLevel(LogLevelEnum.Trace))
				Debug.LogFormat("------ Titan fall Bug----- {0} Start. target position {1}", _unit.name, targetPosition);

			if (_unit.IsPlayerForThisClient)
			{
				_tempCameraFollow = new GameObject("TempCameraFollow");
				_tempCameraFollow.transform.rotation = _unit.Model.rotation;
				_tempCameraFollow.transform.position = targetPosition;
				Camera.main.GetComponent<Deftly.DeftlyCamera>().ResetTarget(new GameObject[] { _tempCameraFollow });

				if (MechSquadPreference.CheckLogLevel(LogLevelEnum.Trace) && _unit.IsPlayerForThisClient)
					Debug.LogFormat("------ Titan fall Bug----- {0} reset camera [{0}]", _unit.name, _tempCameraFollow.transform.position);
			}

			if (_unit.IsControlByThisClient)
			{
				_unit.SendAbilityRPC(this, "RPCStartFall", new object[] { _targetPosition });
			}
		}

		public override void OnUpdate()
		{
			base.OnUpdate();

			if (_state == StateEnum.Falling)
			{
				_fallElapsedTime += Time.deltaTime;
				_currentHeight = Mathf.Lerp(_fromHeight, 0, _fallElapsedTime / _fallDuration);
				_unit.Model.position = _targetPosition + new Vector3(0, _currentHeight, 0);

				if (_fallElapsedTime > _fallDuration)
				{
					if (MechSquadPreference.CheckLogLevel(LogLevelEnum.Trace) && _unit.IsPlayerForThisClient)
						Debug.LogFormat("------ Titan fall Bug----- {0} change fall stun. _fallElapsedTime {1}. _fallDuration {2}. current unit position {3}. current fsm state {4}",
							_unit.name, _fallElapsedTime, _fallDuration, _unit.Model.position, _unit.Fsm.ActiveStateName);
					_fallElapsedTime = 0;
					_state = StateEnum.FallStun;
				}
			}
			else if (_state == StateEnum.FallStun)
			{
				_fallElapsedTime += Time.deltaTime;

				if (_fallElapsedTime > _stunDuration)
				{
					_fallElapsedTime = 0;
					_unit.Animator.SetBool("TitanFall", false);

					_state = StateEnum.SystemActivation;
				}
			}
			else if (_state == StateEnum.SystemActivation)
			{
				_fallElapsedTime += Time.deltaTime;

				if (_fallElapsedTime > _systemActivationDuration)
				{
					_fallElapsedTime = 0;
					_state = StateEnum.None;

					if (_unit.IsPlayerForThisClient)
					{
						Destroy(_tempCameraFollow);
						_unit.GetAbility<IMainFireControlSystem>().SetAsCameraFollow();
					}
				}
			}
		}

		void RPCStartFall(Vector3 position)
		{
			_targetPosition = position;

			ShowDropPositionFx(position);
			StartCoroutine(PlayFallSE(position));
			StartCoroutine(EnableFallStrike(position));
			StartCoroutine(ShowFallFx(position));
		}
		void ShowDropPositionFx(Vector3 position)
		{
			RaycastHit hit;
			var rayFrom = position + new Vector3(0, 1000, 0);
			if (Physics.Raycast(rayFrom, Vector3.down, out hit, 2000, LayerMask.GetMask("EnvUnit")))
			{
				position = hit.point;
			}

			if (_unit.Team == PhotonNetwork.player.GetUnitTeam())
				_dropPointFxInstance = _dropPointFxAlly.Show(position);
			else
				_dropPointFxInstance = _dropPointFxEnemy.Show(position);
		}

		IEnumerator PlayFallSE(Vector3 position)
		{
			yield return new WaitForSeconds(_playFallSeTiming);
			_fallSe.Play(position);
		}

		IEnumerator EnableFallStrike(Vector3 position)
		{
			yield return new WaitForSeconds(_enableFallStrikeTiming);

			HashSet<int> _damagedUnits = new HashSet<int>();
			_damagedUnits.Add(_unit.SeqNo);
			var hits = Physics.OverlapSphere(position, 13f, _fallCollideLayers, QueryTriggerInteraction.Collide);
			for (var i = 0; i < hits.Length; i++)
			{
				var hit = hits[i];
				var receiver = hit.GetComponent<CollisionEventReceiver>();
				if (receiver != null)
				{
					if (receiver.Unit == null)
					{
						Debug.LogErrorFormat(receiver, "receiver has not been initialized yet");
						continue;
					}

					if (!_damagedUnits.Contains(receiver.Unit.SeqNo))
					{
						_damagedUnits.Add(receiver.Unit.SeqNo);

						receiver.OnEvent(new TitanFallStrikeEvent()
						{
							SourcePosition = _unit.Model.position,
							Attacker = _unit.Info,
						});
					}
				}
			}
		}

		IEnumerator ShowFallFx(Vector3 position)
		{
			yield return new WaitForSeconds(_showFallFxTiming);
			_fallFx.Show(position);
			if (_dropPointFxInstance != null)
			{
				_dropPointFxInstance.Hide();
			}
		}

		private void OnDrawGizmosSelected()
		{
			if (_showStrikeAreaGizmo)
			{
				Gizmos.color = new Color(0, 1, 0, 0.5f);
				Gizmos.DrawSphere(transform.position, _fallStrikeRadius);
			}
		}
	}
}