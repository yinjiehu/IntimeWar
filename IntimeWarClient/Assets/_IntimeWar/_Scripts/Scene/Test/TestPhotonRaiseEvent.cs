using UnityEngine;

public class TestPhotonRaiseEvent : MonoBehaviour
{
	private void Start()
	{
		MechSquad.PhotonCustomEventReceiver.RegistCustomEvent("abcde", obj =>
		{
			Debug.LogFormat("receive event abcde! {0}", obj);
		});
	}


	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			MechSquad.PhotonCustomEventSender.RaisePhotonCustomEvent("abcde", "aaaaaaaaaaaaaaaaaa");
		}
	}
}
