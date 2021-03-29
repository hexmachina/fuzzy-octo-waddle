using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class RestraintRenderer : MonoBehaviour
{

	[SerializeField] Transform target;
	[SerializeField] Gradient gradient;
	[SerializeField] private float _restraintRadius = 5;
	[SerializeField] private float _widthMin = 0.1f;
	[SerializeField] private float _widthMax = 0.5f;
	private LineRenderer _lineRenderer;

	private void Awake()
	{
		TryGetComponent(out _lineRenderer);

	}

	// Update is called once per frame
	void Update()
	{
		if (!_lineRenderer || !target)
			return;

		var pos = transform.position;
		var tPos = target.position;
		_lineRenderer.SetPosition(0, pos);
		_lineRenderer.SetPosition(1, tPos);

		var dist = Vector3.Distance(pos, tPos);
		var ratio = Mathf.Clamp01(dist / _restraintRadius);
		var col = gradient.Evaluate(ratio);

		_lineRenderer.endColor = col;
		_lineRenderer.startColor = col;
		var width = Mathf.Lerp(_widthMax, _widthMin, ratio);
		_lineRenderer.startWidth = width;
		_lineRenderer.endWidth = width;
	}
}
