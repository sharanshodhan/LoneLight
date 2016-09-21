using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameOverScene : MonoBehaviour {

    AsyncOperation op;
    void Start()
    {
        op = SceneManager.LoadSceneAsync("Swimming");
        op.allowSceneActivation = false;
        
    }
	public void Retry()
    {
        op.allowSceneActivation = true;
    }
}
