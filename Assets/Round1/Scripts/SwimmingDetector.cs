using UnityEngine;
using System.Collections;
using Leap.Unity.Attributes;
using Leap.Unity;
using Leap;
using UnityEngine.Events;


public class SwimmingDetector : MonoBehaviour
{
    Hand hand;

    public IHandModel LeftHandModel = null;
	public IHandModel RightHandModel = null;

    [SerializeField]
    AudioSource Ambient;

	public GameObject camera;
    public GameObject HandSword;
    public GameObject FreeSword;

    //add keyboard support
    private Vector3 moveDirection = Vector3.zero;


    float speedDivider = 70f;

    bool isSwimming = false;

    bool shouldGuestMove = true;
    bool freezeMovement = true;
    bool isRightTime = false;

    public UnityEvent SwordPickedUp;

    void Start()
    {
        Ambient.clip = Resources.Load(Sounds.AmbientBreathing) as AudioClip;
        Ambient.Play();

    }
    public void StartGame()
    {
        freezeMovement = false;
    }

    public void PickUpSword()
    {
        isRightTime = true;
    }
    public void SwordProximityActive()
    {

        if(FreeSword == null || !isRightTime)
        {
            return;
        }
        HandSword.SetActive(true);
        Destroy(FreeSword);
        SwordPickedUp.Invoke();
        StartCoroutine(RemoveParticleOnSword());
    }
    IEnumerator RemoveParticleOnSword()
    {
        yield return new WaitForSeconds(3f);
        HandSword.transform.Find("Particles").gameObject.SetActive(false);
    }
    public void FreezeMovement()
    {
        freezeMovement = true;
    }

    public void UnFreezeMovement()
    {
        freezeMovement = false;
    }

    public void GotoPos(Vector3 lastPos)
    {
        camera.transform.localPosition = lastPos;
    }

    public void HandsFolded()
    {
        shouldGuestMove = false;
    }

    public void HandsUnfolded()
    {
        shouldGuestMove = true;
    }

    void Update()
    {
        /*
        camera.transform.Rotate(new Vector3(-Input.GetAxis("Mouse Y"), 0, 0) * Time.deltaTime * 100.0f);
        moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        moveDirection = transform.TransformDirection(moveDirection);
        moveDirection *= 6.0f;
        camera.transform.Translate(moveDirection * Time.deltaTime);

        if (Input.GetKeyDown("c"))
        {
            SwordProximityActive();
        }
        */

        if (freezeMovement || !shouldGuestMove)
        {
            return;
        }

        if (RightHandModel.IsTracked)
        {
            hand = RightHandModel.GetLeapHand();
        }        
        else
        {
            hand = null;
        }

		if (hand != null) {
            Vector3 handVector = (hand.Fingers [1].Bone (Bone.BoneType.TYPE_DISTAL).Direction.ToVector3 ());
			camera.transform.Translate (new Vector3 (handVector.x / speedDivider, handVector.y / speedDivider, handVector.z / speedDivider));
        }
    }
}
