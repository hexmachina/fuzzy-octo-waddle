using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Character
{
	[RequireComponent(typeof(CapsuleCollider))]
	[RequireComponent(typeof(Rigidbody))]
	public class CharacterMover : MonoBehaviour
	{
		public float speed = 5f;
		public float turnSpeed = 2f;
		public bool airJump;
		public float jumpHeight = 2f;
		public float groundDistance = 0.2f;
		public LayerMask GroundLayer;
		public UnityEvent onGrounded;
		public UnityEvent onJumped;

		[Range(1f, 4f)] [SerializeField] float _GravityMultiplier = 2f;


		[SerializeField] private bool _canConstrain = true;
		public bool canConstrain { get => _canConstrain; set => _canConstrain = value; }

		[SerializeField] Transform _constrainer;
		public Transform constrainer { get => _constrainer; set => _constrainer = value; }

		[SerializeField] private float _constrainedRadius = 5;
		public float constrainedRadius { get => _constrainedRadius; set => _constrainedRadius = value; }

		Rigidbody _rigidbody;
		CapsuleCollider _capsule;

		private bool _isGrounded = true;

		// Use this for initialization
		void Start()
		{
			TryGetComponent(out _rigidbody);
			TryGetComponent(out _capsule);
			_rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;

		}

		public void Move(Vector3 move, bool jump)
		{

			var capsuleOffset = transform.TransformPoint(_capsule.center) + Vector3.down * (_capsule.height * 0.5f);
			var oldGround = _isGrounded;
			_isGrounded = Physics.CheckSphere(capsuleOffset, groundDistance, GroundLayer, QueryTriggerInteraction.Ignore);

			if (oldGround != _isGrounded && _isGrounded)
			{
				onGrounded.Invoke();
			}

			if (jump && (_isGrounded || airJump) && !AboveBounds())
			{
				onJumped.Invoke();
				var ratio = ConstrainHeightRatio();
				_rigidbody.AddForce(Vector3.up * Mathf.Sqrt(jumpHeight * ratio * -2f * Physics.gravity.y), ForceMode.Impulse);
			}
			ApplyTurnRotation(move);

			_rigidbody.MovePosition(_rigidbody.position + move * speed * Time.fixedDeltaTime);
			ApplyConstraints(move);
		}

		private void ApplyTurnRotation(Vector3 move)
		{

			if (move.magnitude > 0.01f)
			{
				move.y = 0;
				var forward = transform.forward;
				forward.y = 0;
				var v = Vector3.Lerp(transform.forward, move, turnSpeed * Time.fixedDeltaTime);
				transform.rotation = Quaternion.LookRotation(v, Vector3.up);
			}

		}

		private void ApplyConstraints(Vector3 move)
		{
			if (!_constrainer || !_canConstrain)
				return;

			var heading = _constrainer.position - _rigidbody.position;
			if (heading.magnitude > _constrainedRadius)
			{
				Vector3 v = heading.normalized;

				var dist = Mathf.Abs(heading.y) - _constrainedRadius;
				if (dist > 0)
				{
					if (_rigidbody.velocity.y > 0)
					{
						var rv = _rigidbody.velocity;
						rv.y = 0;
						_rigidbody.velocity = rv;
					}
					if (Vector3.Dot(v, Vector3.up) > 0)
					{
						//Debug.Log("hello");
						v.y += -Physics.gravity.y * dist;
					}
					else
					{
						//v.y += Physics.gravity.y;
						v.y += Physics.gravity.y * dist;
					}
				}

				var sp = speed * Mathf.Max(heading.magnitude - _constrainedRadius, 0.5f);
				_rigidbody.MovePosition(_rigidbody.position + v * sp * Time.fixedDeltaTime);
				//if (move.magnitude > 0.1)
				//{

				//}
				//else
				//{
				//	_rigidbody.AddForce(v * sp * Time.fixedDeltaTime, ForceMode.VelocityChange);
				//}
			}

		}

		private bool AboveBounds()
		{
			if (!_constrainer || !_canConstrain)
				return false;

			var heading = _constrainer.position - _rigidbody.position;
			if (heading.magnitude > _constrainedRadius)
			{
				var dist = Mathf.Abs(heading.y) - _constrainedRadius;
				if (dist > 0)
				{
					Vector3 v = heading.normalized;
					return Vector3.Dot(v, Vector3.up) < 0;

				}
			}
			return false;
		}

		private float ConstrainHeightRatio()
		{
			if (!_constrainer || !_canConstrain)
				return 1;

			var heading = _constrainer.position - _rigidbody.position;
			var dist = _constrainedRadius - Mathf.Abs(heading.y);

			var ratio = Mathf.Abs(dist / _constrainedRadius);
			if (ratio < 0.15f)
			{
				Vector3 v = heading.normalized;
				if (Vector3.Dot(v, Vector3.up) < 0)
				{
					return Mathf.Max(1 - ratio, 0.1f);
				}
			}

			return 1;
		}

		private bool OutOfBounds()
		{
			if (!_constrainer || !_canConstrain)
				return false;

			var heading = _constrainer.position - _rigidbody.position;
			return heading.magnitude > _constrainedRadius;

		}
	}
}