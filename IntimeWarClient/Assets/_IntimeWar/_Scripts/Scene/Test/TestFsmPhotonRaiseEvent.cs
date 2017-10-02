using UnityEngine;

public class TestFsmPhotonRaiseEvent : HutongGames.PlayMaker.FsmStateAction
{

	public override void OnEnter()
	{
		base.OnEnter();

		MechSquad.PhotonCustomEventReceiver.RegistCustomEvent("GGGGG", obj =>
		{
			Debug.LogFormat("receive event abcde! {0}", obj);
		});
	}

	public override void OnUpdate()
	{
		base.OnUpdate();

		if (Input.GetKeyDown(KeyCode.G))
		{
			MechSquad.PhotonCustomEventSender.RaisePhotonCustomEvent("GGGGG", "11111111111111111111111111");
		}
	}

	public override void OnExit()
	{
		base.OnExit();
		MechSquad.PhotonCustomEventReceiver.UnregistCustomEvent("GGGGG");
	}
}
