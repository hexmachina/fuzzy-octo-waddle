using EventUtils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleReceiver : MonoBehaviour
{
	public UnityBoolEvent onValueChanged;
	public void Toggle(bool value)
	{
		onValueChanged.Invoke(value);
	}
}
