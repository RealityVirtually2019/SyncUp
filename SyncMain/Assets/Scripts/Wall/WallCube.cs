using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallCube : MonoBehaviour {

    private Transform PlayerHead;
    // Visual indication that cube is "on"
    public Color onColor;
    public Color offColor;

    private Transform leftHandObject;
    private Transform rightHandObject;
    MeshRenderer cubeRenderer;
    public Renderer rend;
    public Material instanceMat;






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
    void Awake() {
        PlayerHead = GameObject.FindGameObjectWithTag("Player").transform;
        leftHandObject = GameObject.FindGameObjectWithTag("Lhand").transform;
        rightHandObject = GameObject.FindGameObjectWithTag("Rhand").transform;
        rend = GetComponent<Renderer>();
        instanceMat = rend.material;
        onColor.a = .9f;
        offColor.a = .2f;

    }

    // Update is called once per frame
    void Update() {

        if ((InCube(PlayerHead.position)|| InCube(leftHandObject.position) )|| InCube(rightHandObject.position))
        {

            instanceMat.EnableKeyword("_EMISSION");
            gameObject.tag = "active";
        }
        else
        {
            instanceMat.DisableKeyword("_EMISSION");
            gameObject.tag = "inactive";
        }
    }
}
