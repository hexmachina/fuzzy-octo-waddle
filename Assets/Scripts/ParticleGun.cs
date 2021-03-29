using System.Collections;
using UnityEngine;
using EventUtils;

namespace Assets.Scripts
{
	public class ParticleGun : MonoBehaviour
	{
		[SerializeField] Transform target;
		ParticleSystem _particleSystem;

		public UnityGameObjectEvent onTargetCollision;
		Coroutine coroutine;
		WaitForSeconds wait;
		private void Awake()
		{
			wait = new WaitForSeconds(1);
			TryGetComponent(out _particleSystem);
		}

		private void OnEnable()
		{
			coroutine = StartCoroutine(Emitting());
		}

		private void OnDisable()
		{
			if (coroutine != null)
			{
				StopCoroutine(Emitting());
				coroutine = null;
			}
		}


		IEnumerator Emitting()
		{
			while (true)
			{
				yield return wait;
				_particleSystem?.Emit(1);
			}
		}

		private void FixedUpdate()
		{
			transform.LookAt(target);
		}

		private void OnParticleCollision(GameObject other)
		{
			if (other == target.gameObject)
			{
				onTargetCollision.Invoke(other);
			}
		}
	}
}