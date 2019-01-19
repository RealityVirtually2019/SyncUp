using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

[RequireComponent(typeof(PhotonView))]
public class MoveToEnd : MonoBehaviour {

    [SerializeField] Transform moveSpot;
    [SerializeField] float speed;

    private PhotonView PV;
    
	// Use this for initialization
	void Awake () {
        PV = GetComponent<PhotonView>();
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = Vector3.MoveTowards(transform.position, moveSpot.position, speed * Time.deltaTime);
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("hand"))
            PhotonNetwork.Destroy(gameObject);
    }
}
