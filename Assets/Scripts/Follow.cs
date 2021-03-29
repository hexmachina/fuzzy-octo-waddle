using System.Collections;
using UnityEngine;
using EventUtils;

namespace Assets.Scripts
{
	public class Follow : MonoBehaviour
	{

		[SerializeField] float speed = 4f;
		[SerializeField] float radius = 5f;
		[SerializeField] Vector3 offset;
		public float turnSpeed = 5;
		[SerializeField] Transform _target;
		public UnityBoolEvent onInRange;
		private bool inRange;


		// Update is called once per frame
		void Update()
		{
			if (!_target)
				return;

			var heading = _target.position - transform.position;
			var value = heading.magnitude > radius;
			if (value)
			{
				transform.position = Vector3.Lerp(transform.position, _target.position + offset, speed * Time.deltaTime);
			}
			SetRange(value);
			heading.y = 0;
			transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(heading), turnSpeed * Time.deltaTime);
		}

		private void SetRange(bool value)
		{
			if (value == inRange)
				return;
			inRange = value;
			onInRange.Invoke(inRange);
		}
	}
}