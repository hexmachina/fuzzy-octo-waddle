using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using UnityEngine.Scripting.APIUpdating;


public class RenderLayerObjectFeature : ScriptableRendererFeature
{
	public enum RenderQueueType
	{
		Opaque,
		Transparent,
	}
	[System.Serializable]
	public class RenderObjectsSettings
	{
		public string passTag = "RenderObjectsFeature";
		public RenderPassEvent Event = RenderPassEvent.AfterRenderingOpaques;

		public FilterSettings filterSettings = new FilterSettings();

		public Material overrideMaterial = null;
		public int overrideMaterialPassIndex = 0;

		public bool overrideDepthState = false;
		public CompareFunction depthCompareFunction = CompareFunction.LessEqual;
		public bool enableWrite = true;

		public StencilStateData stencilSettings = new StencilStateData();

		public CustomCameraSettings cameraSettings = new CustomCameraSettings();
	}

	[System.Serializable]
	public class FilterSettings
	{
		// TODO: expose opaque, transparent, all ranges as drop down
		public RenderQueueType RenderQueueType;
		//public LayerMask LayerMask;
		[RenderLayerMask] public uint RenderLayerMask;
		public string[] PassNames;
		public FilterSettings()
		{
			RenderQueueType = RenderQueueType.Opaque;
			//LayerMask = 0;
			RenderLayerMask = 1;
		}
	}

	[System.Serializable]
	public class CustomCameraSettings
	{
		public bool overrideCamera = false;
		public bool restoreCamera = true;
		public Vector4 offset;
		public float cameraFieldOfView = 60.0f;
	}

	public RenderObjectsSettings settings = new RenderObjectsSettings();

	RenderLayerObjectPass renderObjectsPass;

	public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
	{
		renderer.EnqueuePass(renderObjectsPass);

	}

	public override void Create()
	{
		FilterSettings filter = settings.filterSettings;
		renderObjectsPass = new RenderLayerObjectPass(settings.passTag, settings.Event, filter.PassNames,
			filter.RenderQueueType, filter.RenderLayerMask, settings.cameraSettings);

		renderObjectsPass.overrideMaterial = settings.overrideMaterial;
		renderObjectsPass.overrideMaterialPassIndex = settings.overrideMaterialPassIndex;

		if (settings.overrideDepthState)
			renderObjectsPass.SetDetphState(settings.enableWrite, settings.depthCompareFunction);

		if (settings.stencilSettings.overrideStencilState)
			renderObjectsPass.SetStencilState(settings.stencilSettings.stencilReference,
				settings.stencilSettings.stencilCompareFunction, settings.stencilSettings.passOperation,
				settings.stencilSettings.failOperation, settings.stencilSettings.zFailOperation);
	}
}
