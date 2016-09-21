using UnityEngine;
using System.Collections;

public class DebrisScript : MonoBehaviour {

    public GameObject hand;
    public bool isAccessible = true;
	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
        if(Vector3.Distance(transform.position, hand.transform.position) > 1.4f)
        {
            isAccessible = false;
        }
	}
}
