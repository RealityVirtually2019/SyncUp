using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildBlock : MonoBehaviour {
    private TrashHealth th;
    private MeshRenderer mRend;

	// Use this for initialization
	void Awake () {
        th = GetComponentInParent<TrashHealth>();
        mRend = GetComponent<MeshRenderer>();
	}
	



    public void HitActive()
    {
        th.HitBlock(1);
        mRend.enabled = false;
    }

    public void HitInactive()
    {
        th.HitInActive();
    }
}
