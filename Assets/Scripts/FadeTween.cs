using EventUtils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FadeTween : MonoBehaviour
{
	public float duration = 2;

	public UnityFloatEvent onValueChanged;
	public UnityEvent onTweenCompleted;
	Coroutine coroutine;
	public void StartTween()
	{
		if (coroutine != null)
			return;

		coroutine = StartCoroutine(Tweening());
	}

	public void StartInverseTween()
	{
		if (coroutine != null)
			return;

		coroutine = StartCoroutine(TweeningInverted());
	}

	IEnumerator Tweening()
	{
		float count = 0;

		while (count < duration)
		{
			onValueChanged.Invoke(count / duration);
			count += Time.deltaTime;
			yield return null;
		}
		onValueChanged.Invoke(1);
		onTweenCompleted.Invoke();
		coroutine = null;
	}

	IEnumerator TweeningInverted()
	{
		float count = duration;

		while (count > 0)
		{
			onValueChanged.Invoke(count / duration);
			count -= Time.deltaTime;
			yield return null;
		}
		onValueChanged.Invoke(0);
		onTweenCompleted.Invoke();
		coroutine = null;
	}
}
