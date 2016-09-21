using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SplashScreen : MonoBehaviour {

    public GameObject StartButton;
	// Use this for initialization
	void Start () {

        StartButton.SetActive(false);

        StartCoroutine(ShowStartButton());
    }
	
    IEnumerator ShowStartButton()
    {
        yield return new WaitForSeconds(18f);
        StartButton.transform.Find("Text").GetComponent<Text>().color = new Color(1, 1, 1, 0);
        Go.to(StartButton.transform.Find("Text").GetComponent<Text>(), 2f, new GoTweenConfig().colorProp("color", new Color(1, 1, 1, 1)));
        StartButton.SetActive(true);
    }

	// Update is called once per frame
	void Update () {
	
	}
}
