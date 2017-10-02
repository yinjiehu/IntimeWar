using UnityEngine;

namespace MechSquad
{
	public class KeyboardAndMouseBinding
	{
		#region Battle
		public BindedKeyCollection ZoomChange = new BindedKeyCollection();

		public BindedKeyCollection AimingModeAuto = new BindedKeyCollection();
		public BindedKeyCollection AimingModeManual = new BindedKeyCollection();

		public KeyboardStickState MovingDirection = new KeyboardStickState();
		public BindedKeyCollection MainFireControl = new BindedKeyCollection();
		public BindedKeyCollection[] AttachmentButtons = new BindedKeyCollection[4]
		{
			new BindedKeyCollection(),
			new BindedKeyCollection(),
			new BindedKeyCollection(),
			new BindedKeyCollection()
		};
		public BindedKeyCollection AllAttachmentConnection = new BindedKeyCollection();

		#endregion

		public void OnUpdate(float deltaTime)
		{
			ZoomChange.OnUpdate(deltaTime);
			AimingModeAuto.OnUpdate(deltaTime);
			AimingModeManual.OnUpdate(deltaTime);
			
			MovingDirection.OnUpdate(deltaTime);
			MainFireControl.OnUpdate(deltaTime);

			AttachmentButtons[0].OnUpdate(deltaTime);
			AttachmentButtons[1].OnUpdate(deltaTime);
			AttachmentButtons[2].OnUpdate(deltaTime);
			AttachmentButtons[3].OnUpdate(deltaTime);

			AllAttachmentConnection.OnUpdate(deltaTime);
		}
	}
}
