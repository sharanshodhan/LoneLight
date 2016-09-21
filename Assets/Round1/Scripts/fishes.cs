using UnityEngine;
using System.Collections;

public class fishes : MonoBehaviour {

	public GameObject[] fishPrefabs;
    public GameObject player;
	public static float rangeX = 56f;
    public static float rangeY= 5f;
    public static float rangeZ = 100.0f;
    static int num_Fishes = 250;
	public static GameObject[] all_Fishes = new GameObject[num_Fishes];
	public static Vector3 targetPos = Vector3.zero;
	// Use this for initialization
	void Start () {
		
		//player = GameObject.FindWithTag("MainCamera");
		Vector3 centerPos = player.transform.localPosition;
		print(centerPos);
		for (int i = 0; i < num_Fishes; i++) {
			Vector3 pos = new Vector3(Random.Range(centerPos.x, centerPos.x+rangeX),
									 Random.Range(centerPos.y, centerPos.y+rangeY),
									 Random.Range(centerPos.z+30f, centerPos.z+rangeZ));

			all_Fishes[i] = (GameObject) Instantiate(fishPrefabs[Random.Range(0,fishPrefabs.Length)], pos, Quaternion.identity);
            all_Fishes[i].transform.SetParent(transform);
            all_Fishes[i].transform.localScale = new Vector3(3f, 3f, 3f);
		} 
	}
	
	//// Update is called once per frame
	//void Update () {
	//	cleanOutOfRangeFishes();
	//}

	////check if any fish is out of range. Clean and replace with a new one.
	//void cleanOutOfRangeFishes () {
	//	Vector3 centerPos = player.transform.localPosition;
	//	for (int i = 0; i < num_Fishes; i++) {
	//		if (Vector3.Distance(all_Fishes[i].transform.position, centerPos) > 10.0f) {
	//			//print("deleting fish");
	//			Destroy(all_Fishes[i]);
	//			Vector3 pos = new Vector3(Random.Range(centerPos.x-range, centerPos.x+range),
	//								 Random.Range(centerPos.y-range, centerPos.y+range),
	//								 Random.Range(centerPos.z-range, centerPos.z+2*range));

	//			all_Fishes[i] = (GameObject) Instantiate(fishPrefab, pos, Quaternion.identity);
	//			//print("fish replaced");
	//		}
	//	}
	//}
}
