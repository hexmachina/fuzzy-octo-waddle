using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceTrigger : ForceBase
{
	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag(_tag))
		{
			if (other.TryGetComponent(out Rigidbody rb))
			{
				rb.AddForce(GetDirection(_direction) * force, _forceMode);
			}
		}
	}
}
