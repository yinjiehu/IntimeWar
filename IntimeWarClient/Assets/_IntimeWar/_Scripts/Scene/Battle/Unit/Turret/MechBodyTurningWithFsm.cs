using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System;

namespace  MechSquad.Battle
{
	public class MechBodyTurningWithFsm : AbstractMainFireControlLockOn, IPunObservable
	{
		protected Transform _turretRoot;
		public override Transform TurretRoot { get { return _turretRoot; } }
		
		public override Vector3 CurrentTurretDirection { get { return _currentDirection; } }

		GameObject _cameraFollowTarget;
		public Transform CameraFollowTarget { get { return _cameraFollowTarget.transform; } }

		PlayMakerFSM _fsm;

		[SerializeField]
		Vector3 _currentDirection;

		public override void Init()
		{
			base.Init();
			
			_turretRoot = _unit.GetAbility<UnitPositionDefine>().MainFireControl;

			_fsm = GetComponent<PlayMakerFSM>();
			_fsm.Fsm.GetFsmVector3("CurrentDirection").Value = _turretRoot.forward;

			_cameraFollowTarget = new GameObject("CameraFollowTarget");
			_cameraFollowTarget.transform.SetParent(transform);
			_cameraFollowTarget.transform.position = _unit.Model.position;
			_cameraFollowTarget.transform.rotation = _turretRoot.rotation;
		}
		
		public override void OnUpdate()
		{
			_fsm.Fsm.GetFsmVector3("TargetDirection").Value = GetAimingDirection();

			base.OnUpdate();
		}

		private void LateUpdate()
		{
			if (_fsm != null)
			{
				_currentDirection = _fsm.Fsm.GetFsmVector3("CurrentDirection").Value;
				_turretRoot.forward = _currentDirection;
			}

			_cameraFollowTarget.transform.position = _unit.Model.position;
			_cameraFollowTarget.transform.rotation = _turretRoot.rotation;
		}

		public override void SetAsCameraFollow()
		{
			Camera.main.GetComponent<Deftly.DeftlyCamera>().ResetTarget(
				new GameObject[] { _cameraFollowTarget });
		}

		public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
		{
			if (stream.isReading)
			{
				_fsm.Fsm.GetFsmVector3("CurrentDirection").Value = (Vector3)stream.ReceiveNext();
			}
			else
			{
				stream.SendNext(_fsm.Fsm.GetFsmVector3("CurrentDirection").Value);
			}
		}
	}
}