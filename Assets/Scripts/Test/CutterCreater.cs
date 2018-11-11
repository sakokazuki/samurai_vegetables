using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CutterCreater : MonoBehaviour {
	private LineRenderer _line;
	public Gradient _lineColor;
	bool _firstClick;

	public Material _capMaterial;
    public int _victimLayer;

	// Use this for initialization
	void Start () {
		_firstClick = true;
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButtonDown(0)){
			

			Vector3 clickPosition = GetClickPosition();


			if(_line && _firstClick){
				Destroy(_line.gameObject);	
			}			

			if(_firstClick){
				_line = CreateLine();
				_line.gameObject.SetActive(false);

				_line.SetPosition(0, clickPosition);
			}else{
				_line.SetPosition(1, clickPosition);
				Cut(_line);
				_line.gameObject.SetActive(true);
				
			}

			_firstClick = !_firstClick;			
		}		
	}

	void Cut(LineRenderer line){
		List<GameObject> victims = new List<GameObject>();
		var lineStartPos = line.GetPosition(0);
		var lineEndPos = line.GetPosition(1);
		int rayNum = 50;
		float raydist = 100.0f;
		for(int i=0; i<rayNum+1; i++){
			var ro = Vector3.Lerp(lineStartPos, lineEndPos, i/(float)rayNum);
			ro.z = raydist/-2.0f;
			Ray ray = new Ray(ro, transform.forward);
			Debug.DrawRay (ray.origin, ray.direction * raydist, Color.red, 1.0f, false);
			RaycastHit hit = new RaycastHit ();
			if (Physics.Raycast (ray, out hit, raydist)) {
				GameObject hitObject = hit.collider.gameObject;
                if (hitObject.layer != _victimLayer) continue;

				
				if(!ListContainsGo(victims, hitObject)){
					victims.Add(hitObject);	
				}
			}			
		}
		
		var p1 = line.GetPosition(0);
		var p2 = line.GetPosition(1);
		var p3 = line.GetPosition(1) + new Vector3(0, 0, 1f);
		var position = Vector3.Lerp(p1, p2, 0.5f);
		Vector3 normal = Vector3.Cross(p1 - p2, p1 - p3).normalized;

		var pieces = new List<GameObject>();
		foreach(var victim in victims){
			var cut = BLINDED_AM_ME.MeshCut.Cut(victim, position, normal, _capMaterial);
			pieces.AddRange(cut);
		}

		foreach(var piece in pieces){
			var mc = piece.GetComponent<MeshCollider>();
            if(mc==null)
                mc = (MeshCollider) piece.AddComponent(typeof(MeshCollider));
            else
            {
                mc.sharedMesh = null;
                mc.sharedMesh = piece.GetComponent<MeshFilter>().mesh;
            }
            mc.convex = true;

            var rb = piece.GetComponent<Rigidbody>();
            if (rb == null)
            {
                rb = (Rigidbody)piece.AddComponent(typeof(Rigidbody));
            }


        }
	}

	bool ListContainsGo(List<GameObject> list, GameObject go){
		foreach(var l in list){
			int id = l.GetInstanceID();
			if(id == go.GetInstanceID()){
				return true;
			}
		}
		return false;

	}




	Vector3 GetClickPosition(){
		Vector3 clickPosition = Input.mousePosition;
		clickPosition.z = 10f;
		Vector3 v = Camera.main.ScreenToWorldPoint(clickPosition);
		return v;
	}

	LineRenderer CreateLine(){
		var go = new GameObject("cutter_line");
		var line = go.AddComponent<LineRenderer>();
		go.transform.SetParent(this.transform, false);
		line.positionCount = 2;
		line.colorGradient = _lineColor;
		line.startWidth = 0.1f;
		line.endWidth = 0.1f;
		
		return line;
	}


}
