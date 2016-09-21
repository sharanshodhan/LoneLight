using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

using System.Collections;

public class GhostController : MonoBehaviour {

    public Vector3[] RuinPoisitions;

	Vector3 StartPosition;
    Vector3 ToPosition;
    float speed = 2f;

	private float startTime;
	private float journeyLength;

    int currentPosition = 0;

	bool shouldMove = false;

    public UnityEvent GhostReachedDestination;
    public UnityEvent DoFinalAnimation;

    [SerializeField]
    GameObject Lights;

    List<GameObject> lights;

	// Use this for initialization
	void Start () {
        lights = new List<GameObject>();
        for(var i = 0; i < Lights.transform.childCount; i++)
        {
            lights.Add(Lights.transform.GetChild(i).gameObject);
        }
    }
	
    public void ShowFearAndHideTo(Vector3 pos)
    {
        StartCoroutine(ShakeAndHide(pos));
    }

    IEnumerator ShakeAndHide(Vector3 pos)
    {
        Go.to(transform.Find("Light"), 1f, new GoTweenConfig().shake(new Vector3(2, 2, 2), GoShakeType.Position));
        yield return new WaitForSeconds(1f);
        Go.to(transform, 1, new GoTweenConfig().localPosition(pos));
    }

    void CheckLightPosition()
    {
        foreach(var light in lights)
        {
            if (light.name == "On")
            {
                continue;
            }
            if(Vector3.Distance(light.transform.position, transform.position) < 1) {
                light.name = "On";
                light.SetActive(true);
                Light l = light.GetComponent<Light>();
                l.intensity = 0f;

                Go.to(l, 1f, new GoTweenConfig().floatProp("intensity", 2.26f));
            }
        }
    }

	// Update is called once per frame
	void Update () {
        

        if (shouldMove) {
            CheckLightPosition();

            float distCovered = (Time.time - startTime) * speed;
			float fracJourney = distCovered / journeyLength;
			transform.localPosition = Vector3.Lerp(StartPosition, ToPosition, fracJourney);
            if(fracJourney > 1f)
            {
                shouldMove = false;
                ReachedRuin();
            }
		}
	}

    public void ReachedRuin()
    {
        //NextPosition();
        GhostReachedDestination.Invoke();
    }

    public void GoToPosition(int currentPosition)
    {
        transform.localPosition = RuinPoisitions[currentPosition];
    }

    public void NextPosition()
    {
        if (currentPosition < RuinPoisitions.Length-1)
        {
            //StartPosition = RuinPoisitions[currentPosition];
            StartPosition = transform.localPosition;
            currentPosition++;
            ToPosition = RuinPoisitions[currentPosition];

            //Go.to(transform, 2f, new GoTweenConfig().localPosition(ToPosition));

            startTime = Time.time;
            journeyLength = Vector3.Distance(StartPosition, ToPosition);

            StartMovement();
        } else
        {
            GetComponent<AudioSource>().clip = Resources.Load(Sounds.FinalVoice) as AudioClip;
            GetComponent<AudioSource>().Play();
            DoFinalAnimation.Invoke();
        }
    }

	void StartMovement() {
		shouldMove = true;
	}
}
