using EventUtils;
using System.Collections;
using UnityEngine;

namespace Interaction
{
	public class InteractableObject : MonoBehaviour
	{
		public UnityBoolEvent onFocused;

		public UnityInteractorEvent onInteract;

		private bool isFocused = false;

		public virtual void SetFocus(bool value)
		{
			if (isFocused == value)
				return;
			isFocused = value;
			onFocused.Invoke(isFocused);
		}

		public virtual void Interact(InteractionInputProvider provider)
		{
			onInteract.Invoke(provider);
		}
	}
}