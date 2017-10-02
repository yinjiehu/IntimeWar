using UnityEngine;
using UnityEngine.UI;

public class UIPanel : MonoBehaviour
{
	public Color enabledColor;
	public Color disabledColor;


	public void SetColor(bool enabled)
	{
		GetComponent<Image>().color = enabled ? enabledColor : disabledColor;
	}
}