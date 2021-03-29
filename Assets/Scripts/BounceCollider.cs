using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceCollider : MonoBehaviour
{
	public enum Direction
	{
		Up,
		Down,
		Left,
		Right,
		Forward,
		Back
	}

	[TagField]
	public string _tag = string.Empty;

	public Direction _direction = Direction.Forward;

	public float force = 2;

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.collider.CompareTag(_tag))
		{
			if (collision.collider.TryGetComponent(out Rigidbody rb))
			{
				rb.AddForce(GetDirection(_direction) * force, ForceMode.VelocityChange);
			}
		}
	}

	private Vector3 GetDirection(Direction direction)
	{
		switch (direction)
		{
			case Direction.Up:
				return transform.up;
			case Direction.Down:
				return -transform.up;
			case Direction.Left:
				return -transform.right;
			case Direction.Right:
				return transform.right;
			case Direction.Forward:
			default:
				return transform.forward;
			case Direction.Back:
				return -transform.forward;
		}
	}
}
