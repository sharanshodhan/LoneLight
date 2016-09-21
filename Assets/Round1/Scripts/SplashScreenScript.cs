using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class SplashScreenScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void StartButtonPressed()
    {
        SceneManager.LoadScene("Swimming");
    }
}
