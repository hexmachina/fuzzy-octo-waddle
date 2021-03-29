using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class ProjectileTrajectory : MonoBehaviour
{
	[SerializeField] private LayerMask layerMask;

	[TagField]
	[Tooltip("Obstacles with this tag will be ignored.  It is a good idea to set this field to the target's tag")]
	public string m_IgnoreTag = string.Empty;

	//public Transform source;
	public Transform reference;

	[SerializeField] private Transform targetView;

	[SerializeField]
	private LayerMask targetLayerMask;

	private float targetRadius = 0.5f;

	public bool useCameraDirection;

	[SerializeField] private float _minDistance = 1;
	public float minDistance { get { return _minDistance; } set { _minDistance = value; } }

	[SerializeField] private float _distance = 10;
	public float distance { get { return _distance; } set { _distance = value; } }

	Vector3 offset;

	[SerializeField] private int _resolution;
	//public int resolution { get { return _resolution; } set { _resolution = value; } }

	//[SerializeField, AttachComponent]
	private LineRenderer lr;

	private Vector3[] vectors;

	private Camera cam;

	private void Awake()
	{
		lr = GetComponent<LineRenderer>();
		vectors = new Vector3[_resolution];
		cam = Camera.main;

	}

	private void FixedUpdate()
	{
		RenderArc();
	}

	private void RenderArc()
	{

		var camPos = cam.transform.position;
		if (reference)
		{
			var refDist = Vector3.Distance(camPos, reference.position);
			camPos += cam.transform.forward * refDist;
		}
		var target = camPos + (cam.transform.forward * _distance);
		//Transform lockon = null;
		RaycastHit hit;
		bool hasHit = false;
		//if (RuntimeUtility.SphereCastIgnoreTag(camPos, targetRadius, cam.transform.forward, out hit, _distance, targetLayerMask, m_IgnoreTag))
		//{
		//	hasHit = true;
		//	var heading = hit.point - camPos;
		//	Ray headinRay = new Ray(camPos, heading.normalized);

		//	if (RuntimeUtility.RaycastIgnoreTag(headinRay, out hit, _distance, layerMask, m_IgnoreTag))
		//	{
		//		target = hit.point;
		//		if ((targetLayerMask & 1 << hit.collider.gameObject.layer) == 1 << hit.collider.gameObject.layer)
		//		{
		//			if (hit.collider.TryGetComponent(out Animator animator) && animator.isHuman)
		//			{
		//				lockon = animator.GetBoneTransform(HumanBodyBones.Head);
		//				target = lockon.position;
		//			}
		//		}
		//	}
		//}

		//if (!lockon)
		//{

		//	Ray ray = new Ray(camPos, cam.transform.forward);

		//	if (RuntimeUtility.RaycastIgnoreTag(ray, out hit, _distance, layerMask, m_IgnoreTag))
		//	{
		//		hasHit = true;
		//		target = hit.point;
		//		if ((targetLayerMask & 1 << hit.collider.gameObject.layer) == 1 << hit.collider.gameObject.layer)
		//		{
		//			if (hit.collider.TryGetComponent(out Animator animator) && animator.isHuman)
		//			{
		//				lockon = animator.GetBoneTransform(HumanBodyBones.Head);
		//				target = lockon.position;
		//			}
		//		}
		//	}
		//	else
		//	if (Physics.Raycast(camPos + (cam.transform.forward * _distance), Vector3.down, out hit, _distance, layerMask, QueryTriggerInteraction.Ignore))
		//	{
		//		target = hit.point;
		//	}
		//}

		if (Trajectory.GetAimTarget(out hit, new Vector2(minDistance, _distance), reference.position, cam.transform, layerMask, m_IgnoreTag))
		{
			hasHit = true;
			target = hit.point;
		}


		Vector3 soPos;
		if (reference)
		{
			soPos = reference.TransformPoint(offset);
		}
		else
		{
			soPos = transform.position;
		}
		var dist = Vector3.Distance(soPos, target);
		//if (dist > minDistance)
		if (hasHit)
		{
			lr.enabled = true;
			Trajectory.TrajectoryPath(ref vectors, soPos, target, Physics.gravity.y);
		}
		else
		{
			lr.enabled = false;
		}

		lr.positionCount = vectors.Length;
		lr.SetPositions(vectors);

		if (targetView)
		{
			if (targetView.gameObject.activeSelf != hasHit)
			{
				targetView.gameObject.SetActive(hasHit);
			}
			if (hasHit)
			{
				targetView.position = target;
				if (hit.normal == Vector3.zero)
				{
					targetView.rotation = Quaternion.LookRotation(Vector3.up, Vector3.up);

				}
				else
				{
					targetView.rotation = Quaternion.LookRotation(Vector3.forward, hit.normal);

				}
			}
			else
			{
				targetView.position = transform.position;

			}

		}
	}

	private void OnDisable()
	{
		if (targetView)
		{
			targetView.position = transform.position;
		}

	}

	//public void AimChanged(bool value, Pickups.AimData data)
	//{
	//	gameObject.SetActive(value);
	//	if (value)
	//	{
	//		reference = data.reference;
	//		distance = data.distance;
	//		minDistance = data.minDistance;
	//		layerMask = data.layerMask;
	//		m_IgnoreTag = data.tag;
	//		offset = data.offset;
	//		targetRadius = data.targetRadius;
	//		RenderArc();
	//	}
	//	else
	//	{
	//		reference = null;
	//	}
	//	//_velocity = vel;
	//}

}
