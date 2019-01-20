using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

[RequireComponent(typeof(PhotonView))]
public class SpawnManager : MonoBehaviour {


    public Transform[] spawnLocations;
    public GameObject[] prefabs;
    public List<GameObject> spawnedObjects;
    public float spawnDelay = 4f;
    [SerializeField] float fastSpeed = 10f;
    [SerializeField] float normSpeed = 5f;
    private int spawnIndex = 0;
    private PhotonView PV;
    private Transform playerPos;
    private Vector3 startDis = new Vector3(10f, 10f, 10f);

    private GameObject TheSpeedyOne;

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
        playerPos = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void RandomSpawn()
    {
        // choose prefab to spawn
        int prefabIndex = Random.Range(0, prefabs.Length);
        //For Regular game
        //GameObject spawn = Instantiate(prefabs[prefabIndex], RandSpot(spawnLocations), Quaternion.identity);
        //For PhotonNetwork
        GameObject spawn = PhotonNetwork.Instantiate(Path.Combine("4piece", prefabs[prefabIndex].name), RandSpot(spawnLocations), Quaternion.identity);
        spawnedObjects.Add(spawn);
    }

    void Start () {
        InvokeRepeating("RandomSpawn", 0.0f, spawnDelay);
    }

    private void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger))
        {
            print("Right Hand Index");
            SpeedUp();

        }

        if(OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger))
        {
            print("Left hand index");
        }

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

    public void SpeedUp()
    {
        TheSpeedyOne = FindClosest();
        if (TheSpeedyOne)
        {
            print("Closest one is " + TheSpeedyOne.name);
            TheSpeedyOne.GetComponent<MoveBlock>().speed = fastSpeed;
        }
    }


    public GameObject FindClosest()
    {
        GameObject thisTheOne = null;
        float dis = 100f;
        float compare;

        foreach(GameObject x in spawnedObjects)
        {
            compare = Vector3.Distance(x.transform.position, playerPos.position);
            if (compare < dis)
            {
                //found closest to player
                dis = compare;
                thisTheOne = x;
            }
        }
        return thisTheOne;
    }


}
