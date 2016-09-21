using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SceneManagement;


public class FightSceneController : MonoBehaviour {

    public GameObject DebrisParent;
    List<DebrisScript> Debris = new List<DebrisScript>();

    bool isMonsterDestroyed = false;
    bool isDebrisOver = false;
	// Use this for initialization
	void Start () {
        for(var i = 0; i < DebrisParent.transform.childCount; i++)
        {
            Debris.Add(DebrisParent.transform.GetChild(i).GetComponent<DebrisScript>());
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            SceneManager.LoadScene("Swimming");
        }

        bool isDebriActive = false;
	    foreach(var debri in Debris)
        {
            if(debri.isAccessible)
            {
                isDebriActive = true;
                break;
            }
        }
        if(!isDebriActive && !isDebrisOver)
        {
            StartCoroutine(AllDebrisFinished());
        }
	}

    IEnumerator AllDebrisFinished()
    {
        isDebrisOver = true;
        print("Debris over! Waiting to see if monster has been destroyed too");
        yield return new WaitForSeconds(3f);
        if (!isMonsterDestroyed)
        {
            print("DEBRIS OVER! BETTER LUCK NEXT TIME");
            //SceneManager.LoadScene("Swimming");
        }
    }

    public void MonsterDestroyed()
    {
        isMonsterDestroyed = true;
        print("Great Job! Monster has been destroyed");
        SceneManager.LoadScene("Swimming");
    }
}
