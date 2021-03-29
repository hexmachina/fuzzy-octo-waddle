using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleMessenger : MonoBehaviour
{
	[SerializeField] private bool _isOn;

	public void SendToReceiver(Component component)
	{
		if (component.TryGetComponent(out ToggleReceiver receiver))
		{
			receiver.Toggle(_isOn);
		}
	}

	public void SendToReceiverByGameObject(GameObject component)
	{
		if (component.TryGetComponent(out ToggleReceiver receiver))
		{
			receiver.Toggle(_isOn);
		}
	}
}
