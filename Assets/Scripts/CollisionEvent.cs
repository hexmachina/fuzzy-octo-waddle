using Cinemachine;
using EventUtils;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts
{
	public class CollisionEvent : MonoBehaviour
	{

		[TagField]
		public string _tag = string.Empty;
		public UnityColliderEvent onTaggedEntered;
		private void OnCollisionEnter(Collision collision)
		{
			if (collision.collider.CompareTag(_tag))
			{
				onTaggedEntered.Invoke(collision.collider);
			}
		}
	}
}