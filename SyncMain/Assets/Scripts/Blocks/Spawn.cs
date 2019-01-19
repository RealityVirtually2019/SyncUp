using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour {
    public Transform spawnLocation;
    public GameObject[] prefabs;
    public List<GameObject> spawnedObjects;
    private int spawnIndex = 0;
    public Transform endpoint;

    private void RandomSpawn()
    {
        // choose prefab to spawn
        int prefabIndex = Mathf.FloorToInt(Random.Range(0, 49));

        // offset for spawnLocation
        float[] xOffset = { -1.5f, 0.0f, 1.5f };
        float[] yOffset = { -1.5f, 0.0f, 1.5f };
        int xIndex = Random.Range(0, 2);
        int yIndex = Random.Range(0, 2);
        Vector3 offset = new Vector3(xOffset[xIndex], yOffset[yIndex], 0);
        Debug.Log(offset.ToString());

        GameObject spawn = Instantiate(prefabs[prefabIndex], spawnLocation.position + offset, spawnLocation.rotation);
        spawnedObjects.Add(spawn);
    }

    void Start () {
        InvokeRepeating("RandomSpawn", 0.0f, 2f);
    }

    // Update is called once per frame
    void Update () {

    }
}
