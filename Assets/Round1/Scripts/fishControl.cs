using UnityEngine;
using System.Collections;

public class fishControl : MonoBehaviour {

	public float speed = 0.005f;
	public float rotationSpeed = 3.0f;

	float neighbourDistance = 2.0f; 
	// Use this for initialization
	void Start () {
		speed = Random.Range(-0.5f, 0.5f);
        if(speed > 0)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
	}
	
	// Update is called once per frame
	void Update () {

		//if (Random.Range(0,5) < 1) {
		//	swimInFlock();
		//}
        transform.localPosition = new Vector3(transform.localPosition.x,transform.localPosition.y,transform.localPosition.z + (Time.deltaTime*speed));

        if (transform.localPosition.z < 0)
        {
            speed = -speed;
            if (speed < 0)
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
            }
            else if (speed > 0)
            {
                transform.eulerAngles = new Vector3(0, 180, 0);
            }
        } else if (transform.localPosition.z > 120)
        {
            speed = -speed;
            if (speed < 0)
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
            } else if (speed > 0)
            {
                transform.eulerAngles = new Vector3(0, 180, 0);
            }
        }
    }

	void OnTriggerEnter(Collider other)
    {
        speed = -speed;
        if (speed < 0)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        else if (speed > 0)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
    }
}
