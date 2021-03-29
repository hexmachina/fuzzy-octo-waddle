using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class RigidbodyModifier : MonoBehaviour
{
	Rigidbody _rigidbody;

	private void Awake()
	{
		TryGetComponent(out _rigidbody);
	}

	public void KillVelocity()
	{
		var v = _rigidbody.velocity;
		if (v.y > 0)
		{
			v.y = 0;
		}
		v.x = v.z = 0;
		_rigidbody.velocity = v;
	}
}
