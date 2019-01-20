using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class BaseBlocks : MonoBehaviour {
    readonly List<GameObject> activeCubes = new List<GameObject>();
    readonly string activeTag = "active";
    OculusHapticsController leftControllerHaptics;
    OculusHapticsController rightControllerHaptics;

    private bool CheckAdjacent(Transform object1, Transform object2, double precision = 1e-4)
    {
        // check vertical or horizontal adjacency on grid
        return (Math.Abs(object1.transform.position.x - object2.transform.position.x) < precision ||
            Math.Abs(object1.transform.position.y - object2.transform.position.y) < precision);
    }

    private bool CheckBlock(Transform[] cubes)
    {
        // only check if there are 3 active cubes available to make a base block
        if (cubes.Length == 3)
        {
            return ((CheckAdjacent(cubes[0], cubes[1]) && CheckAdjacent(cubes[1], cubes[2])) ||
                (CheckAdjacent(cubes[0], cubes[1]) && CheckAdjacent(cubes[0], cubes[2]))) ||
                (CheckAdjacent(cubes[1], cubes[2]) && CheckAdjacent(cubes[0], cubes[2]));
        }
        return false;
    }

    // Use this for initialization
    void Start () {

    }

    private void OnEnable()
    {
        leftControllerHaptics = GameObject.FindGameObjectWithTag("Lhand").GetComponent<OculusHapticsController>();
        rightControllerHaptics = GameObject.FindGameObjectWithTag("Rhand").GetComponent<OculusHapticsController>();
    }

    // Update is called once per frame
    void Update () {
        Transform[] arrayOfChildren = gameObject.transform.Cast<Transform>().Where(c => c.gameObject.tag == "active").ToArray();
        bool foundBlock = CheckBlock(arrayOfChildren);
        if (foundBlock)
        {
            print("found block");
            leftControllerHaptics.Vibrate(VibrationForce.Medium);
            rightControllerHaptics.Vibrate(VibrationForce.Medium);
        }
    }
}
