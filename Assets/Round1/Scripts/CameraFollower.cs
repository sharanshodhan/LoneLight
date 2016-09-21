using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CameraFollower : MonoBehaviour {

    Vector3 origPosition;
    public Transform playerTransform;

    bool isActive = false;
    // Use this for initialization
    void Start () {
        origPosition = transform.position;
    }
	
    public void BringOnScreen()
    {
        gameObject.SetActive(true);
        Go.to(transform.Find("Image").GetComponent<Image>(), 1f, new GoTweenConfig().colorProp("color", new Color(1, 1, 1, 0.5f)));
        Go.to(transform.Find("Image").Find("Title").GetComponent<Text>(), 1f, new GoTweenConfig().colorProp("color", new Color(1, 1, 1, 1)));
        Go.to(transform.Find("Image").Find("Left").GetComponent<Text>(), 1f, new GoTweenConfig().colorProp("color", new Color(1, 1, 1, 1)));
        Go.to(transform.Find("Image").Find("Right").GetComponent<Text>(), 1f, new GoTweenConfig().colorProp("color", new Color(1, 1, 1, 1)));
        Go.to(transform.Find("Image").Find("Center").GetComponent<Text>(), 1f, new GoTweenConfig().colorProp("color", new Color(1, 1, 1, 1)));
    }
}
