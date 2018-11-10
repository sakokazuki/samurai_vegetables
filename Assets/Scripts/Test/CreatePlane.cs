using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SKTEST{
[RequireComponent(typeof(LineRenderer))]
public class CreatePlane : MonoBehaviour {
	private LineRenderer _lineRenderer;
	private Plane _plane;

	public Plane Plane{
		get { return _plane;}
	}

	private Vector3 normal;
	private Vector3 position;

	// Use this for initialization
	void Start () {
	}

	public void Create(){
		_lineRenderer = GetComponent<LineRenderer>();
		_plane = new Plane();

		position = (_lineRenderer.GetPosition(0) + _lineRenderer.GetPosition(1))/2;
		var p1 = _lineRenderer.GetPosition(0) - position;
		normal = (Quaternion.Euler(0, 0, 90f) * p1).normalized;
		_plane.SetNormalAndPosition(normal, position);

	}

	void OnDrawGizmosSelected()
    {
        float length = 10.0f;
        Gizmos.color = Color.blue;

        Gizmos.DrawLine(position, position + (normal * length));

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
}
