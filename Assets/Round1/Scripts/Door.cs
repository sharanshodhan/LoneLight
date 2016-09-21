using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class Door : MonoBehaviour {

    [SerializeField]
    GameObject Player;

    bool doCheck = false;
    public UnityEvent DoorOpened;

    GameObject particles;
	// Use this for initialization
	void Start () {
        particles = transform.Find("Particles").gameObject;
        particles.SetActive(false);

    }
	
    public void StartChecking()
    {
        particles.SetActive(true);
        doCheck = true;
    }
	// Update is called once per frame
	void Update () {
        if(!doCheck)
        {
            return;
        }

	    if(Vector3.Distance(Player.transform.position, transform.position) < 2)
        {
            OpenDoor();
            doCheck = false;
        }
	}

    void OpenDoor()
    {
        particles.SetActive(false);

        Go.to(transform.Find("Right").transform, 2f, new GoTweenConfig().localPosition(new Vector3(1.5f,0,0),true));
        Go.to(transform.Find("Left").transform, 2f, new GoTweenConfig().localPosition(new Vector3(-1.5f, 0, 0), true));
        DoorOpened.Invoke();
    }


}
