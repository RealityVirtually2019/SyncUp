using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwuitch : MonoBehaviour {

    public OVRScreenFade fader;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("hand"))
        {
            StartCoroutine(StartLogin(2.5f));
        }
    }

    IEnumerator StartLogin(float time)
    {
        fader.FadeOut();
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene(2);
    }
}
