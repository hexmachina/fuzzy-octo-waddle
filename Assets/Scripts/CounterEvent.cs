using EventUtils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CounterEvent : MonoBehaviour
{
	[SerializeField] private int maxCount;

	public UnityEvent onCountComplete;
	public UnityFloatEvent onPercentCahnged;

	private int _count;
	private bool completed = false;
	public void Increment()
	{
		++_count;
		if (_count >= maxCount && !completed)
		{
			completed = true;
			onCountComplete.Invoke();
		}
		var ratio = Mathf.Clamp01((float)_count / (float)maxCount);
		onPercentCahnged.Invoke(ratio);
	}

	public void ResetCount()
	{
		completed = false;
		_count = 0;
	}
}
