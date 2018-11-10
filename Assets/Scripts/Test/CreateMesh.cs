using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SKTEST{

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
public class CreateMesh : MonoBehaviour {
	private MeshFilter _meshFilter;
	private Mesh _mesh;
	private Vector3[] _vertices;

	public Vector3[] Vertices{
		get {return _vertices;}
	}
	// Use this for initialization
	void Awake () {
		_mesh = new Mesh();
		_meshFilter = GetComponent<MeshFilter>();
		
		
	}

	public void Create(){

		_meshFilter.mesh = _mesh;
		_vertices = new Vector3[]{
			new Vector3(0.0f, 5.0f, 0.0f),
			new Vector3(5.0f, 0.0f, 0.0f),
			new Vector3(-5.0f, 0.0f, 0.0f),
		};

		var triangles = new int[]{
			0,1,2
		};

		_mesh.vertices = _vertices;
		_mesh.triangles = triangles;


	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
}
