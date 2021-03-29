using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class RenderLayerChanger : MonoBehaviour
{
	private Renderer _renderer;

	[RenderLayerMask] public uint primaryLayerMask = 0;
	[RenderLayerMask] public uint secondaryLayerMask = 0;

	private uint initialLayerMask;

	//private bool isInitialized = false;

	public void SetFlag(uint flag)
	{
		_renderer.renderingLayerMask |= flag;
	}

	public void UnsetFlag(uint flag)
	{
		_renderer.renderingLayerMask &= ~flag;
	}

	public bool HasFlag(uint a, uint b)
	{
		return (a & b) == b;
	}

	private void Awake()
	{
		if (TryGetComponent(out _renderer))
		{

			initialLayerMask = _renderer.renderingLayerMask;
		}
	}

	//private void Initialized()
	//{
	//	if (isInitialized)
	//		return;
	//	initialLayerMask = _renderer.renderingLayerMask;
	//	isInitialized = true;
	//}

	public void RestoreLayerMask()
	{
		_renderer.renderingLayerMask = initialLayerMask;
	}

	public void TogglePrimary(bool value)
	{
		//Initialized();
		if (!_renderer)
		{
			TryGetComponent(out _renderer);
		}

		if (value && !HasFlag(_renderer.renderingLayerMask, primaryLayerMask))
		{
			SetFlag(primaryLayerMask);
		}
		else if (!value && HasFlag(_renderer.renderingLayerMask, primaryLayerMask))
		{
			UnsetFlag(primaryLayerMask);
		}
	}

	public void ToggleSecondary(bool value)
	{
		//Initialized();

		if (value && !HasFlag(_renderer.renderingLayerMask, secondaryLayerMask))
		{
			SetFlag(secondaryLayerMask);
		}
		else if (!value && HasFlag(_renderer.renderingLayerMask, secondaryLayerMask))
		{
			UnsetFlag(secondaryLayerMask);
		}
	}
}