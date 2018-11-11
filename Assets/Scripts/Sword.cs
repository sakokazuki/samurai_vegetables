using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour {
    [SerializeField]
    private int _victimLayer;
    private Vector3 _velocity;
    private Vector3 _pPos;
    public Material _camMaterial;
    public float _nextCutInterval;
    private float _nextCutTime;
    public Transform _tip;

    // Use this for initialization
    void Start () {
		
	}

    void Update()
    {
        if(_nextCutTime > 0f)
        {
            _nextCutTime -= Time.deltaTime;
        }

        Vector3 currentPos = _tip.transform.position;
        var p1 = currentPos;
        var p2 = _pPos;
        var p3 = currentPos + new Vector3(0, 0, 1f);


        Vector3 normal = Vector3.Cross(p1 - p2, p1 - p3).normalized;
        Vector3 diff = p1 - p2;
        Debug.Log("------");
        Debug.LogFormat("c {0}, {1}, {2}", p1.x, p1.y, p1.z);
        Debug.LogFormat("p {0}, {1}, {2}", p2.x, p2.y, p2.z);
        Debug.LogFormat("d {0}, {1}, {2}", diff.x, diff.y, diff.z);

    }
	
	void LateUpdate () {
        Debug.Log("late update");
        _pPos = _tip.transform.position;
		
	}

    void OnTriggerEnter(Collider other)
    {
        var go = other.gameObject;
        if(go.layer == _victimLayer && _nextCutTime <= 0f)
        {
            _nextCutTime = _nextCutInterval;

            Cut(go);
        }
    }

    private void Cut(GameObject victim)
    {
        
        Vector3 currentPos = transform.position;
        var p1 = currentPos;
        var p2 = _pPos;
        var p3 = currentPos + new Vector3(0, 0, 1f);
        
        
        Vector3 normal = Vector3.Cross(p1 - p2, p1 - p3).normalized;
        Vector3 center = victim.transform.position;
        Debug.Log(normal);
        Debug.Log(center);

        var pieces = new List<GameObject>();
        var cut = BLINDED_AM_ME.MeshCut.Cut(victim, center, normal, _camMaterial);
        pieces.AddRange(cut);

        foreach (var piece in pieces)
        {
            var mc = piece.GetComponent<MeshCollider>();
            if (mc == null)
                mc = (MeshCollider)piece.AddComponent(typeof(MeshCollider));
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
}
