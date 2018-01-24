using UnityEngine;

namespace IntimeWar
{
	public class KeyboardMouseBinding
	{
        #region Battle

        public BindedKeyCollection MainFireControl = new BindedKeyCollection();

        public BindedKeyCollection MovingUp = new BindedKeyCollection();
        public BindedKeyCollection MovingDown = new BindedKeyCollection();
        public BindedKeyCollection MovingLeft = new BindedKeyCollection();
        public BindedKeyCollection MovingRight = new BindedKeyCollection();

        public BindedKeyCollection ZoomChange = new BindedKeyCollection();

		public BindedKeyCollection AimingModeAuto = new BindedKeyCollection();
		public BindedKeyCollection AimingModeManual = new BindedKeyCollection();
		public BindedKeyCollection SwitchAutoAimingTarget = new BindedKeyCollection();
		

		public BindedKeyCollection[] AttachmentButtons = new BindedKeyCollection[4]
		{
			new BindedKeyCollection(),
			new BindedKeyCollection(),
			new BindedKeyCollection(),
			new BindedKeyCollection()
		};

		public BindedKeyCollection AttachmentConnection = new BindedKeyCollection();
		public BindedKeyCollection ReloadAll = new BindedKeyCollection();
		#endregion

		public KeyboardMouseBinding()
		{
			SetDefault();
		}

		public void SetDefault()
		{
			ZoomChange.ClearAll();
			ZoomChange.BindKey(KeyCode.M, 0);

			AimingModeAuto.ClearAll();
			AimingModeAuto.BindKey(KeyCode.F, 0);
			//AimingModeManual.ClearAll();
			//AimingModeManual.BindKey(KeyCode.Mouse1, 0);
			SwitchAutoAimingTarget.ClearAll();
			SwitchAutoAimingTarget.BindKey(KeyCode.F, 0);

			MovingUp.ClearAll();
			MovingUp.BindKey(KeyCode.W, 0);
			MovingDown.ClearAll();
			MovingDown.BindKey(KeyCode.S, 0);
			MovingLeft.ClearAll();
			MovingLeft.BindKey(KeyCode.A, 0);
			MovingRight.ClearAll();
			MovingRight.BindKey(KeyCode.D, 0);

			MainFireControl.ClearAll();
			MainFireControl.BindKey(KeyCode.Mouse0, 0);

			AttachmentButtons[0].ClearAll();
			AttachmentButtons[0].BindKey(KeyCode.Q, 0);

			AttachmentButtons[1].ClearAll();
			AttachmentButtons[1].BindKey(KeyCode.E, 0);

			AttachmentButtons[2].ClearAll();
			AttachmentButtons[2].BindKey(KeyCode.Mouse1, 0);

			AttachmentButtons[3].ClearAll();
			AttachmentButtons[3].BindKey(KeyCode.Space, 0);

			AttachmentConnection.ClearAll();
			AttachmentConnection.BindKey(KeyCode.LeftShift, 0);

			ReloadAll.ClearAll();
			ReloadAll.BindKey(KeyCode.R, 0);
		}

		Vector2 _normalizedMovingDirection = Vector2.zero;
		public Vector2 NormailizedMovingDirection { get { return _normalizedMovingDirection; } }

		public void OnUpdate(float deltaTime)
		{
            ZoomChange.OnUpdate(deltaTime);

            AimingModeAuto.OnUpdate(deltaTime);
            AimingModeManual.OnUpdate(deltaTime);
            SwitchAutoAimingTarget.OnUpdate(deltaTime);

            MovingUp.OnUpdate(deltaTime);
            MovingDown.OnUpdate(deltaTime);
            MovingLeft.OnUpdate(deltaTime);
            MovingRight.OnUpdate(deltaTime);

            MainFireControl.OnUpdate(deltaTime);

            AttachmentButtons[0].OnUpdate(deltaTime);
            AttachmentButtons[1].OnUpdate(deltaTime);
            AttachmentButtons[2].OnUpdate(deltaTime);
            AttachmentButtons[3].OnUpdate(deltaTime);

            AttachmentConnection.OnUpdate(deltaTime);
            ReloadAll.OnUpdate(deltaTime);

            _normalizedMovingDirection = Vector3.zero;
            for (var i = 0; i < 4; i++)
            {
                _normalizedMovingDirection.y += Input.GetKey(MovingUp.GetKeyCode(i)) ? 1 : 0;
                _normalizedMovingDirection.y += Input.GetKey(MovingDown.GetKeyCode(i)) ? -1 : 0;
                _normalizedMovingDirection.x += Input.GetKey(MovingRight.GetKeyCode(i)) ? 1 : 0;
                _normalizedMovingDirection.x += Input.GetKey(MovingLeft.GetKeyCode(i)) ? -1 : 0;
            }
            _normalizedMovingDirection.Normalize();
        }

	}
}
