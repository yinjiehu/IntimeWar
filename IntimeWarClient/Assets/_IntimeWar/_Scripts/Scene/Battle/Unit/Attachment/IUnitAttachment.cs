namespace MechSquad.Battle
{
	public enum ReloadStateEnum
	{
		None,
		CancelableReloading,
		ForceReloading,
	}

	public interface IUnitAttachment
	{
		string AttachmentID { get; }
	}

	/// <summary>
	/// Rapid
	/// </summary>
	public interface IWeaponTypeR
	{
		void StartActivate();
		void Activating(int delayIndex);
		bool IsConnectedToMainFireControl { get; }
		void DisconnectFromMainFireControl();
		void ConnectToMainFireControl();
		//void Aiming(Vector3 direction);
	}
	/// <summary>
	/// Single
	/// </summary>
	public interface IWeaponTypeS
	{
		void Activate();
		void Reload();
	}
	/// <summary>
	/// Grenade
	/// </summary>
	public interface IWeaponTypeG
	{
		void RapidFire();
		bool IsPrepairing { get; }
		void Prepairing();
		void Release();
		void Cancel();
	}


	//public interface IRapidWeapon
	//{
	//	void Activate();
	//	bool IsConnectedToMainFireControl { get; }

	//	ReloadStateEnum ReloadingState { get; }
	//	float ReloadingCompleteRate { get; }

	//	float CartridgeCapacity { get; }
	//	float CurrentAmmoCountInCartridge { get; }
	//	float CurrentAmmoCountInTotal { get; }
	//}

	//public interface ISingleWeapon
	//{
	//	void Activate();
	//	bool IsReloading { get; }
	//	float ReloadingCompleteRate { get; }
	//	float CurrentAmmoCount { get; }
	//}



}