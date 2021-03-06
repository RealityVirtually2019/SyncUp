﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour {

    static private ScoreManager _SM;
    static public ScoreManager SM
    {
        get
        {
            return _SM;
        }
        private set
        {
            if(_SM != null)
            {
                Debug.LogWarning("second attempt to set ScoreManager SM");
            }
            _SM = value;
        }
    }

    [SerializeField] Text textScore;
    [SerializeField] Text textLife;
    [SerializeField] int playerScore;
    [SerializeField] int playerLife;
    public AudioClip audiGood;
    public AudioClip audiBad;


    private AudioSource audiS;

    private void Awake()
    {
        SM = this;
        audiS = GetComponent<AudioSource>();
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void LoseLife(int lifeLoss)
    {
        playerLife -= lifeLoss;
        textLife.text = playerLife.ToString();
    }

    public void AddScore(int score)
    {
        playerScore += score;
        textScore.text = playerScore.ToString();
    }

    public void playGood()
    {
        audiS.PlayOneShot(audiGood);
    }

    public void playBad()
    {
        audiS.PlayOneShot(audiBad);
    }

}
