using UnityEngine;

public class ShakeTest : MonoBehaviour
{
	[SerializeField]
	float _magnitude;
	[SerializeField]
	float _rough;
	[SerializeField]
	float _fadeIn;
	[SerializeField]
	float _fadeOut;

	[SerializeField]
	Haruna.Inspector.InspectorButton _shake;

	public void Shake()
	{
		//EZCameraShake.CameraShaker.Instance.ShakeOnce(_magnitude, _rough, _fadeIn, _fadeOut);
	}
	

}
