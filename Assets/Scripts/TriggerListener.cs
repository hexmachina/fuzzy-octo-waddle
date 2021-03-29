using Cinemachine;
using EventUtils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerListener : MonoBehaviour
{
	[TagField]
	public string _tag = string.Empty;
	public UnityColliderEvent onTaggedEntered;

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag(_tag))
		{
			onTaggedEntered.Invoke(other);
		}
	}
}
