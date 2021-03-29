using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using EventUtils;

public class EnableEvent : MonoBehaviour
{
	public UnityEvent onEnabled = new UnityEvent();

	public UnityEvent onDisabled = new UnityEvent();

	public UnityBoolEvent onEnableChanged;
	public UnityBoolEvent onEnableChangedInverted;

	private void OnEnable()
	{
		onEnabled.Invoke();
		onEnableChanged.Invoke(true);
		onEnableChangedInverted.Invoke(false);
	}

	private void OnDisable()
	{
		onDisabled.Invoke();
		onEnableChanged.Invoke(false);
		onEnableChangedInverted.Invoke(true);
	}
}
