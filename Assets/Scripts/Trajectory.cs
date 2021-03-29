using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Trajectory
{

	public static TrajectoryData CalculateTrajectoryData(Vector3 start, Vector3 target, float gravity)
	{
		float displacementY = target.y - start.y;
		float h = Mathf.Abs(displacementY * 1f);
		Vector3 displacementXZ = new Vector3(target.x - start.x, 0, target.z - start.z);
		float time = Mathf.Sqrt(-2 * h / gravity) + Mathf.Sqrt(2 * (displacementY - h) / gravity);
		Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * h);
		Vector3 velocityXZ = displacementXZ / time;

		return new TrajectoryData(velocityXZ + velocityY * -Mathf.Sign(gravity), time);
	}

	public static void TrajectoryPath(ref Vector3[] points, Vector3 start, Vector3 target, float gravity)
	{
		TrajectoryData launchData = CalculateTrajectoryData(start, target, gravity);
		//Vector3 previousDrawPoint = start;

		//points = new Vector3[resolution];
		//points[0] = start;
		//int resolution = 30;
		for (int i = 0; i < points.Length; i++)
		{
			float simulationTime = i / (float)points.Length * launchData.timeToTarget;
			Vector3 displacement = launchData.initialVelocity * simulationTime + Vector3.up * gravity * simulationTime * simulationTime / 2f;
			Vector3 drawPoint = start + displacement;
			points[i] = drawPoint;
			//Debug.DrawLine(previousDrawPoint, drawPoint, Color.green);
			//previousDrawPoint = drawPoint;
		}
	}

	public struct TrajectoryData
	{
		public readonly Vector3 initialVelocity;
		public readonly float timeToTarget;

		public TrajectoryData(Vector3 initialVelocity, float timeToTarget)
		{
			this.initialVelocity = initialVelocity;
			this.timeToTarget = timeToTarget;
		}

	}

	public static bool GetAimTarget(out RaycastHit hit, Vector2 minMax, Vector3 source, Transform aim, LayerMask layerMask, string m_IgnoreTag)
	{
		var aimPos = aim.position;
		var aimForward = aim.forward;
		//var pos = source.position;

		var posDist = Vector3.Distance(aimPos, source);
		aimPos += aimForward * posDist;
		Debug.DrawLine(aim.position, aimPos, Color.red);
		//hit.point = aimPos + (aimForward * minMax.y);

		Ray ray = new Ray(aimPos, aimForward);

		if (RuntimeUtility.RaycastIgnoreTag(ray, out hit, minMax.y, layerMask, m_IgnoreTag))
		{
			//target = hit.point;
			Debug.DrawLine(aimPos, hit.point, Color.green);

		}
		else if (Physics.Raycast(aimPos + (aimForward * minMax.y), Vector3.down, out hit, minMax.y, layerMask, QueryTriggerInteraction.Ignore))
		{
			Debug.DrawLine(aimPos, hit.point, Color.yellow);
			//target = hit.point;
		}
		else
		{
			hit.point = aimPos + (aimForward * minMax.y);
			return false;
		}

		var dist = Vector3.Distance(source, hit.point);

		return dist > minMax.x;
	}
}
