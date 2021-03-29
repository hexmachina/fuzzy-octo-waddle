using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimatorParameterSetter : MonoBehaviour
{
	[SerializeField] string _parameter;
	Animator _animator;

	private void Awake()
	{
		TryGetComponent(out _animator);

	}

	public void SetBool(bool value)
	{
		if (string.IsNullOrEmpty(_parameter))
			return;

		_animator?.SetBool(_parameter, value);
	}

	public void SetTrigger()
	{
		if (string.IsNullOrEmpty(_parameter))
			return;

		_animator?.SetTrigger(_parameter);
	}
}
