using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CatchCatch : MonoBehaviour
{
    public List<BoxCollider> list_Trigger = new List<BoxCollider>();   

    public GameObject btExit;
    public GameObject claw3;
    public GameObject clawRailRow;
    public GameObject clawRailColumn;
    public GameObject follow;
    public GameObject dollLocation;

    public TextMeshProUGUI tRemaingTime;

    public Animator animator;

    public FixedJoystick joyStick;  

    public int countCatchCatch = 12;

    public bool isCatch = false;
    public bool isPlayState = false;

    public float remainingTime = 20.0f;
    public float respawnTime = 0f;

    private bool btClick = false;

    private float clawSpeed = 100.0f;
    private float time1 = 0f;
    private float time2 = 0f;

    private static CatchCatch instance;
    public static CatchCatch Instance
    {
        get
        {
            if (instance == null)
            {
                var obj = FindObjectOfType<CatchCatch>();

                if (obj != null)
                    instance = obj;
                else
                {
                    var newObj = new GameObject().AddComponent<CatchCatch>();

                    instance = newObj;
                }
            }
            return instance;
        }
    }

    private void Awake()
    {
        var objs = FindObjectsOfType<CatchCatch>();

        if (objs.Length != 1)
        {
            Destroy(gameObject);
            return;
        }
    }

    private void OnEnable()
    {
        tRemaingTime.text = remainingTime.ToString("F0");
    }
  
    void Update()
    {
        if (respawnTime <= 4.0f)
            respawnTime += Time.deltaTime;

        if (respawnTime <= 3.0f)
            btExit.GetComponent<Button>().interactable = false;
        else if (respawnTime >= 3.0f && isPlayState == false)
            btExit.GetComponent<Button>().interactable = true;

        tRemaingTime.text = remainingTime.ToString("F0");

        CatchCatchGameStart();
        ClawMove();
       // DollCountCheck();
    }

    IEnumerator CatchCatch1()
    {
        yield return null;

        animator.Play("ani_CatchCatch");

        time1 += Time.deltaTime;

        if (time1 >= 5.0f)
            StartCoroutine("CatchCatch2");
    }

    IEnumerator CatchCatch2()
    {
        yield return null;

        isCatch = true;

        animator.speed = 0f;

        if (clawRailColumn.transform.localPosition.x < 80.0f)
        {
            claw3.transform.Translate(new Vector3(clawSpeed * Time.deltaTime, 0f, 0f));
            clawRailColumn.transform.Translate(new Vector3(clawSpeed * Time.deltaTime, 0f, 0f));
        }
        else if (clawRailRow.transform.localPosition.z > -41.5f)
        {
            claw3.transform.Translate(new Vector3(0f, 0f, -clawSpeed * Time.deltaTime));
            clawRailRow.transform.Translate(new Vector3(0f, clawSpeed * Time.deltaTime, 0f));
        }

        if (claw3.transform.localPosition.x > -5.0f)
            claw3.transform.localPosition = new Vector3(-5.0f, 0f, claw3.transform.localPosition.z);
        if (claw3.transform.localPosition.z < 2.5f)
            claw3.transform.localPosition = new Vector3(claw3.transform.localPosition.x, 0f, 2.5f);

        if (clawRailColumn.transform.localPosition.x > 80.0f)
            clawRailColumn.transform.localPosition = new Vector3(80.0f, 240.0f, 0f);

        if (clawRailRow.transform.localPosition.z < -41.5f)
            clawRailRow.transform.localPosition = new Vector3(0f, 240.0f, -41.5f);

        if (clawRailColumn.transform.localPosition.x != 80.0f || clawRailRow.transform.localPosition.z != -41.5f)
            AudioManager.Instance.AudioRail();

        if (claw3.transform.localPosition.x == -5.0f && claw3.transform.localPosition.z == 2.5f)
            StartCoroutine("CatchCatch3");
    }

    IEnumerator CatchCatch3()
    {
        yield return null;

        if (time2 <= 0)
        {
            var dollCount = follow.transform.childCount;

            if (dollCount != 0)
            {
                for (int i = 0; i < dollCount; i++)
                    follow.transform.GetChild(0).SetParent(dollLocation.transform);
            }
        }

        follow.GetComponent<SphereCollider>().enabled = false;

        animator.speed = 1.0f;

        time2 += Time.deltaTime;

        if (time2 >= 2.0f)
            StartCoroutine("CatchCatch4");
    }

    IEnumerator CatchCatch4()
    {
        yield return null;

        animator.Rebind();

        isCatch = false;
        btClick = false;
        isPlayState = false;

        remainingTime = 20.0f;
        time1 = 0f;
        time2 = 0f;

        btExit.GetComponent<Button>().interactable = true;

        var tiggerLen = list_Trigger.Count;

        for (int i = 0; i < tiggerLen; i++)
            list_Trigger[i].isTrigger = true;
    }

    public void CatchCatchGameStart()
    {
        if (joyStick.Horizontal != 0f || joyStick.Vertical != 0f)
        {
            if (respawnTime >= 3.0f && GameManager.Instance.countPlay > 0)
            {
                isPlayState = true;

                if (remainingTime >= 20.0f)
                {
                    follow.GetComponent<SphereCollider>().enabled = true;
                    btExit.GetComponent<Button>().interactable = false;
                    GameManager.Instance.countPlay -= 1;
                    GameManager.Instance.GameCountPlay();
                    GameManager.Instance.Save();

                    var tiggerLen = list_Trigger.Count;

                    for (int i = 0; i < tiggerLen; i++)
                        list_Trigger[i].isTrigger = false;
                }
            }
        }
    }

    public void CatchCatchBT()
    {
        if (isPlayState == true)
            btClick = true;
    }

    public void ClawMove()
    {
        if (isPlayState == true)
        {
            if (btClick == false && remainingTime > 0f)
            {
                claw3.transform.Translate(new Vector3(-joyStick.Horizontal * clawSpeed * Time.deltaTime, 0f, -joyStick.Vertical * clawSpeed * Time.deltaTime));
                clawRailRow.transform.Translate(new Vector3(0, joyStick.Vertical * clawSpeed * Time.deltaTime, 0f));
                clawRailColumn.transform.Translate(new Vector3(-joyStick.Horizontal * clawSpeed * Time.deltaTime, 0f, 0f));

                if (claw3.transform.localPosition.x < -148.0f)
                    claw3.transform.localPosition = new Vector3(-148.0f, 0f, claw3.transform.localPosition.z);
                else if (claw3.transform.localPosition.x > -5.0f)
                    claw3.transform.localPosition = new Vector3(-5.0f, 0f, claw3.transform.localPosition.z);
                if (claw3.transform.localPosition.z > 102.0f)
                    claw3.transform.localPosition = new Vector3(claw3.transform.localPosition.x, 0f, 102.0f);
                else if (claw3.transform.localPosition.z < 2.5f)
                    claw3.transform.localPosition = new Vector3(claw3.transform.localPosition.x, 0f, 2.5f);

                if (clawRailRow.transform.localPosition.z > 58.0f)
                    clawRailRow.transform.localPosition = new Vector3(0f, 240.0f, 58.0f);
                else if (clawRailRow.transform.localPosition.z < -41.5f)
                    clawRailRow.transform.localPosition = new Vector3(0f, 240.0f, -41.5f);

                if (clawRailColumn.transform.localPosition.x < -63.0f)
                    clawRailColumn.transform.localPosition = new Vector3(-63.0f, 240.0f, 0f);
                else if (clawRailColumn.transform.localPosition.x > 80.0f)
                    clawRailColumn.transform.localPosition = new Vector3(80.0f, 240.0f, 0f);

                remainingTime -= Time.deltaTime;

                if (joyStick.Horizontal != 0 || joyStick.Vertical != 0f)
                    AudioManager.Instance.AudioRail();
            }
            else if (btClick == true || remainingTime <= 0f)
            {
                if (time1 <= 0f)
                    AudioManager.Instance.AudioPush();

                StartCoroutine("CatchCatch1");
            }
        }
    }

    public void DollCountCheck()
    {
        if (countCatchCatch == 0)
        {
            countCatchCatch = 12;
            respawnTime = 0f;
            CatchCatchDollDrop.Instance.StartCoroutine("DollDropCatchCatch");
        }
    }
}