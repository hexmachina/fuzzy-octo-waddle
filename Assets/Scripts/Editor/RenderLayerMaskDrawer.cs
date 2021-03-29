using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Rendering;

[CustomPropertyDrawer(typeof(RenderLayerMaskAttribute))]
public class RenderLayerMaskDrawer : PropertyDrawer
{

	private static string[] m_DefaultRenderingLayerNames;
	internal static string[] defaultRenderingLayerNames
	{
		get
		{
			if (m_DefaultRenderingLayerNames == null)
			{
				m_DefaultRenderingLayerNames = new string[32];
				for (int i = 0; i < m_DefaultRenderingLayerNames.Length; ++i)
				{
					m_DefaultRenderingLayerNames[i] = string.Format("Layer{0}", i + 1);
				}
			}
			return m_DefaultRenderingLayerNames;
		}
	}

	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		RenderPipelineAsset srpAsset = GraphicsSettings.currentRenderPipeline;
		bool usingSRP = srpAsset != null;
		if (!usingSRP)
		{
			base.OnGUI(position, property, label);
			return;
		}
		if (property.propertyType == SerializedPropertyType.Integer)
		{
			var mask = property.intValue;
			var layerNames = srpAsset.renderingLayerMaskNames;
			if (layerNames == null)
				layerNames = defaultRenderingLayerNames;

			EditorGUI.BeginChangeCheck();

			EditorGUI.BeginProperty(position, label, property);
			mask = EditorGUI.MaskField(position, label, mask, layerNames);
			EditorGUI.EndProperty();

			if (EditorGUI.EndChangeCheck())
			{
				property.intValue = mask;
			}
		}
	}
}
