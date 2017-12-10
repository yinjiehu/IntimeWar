using MechSquad.Battle;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MechSquad
{
	public enum SeMultipleOverriadeTypeEnum
	{
		Cancel,
		Override,
		Queue,
	}

	public enum SeSourceControlModeEnum
	{
		Random,
		Sequence,
		Concurrent,
	}

	public class SePlayer : MonoBehaviour
	{
		[SerializeField]
		SeMultipleOverriadeTypeEnum _multipleOverrideType;
		public SeMultipleOverriadeTypeEnum MultipleOverrideType { get { return _multipleOverrideType; } }

		[SerializeField]
		int _multipleLimit = -1;
		public int MultipleLimit { get { return _multipleLimit < 0 ? int.MaxValue : _multipleLimit; } }

		[SerializeField]
		SeSourceControlModeEnum _sourceControlMode;
		public SeSourceControlModeEnum SourceControlMode { get { return _sourceControlMode; } }

		[SerializeField]
		List<AudioSource> _audioSources;

		[SerializeField]
		[Range(1, 1000)]
		int _loopTimes = 1;

		[SerializeField]
		SePlayer _playOnEnd;

		[SerializeField]
		bool _randomDelay;
		
		[SerializeField]
		[Haruna.Inspector.HarunaInspect(ShowIf = "_randomDelay")]
		[Range(0, 10)]
		float _randomDelaySeconds;
		float _actualDelayedSeconds;

		float _duration;
		float _elapsedSeconds;
		bool _activated;
		int _playingIndex;

		public int PrefabInstanceID { set; get; }
		public string Sid { set; get; }

		[Header("ShakeCamera")]
		[SerializeField]
		bool _cameraShake;
		[SerializeField][Haruna.Inspector.HarunaInspect(ShowIf = "_cameraShake")]
		int _cameraShakeLevel;

		Transform _followTransform;
		public Transform FollowTransform
		{
			set
			{
				_followTransform = value;
				if(_followTransform != null)
					transform.position = _followTransform.position;
			}
			get { return _followTransform; }
		}

		[SerializeField]
		Haruna.Inspector.InspectorButton _testPlay;
		void TestPlay()
		{
			Activate();
		}

		public SePlayer Play(string sid = "")
		{
			var instance = SeManager.Instance.GetInstanceFromPrefab(this, sid);
			instance.Sid = sid;
			instance.Activate();
			return instance;
		}
		public SePlayer Play(Vector3 position, string sid = "")
		{
			var instance = SeManager.Instance.GetInstanceFromPrefab(this, sid);
			instance.transform.position = position;
			instance.Sid = sid;
			instance.Activate();
			return instance;
		}
		public SePlayer Play(Transform followTransform, string sid = "")
		{
			var instance = SeManager.Instance.GetInstanceFromPrefab(this, sid);
			instance.FollowTransform = followTransform;
			instance.Sid = sid;
			instance.Activate();
			return instance;
		}

		public void RecycleThis()
		{
			_activated = false;
			for (var i = 0; i < _audioSources.Count; i++)
			{
				if (_audioSources[i].isPlaying)
					_audioSources[i].Stop();
			}
			SeManager.Instance.RecycleInstance(this, Sid);

			if (_playOnEnd != null)
			{
				var onEndInstance = _playOnEnd.Play(Sid);
				if (FollowTransform == null)
					onEndInstance.transform.position = transform.position;
				else
					onEndInstance.FollowTransform = FollowTransform;
			}
		}

		public void Activate()
		{
			_activated = true;
			_elapsedSeconds = 0;

			if (_sourceControlMode == SeSourceControlModeEnum.Random)
			{
				var random = UnityEngine.Random.Range(0, _audioSources.Count);
				for (var i = 0; i < _audioSources.Count; i++)
				{
					if (i == random)
					{
						_actualDelayedSeconds = _randomDelay ? UnityEngine.Random.Range(0, _randomDelaySeconds) : 0;
						_duration = _audioSources[i].clip.length * _loopTimes + _actualDelayedSeconds;

						if (!_audioSources[i].isPlaying)
						{
							_audioSources[i].PlayDelayed(_actualDelayedSeconds);
						}

						_audioSources[i].time = 0;
					}
					else
					{
						if (_audioSources[i].isPlaying)
							_audioSources[i].Stop();
					}
				}
				_playingIndex = random;
			}
			else if (_sourceControlMode == SeSourceControlModeEnum.Sequence)
			{
				_actualDelayedSeconds = _randomDelay ? UnityEngine.Random.Range(0, _randomDelaySeconds) : 0;

				float temp = 0;
				for (var i = 0; i < _audioSources.Count; i++)
				{
					if (i == 0)
					{
						if (!_audioSources[i].isPlaying)
						{
							_audioSources[i].PlayDelayed(_actualDelayedSeconds);
						}
					}
					else
					{
						if (_audioSources[i].isPlaying)
							_audioSources[i].Stop();
					}

					temp += _audioSources[i].clip.length;
				}

				_playingIndex = 0;
				_duration = temp * _loopTimes + _actualDelayedSeconds;
			}
			else if (_sourceControlMode == SeSourceControlModeEnum.Concurrent)
			{
				throw new System.NotImplementedException();
			}

			//if (_cameraShake)
			//{
			//	MechSquadBattleCameraShaker.ShakeCameraOnce(_cameraShakeLevel, transform.position);
			//}
		}

		public void CancelImmidiately()
		{
			RecycleThis();
		}

		public void CancelAtCurrentAudioSourceStop()
		{
			if (!_audioSources[_playingIndex].isPlaying)
			{
				RecycleThis();
			}
			else
			{
				_duration = _elapsedSeconds + _audioSources[_playingIndex].clip.length - _audioSources[_playingIndex].time + _actualDelayedSeconds;
			}
		}

		private void Update()
		{
			if (_activated)
			{
				_elapsedSeconds += Time.deltaTime;
				if (_elapsedSeconds > _duration)
				{
					RecycleThis();
					return;
				}

				if (_sourceControlMode == SeSourceControlModeEnum.Sequence)
				{
					if (!_audioSources[_playingIndex].isPlaying)
					{
						_playingIndex++;
						if (_playingIndex >= _audioSources.Count)
							_playingIndex = 0;

						//_audioSources[_playingIndex].Play();
						{
							_audioSources[_playingIndex].Play();
						}
					}
				}

				if (_followTransform != null)
					transform.position = _followTransform.position;
			}
		}
	}
}
