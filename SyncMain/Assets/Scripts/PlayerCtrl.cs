using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour {

    static private PlayerCtrl _PC;
    static public PlayerCtrl PC
    {
        get
        {
            return _PC;
        }
        private set
        {
            if (_PC != null)
            {
                Debug.LogWarning("second attempt to set ScoreManager SM");
            }
            _PC = value;
        }
    }

    public GameObject Head;
    public GameObject Lhand;
    public GameObject Rhand;

    private void Awake()
    {
        PC = this;
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
