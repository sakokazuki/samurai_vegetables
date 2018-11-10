using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SKTEST{
public class CrossPointCheck : MonoBehaviour {
	public CreateMesh _createMesh;
	public CreatePlane _createPlane;

	public Vector3 _pos1;
	public Vector3 _pos2;

	public GameObject _prefabSphere;
	public List<GameObject> _posObjs;

	// Use this for initialization
	void Start () {
		_createMesh.Create();
        _createPlane.Create();
        Cut();
		
	}

	public void Cut(){
		var plane = _createPlane.Plane;
		var group1PosList = new List<Vector3>();
		var group2PosList = new List<Vector3>();

		CheckPlaneSide(plane, group1PosList, group2PosList);	

		CalcCrossPoint(plane, group1PosList, group2PosList);
			

		DebugShowPos();

	}

	private void CheckPlaneSide(Plane plane, List<Vector3> group1, List<Vector3> group2){
		var vertices = _createMesh.Vertices;
		foreach(var v in vertices){
			if(plane.GetSide(v)){
				group1.Add(v);
			}else{
				group2.Add(v);
			}
		}
	}

	private void CalcCrossPoint(Plane plane, List<Vector3> group1, List<Vector3> group2){
		float distance = 0;
		Vector3 basePos;
		Vector3 tmpPos1;
		Vector3 tmpPos2;

		if(group2.Count < group1.Count){
			basePos = group2[0];
			tmpPos1 = group1[0];
			tmpPos2 = group1[1];
		}else{
			basePos = group1[0];
			tmpPos1 = group2[0];
			tmpPos2 = group2[1];
		}

		Ray ray1 = new Ray(basePos, (tmpPos1 - basePos).normalized);
		plane.Raycast(ray1, out distance);

		_pos1 = ray1.GetPoint(distance);
		
		Ray ray2 = new Ray(basePos, (tmpPos2 - basePos).normalized);
		plane.Raycast(ray2, out distance);

		_pos2 = ray2.GetPoint(distance);


	}

	private void DebugShowPos(){
		if(_posObjs != null){
			foreach(var o in _posObjs){
				Destroy(o.gameObject);
			}
		}

		_posObjs = new List<GameObject>();

		var vertices = _createMesh.Vertices;

		foreach(var v in vertices){
			var go = Instantiate(_prefabSphere);
			go.transform.position = v;
			this.transform.SetParent(go.transform);
			_posObjs.Add(go);
		}

		var go1 = Instantiate(_prefabSphere);
		go1.transform.position = _pos1;
		this.transform.SetParent(go1.transform);
		_posObjs.Add(go1);

		var go2 = Instantiate(_prefabSphere);
        go2.transform.position = _pos2;
        this.transform.SetParent(go2.transform);
        _posObjs.Add(go2);

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
}