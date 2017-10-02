using MechSquad.Battle;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MechSquad.View
{
	public class ActiveAttachmentBtnViewAction : MonoBehaviour
	{
		[SerializeField]
		int _inputNo;

		ActiveAttachment _attachment;

		[SerializeField]
		Image _pad;
		[SerializeField]
		Image _stick;

		[SerializeField]
		Image _icon;

		[SerializeField]
		Image _linkedIcon;
		[SerializeField]
		float _linkConnectedAlpha = 1;
		[SerializeField]
		float _linkDisconnectedAlpha = 0.3f;

		[SerializeField]
		Image _forceReloadingIcon;
		[SerializeField]
		Image _cancelableReloadingIcon;
		[SerializeField]
		Image _cartridge;

		[SerializeField]
		Text _currentAmmo;

		public void InitAttachment()
		{
			var unit = UnitManager.ThisClientPlayerUnit;
			if (unit == null)
				return;

			var attManager = unit.GetAbility<UnitAttachmentManager>();
			_attachment = attManager.GetAttachmentByInputNo(_inputNo) as ActiveAttachment;

			if (_attachment == null)
			{
				_pad.enabled = false;
				_stick.enabled = false;

				_linkedIcon.enabled = false;
				_forceReloadingIcon.enabled = false;
				_cancelableReloadingIcon.enabled = false;
				_cartridge.enabled = false;
				_icon.enabled = false;
				_currentAmmo.enabled = false;

				return;
			}

			_linkedIcon.enabled = _attachment.CanConnectToMainFireControl;

			_forceReloadingIcon.enabled = false;
			_cancelableReloadingIcon.enabled = false;

			_cartridge.enabled = _attachment.ShowCartridge;
			_currentAmmo.enabled = _attachment.ShowTotalAmmo;

			_icon.enabled = true;

			var attachmentSettings = GlobalCache.GetActiveAttachmentSettingsCollection().Get(_attachment.AttachmentID);
			_icon.sprite = GlobalCache.GetIcon().GetAttButtonIcon(attachmentSettings.Category);
			
			UpdateAttachmentState();
		}

		private void Update()
		{
			if (_attachment != null)
			{
				UpdateAttachmentState();
			}
		}

		public void UpdateAttachmentState()
		{
			if(_attachment.CanConnectToMainFireControl)
				SetImageColorAlpha(_linkedIcon, _attachment.IsConnectedToMainFireControl ? _linkConnectedAlpha : _linkDisconnectedAlpha);

			_stick.enabled = _attachment.ShowDragingCursor;

			if (_attachment.ReloadingState == ReloadStateEnum.None)
			{
				_forceReloadingIcon.enabled = false;
				_cancelableReloadingIcon.enabled = false;

				if (_attachment.ShowCartridge)
				{
					_cartridge.enabled = true;
					_cartridge.fillAmount = _attachment.CurrentAmmoCountInCartridge / _attachment.CartridgeCapacity;
				}
			}
			else if (_attachment.ReloadingState == ReloadStateEnum.ForceReloading)
			{
				_forceReloadingIcon.enabled = true;
				_cancelableReloadingIcon.enabled = false;
				_cartridge.enabled = false;

				_forceReloadingIcon.fillAmount = _attachment.ReloadingCompleteRate;
			}
			else
			{
				_forceReloadingIcon.enabled = false;
				_cancelableReloadingIcon.enabled = true;
				_cartridge.enabled = false;

				_cancelableReloadingIcon.fillAmount = _attachment.ReloadingCompleteRate;
			}

			if (_attachment.ShowTotalAmmo)
				_currentAmmo.text = _attachment.CurrentAmmoCountInTotal.ToString();
		}

		void SetImageColorAlpha(Image img, float alpha)
		{
			var color = img.color;
			color.a = alpha;
			img.color = color;
		}
	}
}