using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

[RequireComponent(typeof(PhotonView))]
public class SpawnManager1 : MonoBehaviour {
    public Transform[] spawnLocations;
    public GameObject[] prefabs;
    public List<GameObject> spawnedObjects;
    public float spawnDelay = 4f;
    private int spawnIndex = 0;
    private PhotonView PV;

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
    }

    private void RandomSpawn()
    {
        // choose prefab to spawn
        int prefabIndex = Random.Range(0, prefabs.Length);        
        GameObject spawn = Instantiate(prefabs[prefabIndex], RandSpot(spawnLocations), Quaternion.identity);
        spawnedObjects.Add(spawn);
    }

    void Start () {
        InvokeRepeating("RandomSpawn", 0.0f, spawnDelay);
    }

    
    private Vector3 RandSpot(Transform[] endP)
    {        
        int num = Random.Range(0, endP.Length);
        return (endP[num].position);
    }


    private Vector3 RandomSPawnOffset()
    {
        // offset for spawnLocation
        float[] xOffset = { -1f, 0.0f, 1f };
        float[] yOffset = { -.5f, 0.0f, .5f };
        int xIndex = Random.Range(0, 2);
        int yIndex = Random.Range(0, 2);
        Vector3 offset = new Vector3(xOffset[xIndex], yOffset[yIndex], 0);
        return offset;

    }


}
