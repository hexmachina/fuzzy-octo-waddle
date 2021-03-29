using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

namespace Interaction
{
	public class PickupableObject : InteractableObject
	{
		public UnityEvent onDrop;
		public UnityEvent onThrow;
		public UnityEvent onRelease;

		public void Drop()
		{
			onRelease.Invoke();
			onDrop.Invoke();
		}

		public void Throw()
		{
			onRelease.Invoke();
			onThrow.Invoke();
		}
	}

}
