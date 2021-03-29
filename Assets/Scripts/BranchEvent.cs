using EventUtils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BranchEvent : MonoBehaviour
{
	public UnityEvent onPostive;
	public UnityEvent onNegative;

	public UnityBoolEvent onInvertedValueChanged;

	public void OnCallback(bool value)
	{
		if (value)
		{
			onPostive.Invoke();
		}
		else
		{
			onNegative.Invoke();
		}
		onInvertedValueChanged.Invoke(!value);
	}
}
