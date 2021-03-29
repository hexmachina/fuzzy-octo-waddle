using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PhysicsRotator : MonoBehaviour
{
	public float speed = 2;

	private Rigidbody _rigidbody;

	private void Awake()
	{
		TryGetComponent(out _rigidbody);
	}

	private void FixedUpdate()
	{
		_rigidbody.maxAngularVelocity = float.MaxValue;
		_rigidbody?.AddTorque(Vector3.right);
	}
}
