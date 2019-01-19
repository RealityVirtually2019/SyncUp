using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallCube : MonoBehaviour {
    public OVRPlayerController mainPlayer;
    // Visual indication that cube is "on"
    public Color onColor;
    public Material material;

    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 playerPos = mainPlayer.transform.position;
        Vector3 cubePos = gameObject.transform.position;
        Vector3 cubeScale = gameObject.transform.lossyScale;

        Debug.Log("xPos: " + playerPos.x.ToString() + ", yPos: " + playerPos.y.ToString());
        // Check if player is "in bounds" of cube
        bool inCube = (cubePos.x - 0.5 * cubeScale.x <= playerPos.x) &&
            (playerPos.x <= cubePos.x + 0.5 * cubeScale.x) &&
            (cubePos.y - 0.5 * cubeScale.y <= playerPos.y) &&
            (playerPos.y <= cubePos.y + 0.5 * cubeScale.y);

        MeshRenderer cubeRenderer = gameObject.GetComponent<MeshRenderer>();

        if (inCube) // Change color
        {
            Material onMaterial = new Material(material);
            onMaterial.color = onColor;
            cubeRenderer.material = onMaterial;
        }
        else
        {
            Material offMaterial = new Material(material);
            offMaterial.color = material.color;
            cubeRenderer.material = offMaterial;
        }
    }
}
