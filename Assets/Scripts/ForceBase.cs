using Cinemachine;
using System.Collections;
using UnityEngine;



public abstract class ForceBase : MonoBehaviour
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

	[SerializeField, TagField]
	protected string _tag = string.Empty;

	[SerializeField] protected Direction _direction = Direction.Forward;
	[SerializeField] protected ForceMode _forceMode;

	public float force = 2;

	protected Vector3 GetDirection(Direction direction)
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
