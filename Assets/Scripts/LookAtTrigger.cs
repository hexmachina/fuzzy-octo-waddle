using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtTrigger : MonoBehaviour
{

	[TagField]
	public string _tag = string.Empty;

	public float turnSpeed = 5;

	Quaternion initRotation;

	Coroutine coroutine;
	private void Awake()
	{
		initRotation = transform.rotation;
	}

	private void OnTriggerStay(Collider other)
	{
		if (other.CompareTag(_tag))
		{
			if (coroutine != null)
			{
				StopCoroutine(coroutine);
				coroutine = null;
			}
			var heading = other.transform.position - transform.position;
			heading.y = 0;
			transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(heading), turnSpeed * Time.deltaTime);

		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag(_tag))
		{
			if (coroutine == null)
			{
				coroutine = StartCoroutine(RestoringPosition());
			}
		}
	}

	IEnumerator RestoringPosition()
	{
		float count = 0;
		while (count < 1)
		{
			count += turnSpeed * Time.deltaTime;
			transform.rotation = Quaternion.Slerp(transform.rotation, initRotation, count);
			yield return null;
		}
		transform.rotation = initRotation;
	}
}
