using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System;
using Haruna.Pool;

namespace MechSquad.Battle
{
	[Serializable]
	public class SimpleReloader
	{
		bool _reloading;
		public bool IsReloading { get { return _reloading; } }

		string _ammoID;

		[SerializeField, Haruna.Inspector.HarunaInspect(HideInEditorMode = true)]
		public float _reloadDuration;
		[SerializeField, Haruna.Inspector.HarunaInspect(HideInEditorMode = true)]
		public float _ammoInAttachment;

		public float _ammoInStorage;
		public float TotalAmmo { get { return _ammoInAttachment + _ammoInStorage; } }

		float _elapsedTime;
		public float ReloadingCompleteRate { get { return _elapsedTime / _reloadDuration; } }

		[SerializeField]
		SePlayer _reloadStartSe;
		[SerializeField]
		SePlayer _reloadCompleteSe;

		[SerializeField]
		float _reloadStartSePlayDelay = 0.8f;
		[SerializeField]
		float _reloadCompleteSePlayAhead = 0.15f;

		bool _reloadStartSePlayed;
		bool _reloadCompleteSePlayed;


		BattleUnit _unit;
		ActiveAttachment _attachment;
		UnitAttachmentManager _attManager;

		public void Init(BattleUnit unit, ActiveAttachment attachment, string slotID)
		{
			_unit = unit;
			_attachment = attachment;

			_attManager = _unit.GetAbility<UnitAttachmentManager>();
			_ammoID = _unit.InitialParameter.GetActiveSlotAttachedAmmo()[slotID];

			var parameters = GlobalCache.GetActiveAttachmentSettingsCollection().Get(attachment.AttachmentID).ExtraParameters;
			_reloadDuration = parameters[ConstParameter.ReloadSeconds];
			_ammoInAttachment = parameters[ConstParameter.DefaultCarriedCartridgeCount];
			_ammoInStorage = _attManager.GetStorageAmmoCount(_ammoID);
			
			_attManager.EvOnAmmoStorageUpdate += (ammoID, value) =>
			{
				if (ammoID == _ammoID)
					_ammoInStorage = value;
			};
		}

		public void Reload()
		{
			if (!_reloading && TotalAmmo > 0)
			{
				_elapsedTime = 0;
				_reloading = true;
				_reloadStartSePlayed = false;
				_reloadCompleteSePlayed = false;

				if (_ammoInAttachment <= 0)
				{
					_attManager.RequestAmmo(_ammoID, 1);
				}
				else
				{
					_ammoInAttachment--;
				}
			}

		}

		public void Update(float deltaTime)
		{
			if (!_unit.IsControlByThisClient)
				return;

			if (_reloading)
			{
				_elapsedTime += deltaTime;

				if (_reloadStartSe != null && !_reloadStartSePlayed && _elapsedTime > _reloadStartSePlayDelay)
				{
					_reloadStartSePlayed = true;
					_reloadStartSe.Play(_attachment.transform);
				}

				if (_reloadCompleteSe != null && !_reloadCompleteSePlayed && _elapsedTime > _reloadDuration - _reloadCompleteSePlayAhead)
				{
					_reloadCompleteSePlayed = true;
					_reloadCompleteSe.Play(_attachment.transform);
				}

				if (_elapsedTime > _reloadDuration)
				{
					_reloading = false;
					_elapsedTime = 0;
				}
			}
		}
		
		float RequestAmmoInAttachment(float count)
		{
			if (_ammoInAttachment > count)
			{
				_ammoInAttachment -= count;
				return count;
			}

			var temp = _ammoInAttachment;
			_ammoInAttachment = 0;
			return temp;
		}

		public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
		{
			if (stream.isWriting)
			{
				stream.SendNext(_reloading);
				stream.SendNext((short)_ammoInAttachment);
			}
			else
			{
				_reloading = (bool)stream.ReceiveNext();
				_ammoInAttachment = (short)stream.ReceiveNext();
			}
		}
	}
}