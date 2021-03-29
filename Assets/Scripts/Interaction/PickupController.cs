using Cinemachine;
using EventUtils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.InputSystem;

namespace Interaction
{

	public class PickupController : MonoBehaviour
	{
		[SerializeField] private InputActionReference _dropInputAction;

		//[SerializeField] private InputActionReference _aimInputAction;

		[SerializeField] private InputActionReference _throwInputAction;

		public Vector2 throwDistance = new Vector2(1, 20);


		[SerializeField] private LayerMask layerMask;

		[TagField]
		[Tooltip("Obstacles with this tag will be ignored.  It is a good idea to set this field to the target's tag")]
		public string m_IgnoreTag = string.Empty;

		[SerializeField]
		private LayerMask targetLayerMask;

		[SerializeField] private float targetRadius = 0.5f;

		public UnityBoolEvent onHolding;

		[SerializeField] Transform holder;
		private PickupableObject pickupable;

		private Transform cam;

		Vector3 _targetPoint;

		private void Awake()
		{
			cam = Camera.main.transform;
		}

		public void GetObjectByComponent(Component component)
		{
			if (!component.TryGetComponent(out pickupable))
				return;
			SetListeners(true);
			onHolding.Invoke(true);
			Pickup(pickupable.transform);
		}

		private void SetListeners(bool value)
		{
			if (value)
			{
				if (_dropInputAction)
				{
					_dropInputAction.action.started += OnDropAction;
				}
				if (_throwInputAction)
				{
					_throwInputAction.action.canceled += OnThrowCancel;
				}

			}
			else
			{
				if (_dropInputAction)
				{
					_dropInputAction.action.started -= OnDropAction;
				}
				if (_throwInputAction)
				{
					_throwInputAction.action.started -= OnThrowAction;
				}
			}
		}

		private void OnThrowCancel(InputAction.CallbackContext obj)
		{
			_throwInputAction.action.started += OnThrowAction;
			_throwInputAction.action.canceled -= OnThrowCancel;

			//Throw();
		}
		private void OnThrowAction(InputAction.CallbackContext obj)
		{
			Throw();
		}

		private void OnDropAction(InputAction.CallbackContext obj)
		{
			Drop();
		}

		private void Pickup(Transform t)
		{
			if (holder)
			{
				t.parent = holder;
				t.localPosition = Vector3.zero;
			}
		}


		private void Drop()
		{
			if (!pickupable)
				return;
			Release();
			pickupable?.Drop();
			SetListeners(false);
			onHolding.Invoke(false);
		}

		private void Throw()
		{
			if (!pickupable)
				return;
			Transform lockon;
			if (GetTarget(out _targetPoint, throwDistance.x, throwDistance.y, pickupable.transform, cam, out lockon))
			{

				Release();
				pickupable.Throw();
				if (pickupable.transform.TryGetComponent(out Rigidbody rb))
				{
					rb.velocity = Trajectory.CalculateTrajectoryData(pickupable.transform.position, _targetPoint, Physics.gravity.y).initialVelocity;
				}
				SetListeners(false);
				onHolding.Invoke(false);
			}

		}

		private void Release()
		{
			if (pickupable && pickupable.transform.parent == holder)
				pickupable.transform.parent = null;

		}

		public bool GetTarget(out Vector3 target, float min, float max, Transform source, Transform aim, out Transform lockon)
		{
			lockon = null;
			var aimPos = aim.transform.position;
			var aimForward = aim.transform.forward;
			var pos = source.position;

			var posDist = Vector3.Distance(aimPos, pos);
			aimPos += aimForward * posDist;
			RaycastHit hit;
			target = aimPos + (aimForward * max);

			if (RuntimeUtility.SphereCastIgnoreTag(aimPos, targetRadius, cam.forward, out hit, max, targetLayerMask, m_IgnoreTag))
			{
				var heading = hit.point - aimPos;
				Ray headinRay = new Ray(aimPos, heading.normalized);

				if (RuntimeUtility.RaycastIgnoreTag(headinRay, out hit, max, layerMask, m_IgnoreTag))
				{
					target = hit.point;
					if ((targetLayerMask & 1 << hit.collider.gameObject.layer) == 1 << hit.collider.gameObject.layer)
					{
						if (hit.collider.TryGetComponent(out Animator animator) && animator.isHuman)
						{
							lockon = animator.GetBoneTransform(HumanBodyBones.Head);
							target = lockon.position;
						}
					}
				}
			}

			if (!lockon)
			{
				Ray ray = new Ray(aimPos, cam.forward);

				if (RuntimeUtility.RaycastIgnoreTag(ray, out hit, max, layerMask, m_IgnoreTag))
				{
					target = hit.point;
					if ((targetLayerMask & 1 << hit.collider.gameObject.layer) == 1 << hit.collider.gameObject.layer)
					{
						if (hit.collider.TryGetComponent(out Animator animator) && animator.isHuman)
						{
							lockon = animator.GetBoneTransform(HumanBodyBones.Head);
							target = lockon.position;
						}
					}
				}
				else if (Physics.Raycast(aimPos + (cam.forward * max), Vector3.down, out hit, max, layerMask, QueryTriggerInteraction.Ignore))
				{
					target = hit.point;
				}
			}

			var dist = Vector3.Distance(aimPos, target);

			return dist > min;
		}
	}
}
