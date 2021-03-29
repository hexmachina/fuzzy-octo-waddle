using Cinemachine;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
	public class ForceApplier : MonoBehaviour
	{
		[TagField]
		public string _tag = string.Empty;
		public float force = 2;

		public void AddForceByComponent(Component component)
		{
			if (component.CompareTag(_tag))
			{
				if (component.TryGetComponent(out Rigidbody rb))
				{
					rb.AddForce(transform.forward * force, ForceMode.VelocityChange);
				}
			}
		}
	}
}