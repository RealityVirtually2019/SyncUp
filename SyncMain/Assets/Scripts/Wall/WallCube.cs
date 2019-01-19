using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallCube : MonoBehaviour {
    public OVRPlayerController mainPlayer;
    // Visual indication that cube is "on"
    public Color onColor;
    public Material material;

    private bool InCube(Vector3 avatarPos)
    {
        Vector3 cubePos = gameObject.transform.position;
        Vector3 cubeScale = gameObject.transform.lossyScale;
        return (cubePos.x - 0.5 * cubeScale.x <= avatarPos.x) &&
            (avatarPos.x <= cubePos.x + 0.5 * cubeScale.x) &&
            (cubePos.y - 0.5 * cubeScale.y <= avatarPos.y) &&
            (avatarPos.y <= cubePos.y + 0.5 * cubeScale.y);
    }

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        Vector3 headPos = mainPlayer.transform.position;
        Vector3 leftHandPos = mainPlayer.transform.Find("TrackingSpace").transform.TransformPoint(OVRInput.GetLocalControllerPosition(OVRInput.Controller.LTrackedRemote));
        Vector3 rightHandPos = mainPlayer.transform.Find("TrackingSpace").transform.TransformPoint(OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTrackedRemote));
        
        MeshRenderer cubeRenderer = gameObject.GetComponent<MeshRenderer>();

        // Change color
        if ((InCube(headPos) || InCube(leftHandPos)) || InCube(rightHandPos))
        {
            Material onMaterial = new Material(material){ color = onColor };
            cubeRenderer.material = onMaterial;
        }
        else
        {
            Material offMaterial = new Material(material) { color = material.color };
            cubeRenderer.material = offMaterial;
        }
    }
}
