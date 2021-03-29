using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class InputProvider : MonoBehaviour
{
	[System.Serializable]
	public class UnityInputActionContext : UnityEvent<InputAction.CallbackContext> { }

	public InputActionReference _input;

	public UnityInputActionContext onStarted;
	public UnityInputActionContext onPerformed;
	public UnityInputActionContext onCancelled;

	private void OnEnable()
	{
		_input.action.started += OnActionStarted;
		_input.action.performed += OnActionPerformed;
		_input.action.canceled += OnActionCancelled;
	}

	private void OnActionStarted(InputAction.CallbackContext obj)
	{
		onStarted.Invoke(obj);
	}

	private void OnActionPerformed(InputAction.CallbackContext obj)
	{
		onPerformed.Invoke(obj);
	}

	private void OnActionCancelled(InputAction.CallbackContext obj)
	{
		onCancelled.Invoke(obj);
	}

	private void OnDisable()
	{
		_input.action.started -= OnActionStarted;
		_input.action.performed -= OnActionPerformed;
		_input.action.canceled -= OnActionCancelled;
	}
}
