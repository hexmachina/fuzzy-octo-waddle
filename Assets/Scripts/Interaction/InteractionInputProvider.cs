using EventUtils;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Interaction
{
	[System.Serializable]
	public class UnityInteractorEvent : UnityEvent<InteractionInputProvider> { }

	[System.Serializable]
	public class UnityInteractableEvent : UnityEvent<InteractableObject> { }

	public class InteractionInputProvider : MonoBehaviour
	{

		public InputActionReference interact;
		public InputActionReference agility;

		[SerializeField] Vector3 castOffset;
		public float castRadius = 0.2f;
		public float castDistance = 4f;
		public LayerMask interactMask;
		public LayerMask cullMask;


		public Transform castReference;
		public UnityBoolEvent onCanInteract;
		public UnityInteractableEvent onInteract;
		public UnityInteractableEvent onAgility;

		private InteractableObject target;

		private void OnEnable()
		{
			interact.action.started += OnInteractStarted;
			if (agility)
			{
				agility.action.started += OnAgilityStarted;

			}
		}

		private void OnAgilityStarted(InputAction.CallbackContext obj)
		{
			if (!target)
				return;
			onAgility.Invoke(target);
		}

		private void OnDisable()
		{
			interact.action.started -= OnInteractStarted;
			if (agility)
				agility.action.started -= OnAgilityStarted;

			ClearTarget();

		}
		private void OnInteractStarted(InputAction.CallbackContext obj)
		{
			if (!target)
				return;
			target.Interact(this);
			onInteract.Invoke(target);
		}

		public void CheckInteraction(Collider collider)
		{
			if (collider)
			{
				var old = target;
				if (collider.TryGetComponent(out target) && target.enabled)
				{
					if (old && old != target)
						old.SetFocus(false);

					if (old != target)
					{
						target.SetFocus(true);
						onCanInteract.Invoke(true);
					}


				}
				else
				{
					ClearTarget();
				}
			}
			else
			{
				ClearTarget();
			}
		}

		private void ClearTarget()
		{
			if (target)
			{
				target.SetFocus(false);
				target = null;
				onCanInteract.Invoke(false);
			}
		}

		private void FixedUpdate()
		{
			if (!castReference)
				return;
			var origin = castReference.TransformPoint(castOffset);
			if (Physics.SphereCast(origin, castRadius, castReference.forward, out RaycastHit hit, castDistance, interactMask, QueryTriggerInteraction.Ignore))
			{

				Debug.DrawRay(origin, -hit.normal * hit.distance, Color.green);
				if (!Physics.Raycast(origin, -hit.normal, hit.distance, cullMask, QueryTriggerInteraction.Ignore))
				{
					CheckInteraction(hit.collider);

				}
				else
				{
					ClearTarget();
				}
			}
			else
			{
				ClearTarget();
			}
		}

		private void OnDrawGizmos()
		{
			if (!castReference)
				return;

			if (target)
			{
				Gizmos.color = Color.cyan;

			}
			else
			{
				Gizmos.color = Color.yellow;
			}
			Gizmos.DrawWireSphere(castReference.TransformPoint(castOffset), castRadius);
			Gizmos.DrawRay(castReference.TransformPoint(castOffset), castReference.forward * castDistance);
			Gizmos.DrawWireSphere(castReference.TransformPoint(castOffset) + castReference.forward * castDistance, castRadius);
		}
	}
}