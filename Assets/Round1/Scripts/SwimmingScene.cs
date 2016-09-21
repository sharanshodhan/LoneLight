using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.UI;


public class SwimmingScene : MonoBehaviour {

    public FollowingController followingcontroller;
    public SwimmingDetector swimmingdetector;
    GameObject myself;

    bool isGameStarted = false;
    public UnityEvent StartGame;
    public GameObject SplashCanvas;
    public GameObject Tutorial1Canvas;

    public GameObject GameOverCanvas;

    AsyncOperation op;

    // Use this for initialization
    void Awake () {
        //op = SceneManager.LoadSceneAsync("Swimming");
        //op.allowSceneActivation = false;

        followingcontroller.MoveGhostToPosition(Config.LastState);
        print(Config.LastCameraPosition);
        swimmingdetector.GotoPos(Config.LastCameraPosition);
    }
	
    public void RetryGamePressed()
    {
        print("RETRY");
        if (GameOverCanvas.activeSelf)
        {
            SceneManager.LoadSceneAsync("Swimming");
        }
    }

    public void ShowGameOverScreen()
    {
        GameOverCanvas.SetActive(true);
    }

    public void LoadScene(string name)
    {
        SceneManager.LoadScene("Fight");
    }

    public void StartButtonPressed()
    {
        if (GameOverCanvas.activeSelf)
        {
            RetryGamePressed();
        }
        if (isGameStarted)
        {
            return;
        }
        else
        {
            Destroy(SplashCanvas);
            Tutorial1Canvas.SetActive(true);
            isGameStarted = true;
            StartGame.Invoke();

            StartCoroutine(AnimateTutorialHand());
            StartCoroutine(HideTutorialCanvas());
        }
    }

    IEnumerator AnimateTutorialHand()
    {
        yield return new WaitForSeconds(1f);
        Go.to(Tutorial1Canvas.transform.Find("Hand"), 1f, new GoTweenConfig().eulerAngles(new Vector3(0, 0, 25), true));
        yield return new WaitForSeconds(1f);
        Go.to(Tutorial1Canvas.transform.Find("Hand"), 2f, new GoTweenConfig().eulerAngles(new Vector3(0, 0, -50), true));
        yield return new WaitForSeconds(2f);
        Go.to(Tutorial1Canvas.transform.Find("Hand"), 1f, new GoTweenConfig().eulerAngles(new Vector3(0, 0, 25), true));
        yield return new WaitForSeconds(1f);
        Go.to(Tutorial1Canvas.transform.Find("Hand"), 1f, new GoTweenConfig().eulerAngles(new Vector3(0, 0, 25), true));
        yield return new WaitForSeconds(1f);
        Go.to(Tutorial1Canvas.transform.Find("Hand"), 2f, new GoTweenConfig().eulerAngles(new Vector3(0, 0, -50), true));
        yield return new WaitForSeconds(2f);
        Go.to(Tutorial1Canvas.transform.Find("Hand"), 1f, new GoTweenConfig().eulerAngles(new Vector3(0, 0, 25), true));

    }
    IEnumerator HideTutorialCanvas()
    {
        yield return new WaitForSeconds(10f);
        Go.to(Tutorial1Canvas.transform.Find("Image").GetComponent<Image>(), 1f, new GoTweenConfig().colorProp("color",new Color(0, 0, 0, 0)));
        Go.to(Tutorial1Canvas.transform.Find("Image").Find("Text").GetComponent<Text>(), 1f, new GoTweenConfig().colorProp("color", new Color(0, 0, 0, 0)));
        Go.to(Tutorial1Canvas.transform.Find("Hand").GetComponent<Image>(), 1f, new GoTweenConfig().colorProp("color", new Color(0, 0, 0, 0)));
        Go.to(Tutorial1Canvas.transform.Find("Fist").GetComponent<Image>(), 1f, new GoTweenConfig().colorProp("color", new Color(0, 0, 0, 0)));
        yield return new WaitForSeconds(2f);
        Destroy(Tutorial1Canvas);
    }
}
