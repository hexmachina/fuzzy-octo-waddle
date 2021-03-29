using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Character
{

	[RequireComponent(typeof(CharacterMover))]
	public class CharacterInputProvider : MonoBehaviour
	{
		public InputActionReference move;
		public InputActionReference sprint;
		public InputActionReference jump;

		[SerializeField] private bool _canMove = true;

		public bool canMove
		{
			get => _canMove; set
			{
				if (value == _canMove)
					return;

				_canMove = value;
				if (_canMove)
				{
					if (move)
					{
						move.action.performed += MovementInput;
						move.action.canceled += MovementInput;
						var delta = move.action.ReadValue<Vector2>();
						z = Mathf.Clamp(delta.x, -1, 1);
						x = Mathf.Clamp(delta.y, -1, 1);
					}
				}
				else
				{
					if (move)
					{
						move.action.performed -= MovementInput;
						move.action.canceled -= MovementInput;
					}
					z = x = 0;
				}
			}
		}

		[SerializeField] private bool _canJump = true;

		public bool canJump
		{
			get => _canJump; set
			{
				if (value == _canJump)
					return;

				_canJump = value;
				if (_canJump)
				{
					if (jump)
					{
						jump.action.started += OnJumpAction;
						jump.action.canceled += OnJumpAction;

					}
				}
				else
				{
					if (jump)
					{
						jump.action.started -= OnJumpAction;
						jump.action.canceled -= OnJumpAction;
					}

				}
			}
		}


		private CharacterMover m_Character;
		private Transform _Cam;
		private Vector3 _CamForward;
		private Vector3 _Move;
		private bool _Jump;
		private bool _Running;

		private float z;
		private float x;

		void Start()
		{

			if (Camera.main) _Cam = Camera.main.transform;

			TryGetComponent(out m_Character);
			if (jump && _canJump)
			{
				jump.action.started += OnJumpAction;
				jump.action.canceled += OnJumpAction;
			}
			if (sprint)
			{
				sprint.action.started += OnSprintAction;
				sprint.action.canceled += OnSprintAction;
			}


			if (move && _canMove)
			{
				move.action.performed += MovementInput;
				move.action.canceled += MovementInput;
				var delta = move.action.ReadValue<Vector2>();
				z = Mathf.Clamp(delta.x, -1, 1);
				x = Mathf.Clamp(delta.y, -1, 1);
			}
		}

		private void OnJumpAction(InputAction.CallbackContext obj)
		{
			switch (obj.phase)
			{
				case InputActionPhase.Disabled:
					break;
				case InputActionPhase.Waiting:
					break;
				case InputActionPhase.Performed:
					break;
				case InputActionPhase.Started:
					if (!_Jump)
						_Jump = true;
					break;
				case InputActionPhase.Canceled:
					if (_Jump)
						_Jump = false;
					break;
				default:
					break;
			}

		}

		private void OnSprintAction(InputAction.CallbackContext obj)
		{
			switch (obj.phase)
			{
				case InputActionPhase.Disabled:
					break;
				case InputActionPhase.Waiting:
					break;
				case InputActionPhase.Started:
				case InputActionPhase.Performed:
					if (!_Running)
						_Running = true;
					break;
				case InputActionPhase.Canceled:
					if (_Running)
						_Running = false;
					break;
				default:
					break;
			}

		}



		public void MovementInput(InputAction.CallbackContext context)
		{
			var delta = context.ReadValue<Vector2>();
			z = Mathf.Clamp(delta.x, -1, 1);
			x = Mathf.Clamp(delta.y, -1, 1);
		}

		private void FixedUpdate()
		{

			if (_Cam != null)
			{

				_CamForward = Vector3.Scale(_Cam.forward, new Vector3(1, 0, 1)).normalized;
				_Move = x * _CamForward + z * _Cam.right;
			}
			else
			{

				_Move = x * Vector3.forward + z * Vector3.right;
			}

			if (_Running)
			{
				_Move *= 1.5f;
			}

			m_Character.Move(_Move, _Jump);
			_Jump = false;
		}
	}
}

