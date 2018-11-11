using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRHandTracer : MonoBehaviour {
    public GameObject target;

	// Use this for initialization
	void Start () {
        this.transform.SetParent(target.transform, false);
		
	}
	
	// Update is called once per frame
	void Update () {
        
	}
}
 