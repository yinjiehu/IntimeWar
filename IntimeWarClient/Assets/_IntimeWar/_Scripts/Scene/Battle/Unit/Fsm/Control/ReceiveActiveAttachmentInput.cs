using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using HutongGames.PlayMaker;

namespace MechSquad.Battle
{
	[HutongGames.PlayMaker.ActionCategory("MechSquad_Unit")]
	public class ReceiveActiveAttachmentInput : FsmStateAbility
	{
		List<IUnitAttachmentInput> _inputs;

		UnitAttachmentManager _attachmentManager;
		
		public override void LateInit()
		{
			base.LateInit();
			_inputs = _unit.GetAllAbilities<IUnitAttachmentInput>().ToList();
			_attachmentManager = _unit.GetAbility<UnitAttachmentManager>();
			Finish();
		}

		public override void OnUpdate()
		{
			base.OnUpdate();

			int rDelayIndex = 0;

			bool mainFireControlActivating = IsAnyMainFireControlHolding();
			bool mainFireControlPressDown = IsAnyMainFireControlPressDown();

			var allActiveAttachment = _attachmentManager.GetAllActiveAttachment();
			using (var itr = allActiveAttachment.GetEnumerator())
			{
				while (itr.MoveNext())
				{
					var inputNo = itr.Current.Key;
					var attachment = itr.Current.Value;

					var holding = IsAnyInputHolding(inputNo);
					var click = IsAnyInputClick(inputNo);
					if (attachment is IWeaponTypeR)
					{
						var weaponR = (IWeaponTypeR)attachment;
						if (mainFireControlActivating)
							weaponR.Activating(rDelayIndex++);

						if (click)
						{
							if (weaponR.IsConnectedToMainFireControl)
								weaponR.DisconnectFromMainFireControl();
							else
								weaponR.ConnectToMainFireControl();
						}
						if (mainFireControlPressDown)
						{
							if (weaponR.IsConnectedToMainFireControl)
								weaponR.StartActivate();
						}
						
					}
					else if(attachment is IWeaponTypeS)
					{
						var singleWeapon = attachment as IWeaponTypeS;
						if (click)
						{
							singleWeapon.Activate();
						}
					}
					else if(attachment is IWeaponTypeG)
					{
						var weaponG = attachment as IWeaponTypeG;
						if (click)
						{
							weaponG.RapidFire();
						}
						else if (holding)
						{
							weaponG.Prepairing();
						}
						else if (weaponG.IsPrepairing && !holding)
						{
							weaponG.Release();
						}
					}
					else
					{
						Debug.LogErrorFormat(attachment, "not support weapon", attachment.name);
					}
				}
			}
		}

		bool IsAnyMainFireControlPressDown()
		{
			bool ret = false;
			for (var i = 0; i < _inputs.Count; i++)
			{
				if (_inputs[i].Enabled)
					ret = ret | _inputs[i].MainFireControlPress;
			}
			return ret;
		}
		bool IsAnyMainFireControlHolding()
		{
			bool ret = false;
			for (var i = 0; i < _inputs.Count; i++)
			{
				if (_inputs[i].Enabled)
					ret = ret | _inputs[i].MainFireControlHoding;
			}
			return ret;
		}
		bool IsAnyInputHolding(int no)
		{
			bool ret = false;
			for (var i = 0; i < _inputs.Count; i++)
			{
				if (_inputs[i].Enabled)
					ret = ret | _inputs[i].AttachmentHolding[no];
			}
			return ret;
		}
		bool IsAnyInputClick(int no)
		{
			bool ret = false;
			for (var i = 0; i < _inputs.Count; i++)
			{
				if (_inputs[i].Enabled)
					ret = ret | _inputs[i].AttachmentClicked[no];
			}
			return ret;
		}
	}
}