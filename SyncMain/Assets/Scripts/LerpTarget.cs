using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpTarget : MonoBehaviour {

    public Transform lerpTarget;
    [SerializeField] float lerpSpeed = 5f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {

        transform.position = Vector3.Lerp(transform.position, lerpTarget.position, lerpSpeed * Time.deltaTime);


	}
}
