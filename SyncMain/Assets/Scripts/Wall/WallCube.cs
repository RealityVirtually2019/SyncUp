using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class WallCube : MonoBehaviour {

    private Transform PlayerHead;
    // Visual indication that cube is "on"
    public Color onColorHands;
    public Color onColorHead;
   // public Color offColor;

    public GameObject GoodPrefab;
    public GameObject BadPrefab;

    private Transform leftHandObject;
    private Transform rightHandObject;
    MeshRenderer cubeRenderer;
    public Renderer rend;
    public Material instanceMat;

    [SerializeField] GameObject ActiveBox;
    private bool SetDone = false;
    private bool Activation = false;

    private void Awake()
    {
        ActiveBox = GetComponentInChildren<ActiveScript>().gameObject;
        ActiveBox.SetActive(false);
    }

    private void Start()
    {
        SetDone = false;
    }


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
    void OnEnable() {

        Invoke("SetUp", 1f);

    }

    // Update is called once per frame
    void Update() {

        if (!SetDone)
            return;

        if(InCube(PlayerHead.position))
        {
            instanceMat.EnableKeyword("_EMISSION");
            instanceMat.SetColor("_EmissionColor", onColorHead);
            //gameObject.tag = "active";
            ActiveBox.SetActive(true);
            Activation = true;

        }
        else if (InCube(leftHandObject.position) || InCube(rightHandObject.position))
        {

            instanceMat.EnableKeyword("_EMISSION");
            instanceMat.SetColor("_EmissionColor", onColorHands);
            ActiveBox.SetActive(true);
            Activation = true;

            // gameObject.tag = "active";
        }
        else
        {
            instanceMat.DisableKeyword("_EMISSION");
            ActiveBox.SetActive(false);
            Activation = false;

            //gameObject.tag = "inactive";
        }
    }

    private void SetUp()
    {
        PlayerHead = GameObject.FindGameObjectWithTag("Player").transform;
        leftHandObject = GameObject.FindGameObjectWithTag("Lhand").transform;
        rightHandObject = GameObject.FindGameObjectWithTag("Rhand").transform;
        rend = GetComponent<Renderer>();
        instanceMat = rend.material;
        SetDone = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("BadBox"))
        {
            if (Activation == true)
            {
                MakeBoom(other.transform);

            }
            else if (Activation == false)
            {
               // GameObject boom = Instantiate(BadPrefab, other.transform.position, Quaternion.identity);
            }
            Destroy(other.gameObject);

        }
    }

    [PunRPC]
    public void MakeBoom(Transform other)
    {
        GameObject boomx = Instantiate(BadPrefab, other.position, Quaternion.identity);
    }

   
}
