using UnityEngine;
using System.Collections;

public class ObjectController : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        float randX = -1 + Random.Range(0f, 2f);
        transform.localPosition = new Vector3(randX, transform.localPosition.y, transform.localPosition.z);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
