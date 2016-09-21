using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class MonsterScript : MonoBehaviour
{

    float boundsDistance = 0.3f;
    Vector3 startPosition;
    Vector3 origPosition;
    Vector3 ToPosition;
    float range = 0.05f;
    bool isMoving = false;
    bool isHurting = false;
    float speed = 0.05f;

    private float startTime;
    private float journeyLength;

    int life = 3;
    int hurt = 0;

    GoTween attackTween;
    Animator anim;

    Vector3 origRotation;
    public Vector3[] AttackToRotation;

    public UnityEvent MonsterDestroyed;
    public UnityEvent MonsterStabbed;

    AudioSource MonsterVoice;

    public int attackNum = 0;

    bool stabbed = true;
    // Use this for initialization
    void Start()
    {
        MonsterVoice = GetComponent<AudioSource>();

        anim = GetComponent<Animator>();
        //anim.SetTrigger("Swim");
        anim.SetBool("SwimmingBool", true);

        origPosition = transform.position;
        origRotation = transform.eulerAngles;
    }

    public IEnumerator StartAttack(Vector3 pos, int num, bool anim = false)
    {
        float dist = Vector3.Distance(transform.localPosition, origPosition);
        float time = dist / 1f;
        if (time != 0)
        {
            if (anim)
            {
                Go.to(transform, time, new GoTweenConfig().localPosition(origPosition));
                Go.to(transform, time, new GoTweenConfig().eulerAngles(origRotation - transform.eulerAngles, true));
            }
            else
            {
                transform.localPosition = origPosition;
                transform.eulerAngles = origRotation;
            }
        }

        

        if(num == 2)
        {
            Go.to(transform, 2f, new GoTweenConfig().localPosition(new Vector3(0,10,0),true));
            yield return new WaitForSeconds(2.5f);
        } else
        {
            yield return new WaitForSeconds(time);
        }
        if (this && this.gameObject)
        {
            StartCoroutine(Attack(pos));
        }
    }

    public void MoveMonsterTo(Vector3 pos)
    {
        StartCoroutine(RotateAndMove(pos));

    }

    IEnumerator RotateAndMove(Vector3 pos)
    {
        Go.to(transform, 10f, new GoTweenConfig().localPosition(pos));
        Go.to(transform, 3f, new GoTweenConfig().eulerAngles(new Vector3(0, 0, 180f), true));
        yield return new WaitForSeconds(7f);
        Go.to(transform, 3f, new GoTweenConfig().eulerAngles(new Vector3(0, 0, -180f), true));
    }

    IEnumerator Attack(Vector3 pos)
    {

        MonsterVoice.clip = Resources.Load(Sounds.MonsterGrowl) as AudioClip;
        MonsterVoice.Play();

        float dist = Vector3.Distance(transform.localPosition, pos);
        float time = dist / 6f;


        attackTween = Go.to(transform, 3.5f, new GoTweenConfig().localPosition(pos));

        attackTween.play();

        anim.SetBool("SwimmingBool", false);
        anim.SetBool("AttackBool", true);
        
        yield return new WaitForSeconds(0.1f);
        
        
    }

    public void AttackAnimationDone()
    {
        stabbed = false;
        anim.SetBool("AttackBool", false);
        anim.SetBool("SwimmingBool", true);
    }
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag != "Sword")
        {
            //return;
        }
        if(!stabbed)
        {
            stabbed = true;

            MonsterVoice.clip = Resources.Load(Sounds.MonsterHurt) as AudioClip;
            MonsterVoice.Play();

            StopAllCoroutines();

            anim.SetBool("SwimmingBool", false);
            anim.SetBool("RoarBool", true);
            attackTween.pause();
        }

    }

    public void RoarAnimationDone()
    {
        

        //anim.SetTrigger("Swim");
        anim.SetBool("RoarBool", false);
        anim.SetBool("SwimmingBool", true);

        if (attackNum < AttackToRotation.Length - 1)
        {
            StartCoroutine(SwimAway());
        }
        else
        {
            anim.SetBool("SwimmingBool", false);
            anim.SetBool("DeadBool", true);
            //Destroy(gameObject);
            MonsterDestroyed.Invoke();
            Go.to(transform, 2f, new GoTweenConfig().eulerAngles(new Vector3(-1, 32, -20), true));
            Go.to(transform, 2f, new GoTweenConfig().localPosition(new Vector3(transform.localPosition.x, 4.7f, transform.localPosition.z)));
            MonsterStabbed.Invoke();
        }
    }

    IEnumerator SwimAway()
    {
        anim.SetBool("TurnBool", true);
        anim.SetBool("SwimmingBool", false);

        yield return new WaitForSeconds(2f);
        var config = new GoTweenConfig().localPosition(origPosition);

        var chain = new GoTweenChain(new GoTweenCollectionConfig().setIterations(1));

        chain.setOnCompleteHandler(
            c => MoveBack());
        
        var tween = new GoTween(transform, 4f, config);
        chain.append(tween);

        chain.play();

        MonsterStabbed.Invoke();
    }

    void MoveBack()
    {
        Destroy(gameObject);
    }
}
