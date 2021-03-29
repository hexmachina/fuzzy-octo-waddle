using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace EventUtils
{
	[System.Serializable]
	public class UnityBoolEvent : UnityEvent<bool> { }

	[System.Serializable]
	public class UnityFloatEvent : UnityEvent<float> { }

	[System.Serializable]
	public class UnityColliderEvent : UnityEvent<Collider> { }

	[System.Serializable]
	public class UnityGameObjectEvent : UnityEvent<GameObject> { }
}