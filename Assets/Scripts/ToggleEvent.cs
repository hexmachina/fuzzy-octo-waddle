using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using EventUtils;

public class ToggleEvent : MonoBehaviour
{
	[SerializeField] bool _value;

	public bool value => _value;

	[SerializeField] bool invokeOnAwake;

	public UnityBoolEvent onValueChanged = new UnityBoolEvent();

	public UnityEvent onPositive = new UnityEvent();
	public UnityEvent onNegative = new UnityEvent();

	private void Awake()
	{
		if (invokeOnAwake)
		{
			ApplyValue(_value);
		}
	}

	public void Toggle()
	{
		_value = !_value;
		ApplyValue(_value);
	}

	public void ApplyValue(bool value)
	{
		onValueChanged.Invoke(value);
		if (value)
		{
			onPositive.Invoke();
		}
		else
		{
			onNegative.Invoke();
		}

	}
}
