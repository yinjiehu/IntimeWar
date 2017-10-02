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
	public class CartridgeReloader
	{
		UnitAttachmentManager _attManager;

		ReloadStateEnum _state;
		public ReloadStateEnum State { get { return _state; } }

		string _ammoID;
		
		[Haruna.Inspector.HarunaInspect(HideInEditorMode = true)]
		public float _reloadDuration;
		[Haruna.Inspector.HarunaInspect(HideInEditorMode = true)]
		public float _ammoInAttachment;
		[Haruna.Inspector.HarunaInspect(HideInEditorMode = true)]
		public float _ammoInStorage;

		[Haruna.Inspector.HarunaInspect(HideInEditorMode = true)]
		public float _ammoInCartridge;

		[Haruna.Inspector.HarunaInspect(HideInEditorMode = true)]
		public float _cartridgeCapacity;
		public float TotalAmmoExceptCartridge { get { return _ammoInAttachment + _ammoInStorage; } }

		float _elapsedTime;
		public bool IsReloadCompleted { get { return _elapsedTime > _reloadDuration; } }
		public float ReloadingCompleteRate { get { return _elapsedTime / _reloadDuration; } }

		float _toReloadAmmoFromAttachment;
		float _toReloadAmmoFromStorage;

		[SerializeField]
		SePlayer _reloadStartSe;
		[SerializeField]
		float _reloadStartSePlayDelay = 0.2f;

		[SerializeField]
		SePlayer _reloadCompleteSe;
		[SerializeField]
		float _reloadCompleteSePlayAhead = 0.4f;

		bool _reloadStartSePlayed;
		bool _reloadCompleteSePlayed;

		BattleUnit _unit;
		ActiveAttachment _attachment;

		public void Init(BattleUnit unit, ActiveAttachment attachment, string slotID)
		{
			_unit = unit;
			_attachment = attachment;

			_attManager = _unit.GetAbility<UnitAttachmentManager>();
			_ammoID = _unit.InitialParameter.GetActiveSlotAttachedAmmo()[slotID];

			var parameters = GlobalCache.GetActiveAttachmentSettingsCollection().Get(attachment.AttachmentID).ExtraParameters;
			_reloadDuration = parameters[ConstParameter.ReloadSeconds];
			_cartridgeCapacity = parameters[ConstParameter.CartridgeCapacity];
			_ammoInAttachment = _cartridgeCapacity * (parameters[ConstParameter.DefaultCarriedCartridgeCount] - 1);
			_ammoInStorage = _attManager.GetStorageAmmoCount(_ammoID);

			_ammoInCartridge = _cartridgeCapacity;
			_attManager.EvOnAmmoStorageUpdate += (ammoID, value) =>
			{
				if (ammoID == _ammoID)
					_ammoInStorage = value;
			};
		}

		public void Reload()
		{
			if (_state == ReloadStateEnum.None
				&& _ammoInCartridge < _cartridgeCapacity
				&& TotalAmmoExceptCartridge > 0)
			{
				_elapsedTime = 0;
				_reloadStartSePlayed = false;
				_reloadCompleteSePlayed = false;

				var desireReloadCount = _cartridgeCapacity - _ammoInCartridge;

				_toReloadAmmoFromAttachment = RequestAmmoInAttachment(desireReloadCount);
				if (_toReloadAmmoFromAttachment < desireReloadCount)
				{
					desireReloadCount -= _toReloadAmmoFromAttachment;
					_toReloadAmmoFromStorage = _attManager.RequestAmmo(_ammoID, desireReloadCount);
				}
				else
				{
					_toReloadAmmoFromStorage = 0;
				}

				if (_ammoInCartridge > 0)
				{
					_reloadStartSePlayed = true;
					_reloadStartSe.Play(_attachment.transform);
					_state = ReloadStateEnum.CancelableReloading;
				}
				else
					_state = ReloadStateEnum.ForceReloading;
			}
		}

		public void Update(float deltaTime)
		{
			if (!_unit.IsControlByThisClient)
				return;

			if (_state == ReloadStateEnum.ForceReloading || _state == ReloadStateEnum.CancelableReloading)
			{
				_elapsedTime += deltaTime;

				if (_reloadStartSe != null && !_reloadStartSePlayed && _state == ReloadStateEnum.ForceReloading && _elapsedTime > _reloadStartSePlayDelay)
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
					_state = ReloadStateEnum.None;
					_elapsedTime = 0;
					_ammoInCartridge += _toReloadAmmoFromAttachment + _toReloadAmmoFromStorage;
				}
			}
		}

		public void Cancel()
		{
			if (_state == ReloadStateEnum.CancelableReloading)
			{
				_state = ReloadStateEnum.None;
				_elapsedTime = 0;

				_ammoInAttachment = _toReloadAmmoFromAttachment;
				_attManager.ReturnAmmo(_ammoID, _toReloadAmmoFromStorage);
			}
		}

		public void ConsumeAmmo(float count = 1)
		{
			_ammoInCartridge -= count;
			if (_ammoInCartridge == 0)
			{
				Reload();
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
				stream.SendNext((byte)_state);
				stream.SendNext((short)_ammoInCartridge);
				stream.SendNext((short)_ammoInAttachment);
			}
			else
			{
				_state = (ReloadStateEnum)(byte)stream.ReceiveNext();
				_ammoInCartridge = (short)stream.ReceiveNext();
				_ammoInAttachment = (short)stream.ReceiveNext();
			}
		}
	}
}