using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour {
    public Transform spawnLocation;
    public GameObject prefabSpawn;
	// Use this for initialization
	void Start () {
        GameObject spawnedObject = Instantiate(prefabSpawn, spawnLocation.position, spawnLocation.rotation);

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
