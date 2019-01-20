using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildBlock : MonoBehaviour {
    private TrashHealth th;

	// Use this for initialization
	void Awake () {
        th = GetComponentInParent<TrashHealth>();
	}
	
	// Update is called once per frame
	void Update () {

	}

    private void OnTriggerEnter(Collider other)
    {
        th.HitBlock(1);
    }
}
