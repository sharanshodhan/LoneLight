using UnityEngine;
using System.Collections;

public class PlayerColliderController : MonoBehaviour {

    BoxCollider coll;
    Transform Player;
	// Use this for initialization
	void Start () {
        coll = GetComponent<BoxCollider>();
        Player = transform.Find("CenterEyeAnchor").transform;
	}
	
	// Update is called once per frame
	void Update () {
        coll.center = Player.forward * 0.3f;
	}
}
