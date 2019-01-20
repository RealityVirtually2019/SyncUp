using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashHealth : MonoBehaviour {
    private int totalBlocks;

    void Awake () {
        ChildBlock[] children = GetComponentsInChildren<ChildBlock>();
        totalBlocks = children.Length;

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void HitBlock(int numBlocks)
    {
        totalBlocks -= numBlocks;
        if (totalBlocks == 0)
            UpdateScore();
    }

    public void HitInActive()
    {
        UpdateScore();
    }

    private bool CheckPassed()
    {
        return totalBlocks == 0;
    }

    private void UpdateScore()
    {
        if (CheckPassed())
        {
            ScoreManager.SM.AddScore(1);
            ScoreManager.SM.playGood();
        }
        else
        {
            ScoreManager.SM.LoseLife(1);
            ScoreManager.SM.playBad();
            Destroy(gameObject);
        }

    }
}
