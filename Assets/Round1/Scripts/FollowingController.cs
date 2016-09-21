using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System.Collections;



public class FollowingController : MonoBehaviour
{

    [SerializeField]
    GameObject Ghost;
    [SerializeField]
    GameObject Player;
    [SerializeField]
    GameObject CreditsCanvas;

    public Door door;

    public SwimmingScene SceneScript;
    public SwimmingDetector SwimDetector;
    public MonsterScript[] monsterScript;

    Transform GhostTransform;
    Transform PlayerTransform;

    public UnityEvent FreezeCameraPosition;
    public UnityEvent UnFreezeCameraPosition;


    int currentStage = 0;

    float maxDistanceAllowed = 20f;
    float minDistanceAllowed = 3f;
    float playAudioDistance = 15f;

    bool waitingForStab = false;

    public GameObject[] Ruins;

    bool checkForCloseness = true;

    public float playAudioIn = 0;

    bool isGameOver = false;

    public Light directionLight;

    AudioSource GhostAudio;
    // Use this for initialization
    void Start()
    {
        GhostTransform = Ghost.transform;
        PlayerTransform = Player.transform;
        GhostAudio = Ghost.GetComponent<AudioSource>();

           
    }


    public void MoveGhostToPosition(int stage)
    {
        if (stage == 0)
        {
            currentStage = 0;
        }
        else
        {
            Ruins[1].transform.Find("Light").gameObject.SetActive(true);
            currentStage = stage + 1;
            //SceneScript.StartButtonPressed();
        }
        Ghost.GetComponent<GhostController>().GoToPosition(stage);
    }

    public void GhostReachedRuin()
    {
        if (currentStage > 21)
        {
            MoveGhostToNextPosition();
        }
        checkForCloseness = true;
    }

    public void MoveGhostToNextPosition()
    {
        checkForCloseness = false;
        currentStage += 1;
        Ghost.GetComponent<GhostController>().NextPosition();
    }

    // Update is called once per frame
    void Update()
    {
        if(isGameOver)
        {
            return;
        }
        if (playAudioIn >= 0)
        {
            playAudioIn -= Time.deltaTime;
        }

        if (currentStage > 0 && currentStage < 21 && Vector3.Distance(GhostTransform.position, PlayerTransform.position) > playAudioDistance)
        {
            if(playAudioIn < 0)
            {
                PlayGhostVoice(Sounds.WhereGoing);
                playAudioIn = 3;
            }
        }
        if (currentStage > 0 && currentStage < 21 && Vector3.Distance(GhostTransform.position, PlayerTransform.position) > maxDistanceAllowed)
        {
            FreezeCameraPosition.Invoke();
            SceneScript.ShowGameOverScreen();
            PlayerTransform.position = new Vector3(0, 0, 0);
            Destroy(Ghost);

           // PlayGhostVoice(Sounds.MonsterEating);

            AudioSource[] sources = PlayerTransform.Find("CenterEyeAnchor").GetComponents<AudioSource>();
            sources[1].clip = Resources.Load(Sounds.GameOver) as AudioClip;
            sources[1].Play();

            isGameOver = true;
        }
        if (waitingForStab)
        {
            return;
        }


        if (checkForCloseness && Vector3.Distance(GhostTransform.position, PlayerTransform.position) < minDistanceAllowed)
        {
            if (currentStage == 0)
            {
                PlayGhostVoice(Sounds.WelcomeGhost);
                MoveGhostToNextPosition();
            } else if (currentStage == 2)
            {
                PlayGhostVoice(Sounds.OpenDoor);
                MoveGhostToNextPosition();
            }           
            else if(currentStage == 5)
            {
                PlayGhostVoice(Sounds.Mural);
                SwitchOnLight(Ruins[0].gameObject);
                MoveGhostToNextPosition();
            }
            else if (currentStage == 8)
            {
                //PlayGhostVoice(Sounds.Light1);
                waitingForStab = true;
                SwimDetector.PickUpSword();
                PlayGhostVoice(Sounds.TakeSword);
            }
            else if (currentStage == 10)
            {
                MoveGhostToNextPosition();
            }
            else if (currentStage == 12)
            {
                PlayGhostVoice(Sounds.MonsterAppear1);
                StartCoroutine(MonsterAttack(0));

                monsterScript[1].gameObject.SetActive(true);
            }
            else if (currentStage == 13)
            {
                PlayGhostVoice(Sounds.Light2);
                SwitchOnLight(Ruins[1].gameObject);
                SwitchOnLight(Ruins[2].gameObject);
                MoveGhostToNextPosition();
            }
            else if (currentStage == 15)
            {
                PlayGhostVoice(Sounds.MonsterAppear2);
                StartCoroutine(MonsterAttack(1));

                monsterScript[2].gameObject.SetActive(true);
            }
            else if (currentStage == 16)
            {
                    waitingForStab = true;
                    door.StartChecking();
                PlayGhostVoice(Sounds.Light3);
                SwitchOnLight(Ruins[3].gameObject);
                //MoveGhostToNextPosition();
            }
            else if (currentStage == 19)
            {
                PlayGhostVoice(Sounds.MonsterAppear3);
                StartCoroutine(MonsterAttack(2));
            }
            else if (currentStage == 21)
            {
                AudioSource[] sources = Player.transform.Find("CenterEyeAnchor").GetComponents<AudioSource>();
                sources[1].clip = Resources.Load(Sounds.HappyEnding) as AudioClip;
                sources[1].Play();
                FreezeCameraPosition.Invoke();
                MoveGhostToNextPosition();
            }
            else
            {
                MoveGhostToNextPosition();
            }
        }
    }

    public void DoorOpened()
    {
        waitingForStab = false;
        PlayGhostVoice(Sounds.DoorOpening);
        MoveGhostToNextPosition();
    }
    public void SwitchOnLight(GameObject light)
    {
        light.SetActive(true);
        Light l = light.GetComponent<Light>();
        float origIntensity = l.intensity;
        Color origColor = l.color;

        l.intensity = 0f;
        l.color = new Color(0, 0, 0, 0);

        Go.to(l, 3f, new GoTweenConfig().floatProp("intensity", origIntensity));
        Go.to(l, 3f, new GoTweenConfig().colorProp("color", origColor));
    }

    public void GameStarted()
    {
        PlayGhostVoice(Sounds.StartGhost);
    }
    public void PlayGhostVoice(string path)
    {
        GhostAudio.clip = Resources.Load(path) as AudioClip;
        GhostAudio.Play();
    }

    public void PickedUpSword()
    {
        waitingForStab = false;
        PlayGhostVoice(Sounds.SwordPickup);
        MoveGhostToNextPosition();
    }

    IEnumerator MonsterAttack(int num)
    {
        Ghost.GetComponent<GhostController>().ShowFearAndHideTo(Player.transform.localPosition - Player.transform.Find("CenterEyeAnchor").forward);
        waitingForStab = true;
        FreezeCameraPosition.Invoke();
        yield return new WaitForSeconds(2f);
        StartCoroutine(monsterScript[num].StartAttack(Player.transform.localPosition + 1.2f*Player.transform.Find("CenterEyeAnchor").forward,num));
    }

    public void FinalAnimation()
    {
        Go.to(directionLight, 3f, new GoTweenConfig().colorProp("color", new Color(2f / 255f, 86f / 256f, 128f / 255f)));
        directionLight.intensity = 1f;

        SwitchOnLight(Ruins[4].gameObject);
        SwitchOnLight(Ruins[5].gameObject);
        Go.to(Player.transform, 10f, new GoTweenConfig().localPosition(new Vector3(24.23f,12.36f,61.04f)));
        StartCoroutine(UnfreezeCameraPositionLast());
    }

    IEnumerator UnfreezeCameraPositionLast()
    {
        yield return new WaitForSeconds(10f);
        CreditsCanvas.GetComponent<CameraFollower>().BringOnScreen();
        UnFreezeCameraPosition.Invoke();
    }
    public void MonsterStabbed()
    {
        if (currentStage == 12)
        {
            PlayGhostVoice(Sounds.MonsterDefeat1);
        }
        else if (currentStage == 16)
        {
            PlayGhostVoice(Sounds.MonsterDefeat2);
        }
        else if (currentStage == 20)
        {
            PlayGhostVoice(Sounds.MonsterDefeat3);
        }
        UnFreezeCameraPosition.Invoke();
        MoveGhostToNextPosition();
        waitingForStab = false;
    }
}
