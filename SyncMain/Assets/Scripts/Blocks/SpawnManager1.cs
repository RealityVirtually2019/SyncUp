using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

[RequireComponent(typeof(PhotonView))]
public class SpawnManager1 : MonoBehaviour {
    public Transform[] spawnLocations;
    public GameObject[] prefabs;
    public List<GameObject> spawnedObjects;
    public float spawnDelay = 4f;
    public Text levelText;

    private int spawnIndex = 0;
    private PhotonView PV;
    private int spawnCount = 0;
    private int level = 1;
    private float blockMoveSpeed = 2f;

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
    }

    private void OnEnable()
    {
        spawnCount = 0;
    }

    private void RandomSpawn()
    {
        // choose prefab to spawn
        int prefabIndex = Random.Range(0, prefabs.Length);        
        GameObject spawn = Instantiate(prefabs[prefabIndex], RandSpot(spawnLocations), Quaternion.identity);
        spawnCount++;
        if ((spawnCount % 7) == 0)
        {
            blockMoveSpeed += .5f;
            level++;
            levelText.text = "Level: " + (level); 
        }
        spawn.GetComponent<MoveBlock>().speed = blockMoveSpeed;

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
