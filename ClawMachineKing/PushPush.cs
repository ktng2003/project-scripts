using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PushPush : MonoBehaviour
{
    public GameObject btExit;
    public GameObject push;

    public int countPushPush = 6;

    public bool isPlayState = false;

    public float respawnTime = 0f;

    private Rigidbody rbPush;

    private bool btClick1 = false;
    private bool btClick2 = false;
    private bool btClick3 = false;

    private float pushSpeed = 70.0f;
    private float pushTime = 0f;

    private static PushPush instance;
    public static PushPush Instance
    {
        get
        {
            if (instance == null)
            {
                var obj = FindObjectOfType<PushPush>();

                if (obj != null)
                    instance = obj;
                else
                {
                    var newObj = new GameObject().AddComponent<PushPush>();

                    instance = newObj;
                }
            }
            return instance;
        }
    }

    private void Awake()
    {
        var objs = FindObjectsOfType<PushPush>();

        if (objs.Length != 1)
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        rbPush = push.GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (respawnTime <= 0.5f)
        {
            respawnTime += Time.deltaTime;
            btExit.GetComponent<Button>().interactable = false;
        }
        else if (respawnTime >= 0.5f && isPlayState == false)
            btExit.GetComponent<Button>().interactable = true;

        PushMove();
        //DollCountCheck();
    }

    IEnumerator PushPush1()
    {
        yield return null;

        if (push.transform.localPosition.z >= 50.0f)
        {
            push.transform.localPosition = new Vector3(push.transform.localPosition.x, push.transform.localPosition.y, 50.0f);
            rbPush.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
            StartCoroutine("PushPush2");
        }
        else
            rbPush.velocity = new Vector3(0f, 0f, -pushSpeed);
    }

    IEnumerator PushPush2()
    {
        yield return null;

        if (push.transform.localPosition.y <= 92.5f)
        {
            push.transform.localPosition = new Vector3(push.transform.localPosition.x, 92.5f, push.transform.localPosition.z);
            rbPush.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
            StartCoroutine("PushPush3");
        }
        else
            rbPush.velocity = new Vector3(0f, -pushSpeed, 0f);

        AudioManager.Instance.AudioRail();
    }

    IEnumerator PushPush3()
    {
        yield return null;

        if (push.transform.localPosition.x >= 95.0f)
        {
            push.transform.localPosition = new Vector3(95.0f, push.transform.localPosition.y, push.transform.localPosition.z);
            StartCoroutine("PushPush4");
        }
        else
            rbPush.velocity = new Vector3(-pushSpeed, 0f, 0f);
    }

    IEnumerator PushPush4()
    {
        yield return null;

        btClick1 = false;
        btClick2 = false;
        btClick3 = false;
        isPlayState = false;

        pushTime = 0f;

        rbPush.velocity = new Vector3(0f, 0f, 0f);
        btExit.GetComponent<Button>().interactable = true;
    }

    public void PushPushGameStart()
    {
        if (isPlayState == false && btClick1 == false && GameManager.Instance.countPlay > 0)
        {
            isPlayState = true;
            btExit.GetComponent<Button>().interactable = false;
            rbPush.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
            GameManager.Instance.countPlay -= 1;
            GameManager.Instance.GameCountPlay();
            GameManager.Instance.Save();
        }
    }

    public void PushPushBT()
    {
        if (isPlayState == true)
        {
            if (btClick1 == true && btClick2 == false)
            {
                btClick2 = true;
                rbPush.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
                AudioManager.Instance.AudioPush();
            }

            if (btClick1 == false)
            {
                btClick1 = true;
                rbPush.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
            }
        }
    }

    public void PushMove()
    {
        if (isPlayState == true)
        {
            if (btClick1 == false)
                rbPush.velocity = new Vector3(pushSpeed, 0f, 0f);

            if (btClick1 == false && push.transform.localPosition.x < -95.0f)
            {
                btClick1 = true;
                push.transform.localPosition = new Vector3(-95.0f, push.transform.localPosition.y, push.transform.localPosition.z);
                rbPush.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
            }

            if (btClick1 == true && btClick2 == false)
                rbPush.velocity = new Vector3(0f, pushSpeed, 0f);

            if (btClick2 == false && push.transform.localPosition.y > 227.5f)
            {
                btClick2 = true;
                push.transform.localPosition = new Vector3(push.transform.localPosition.x, 227.5f, push.transform.localPosition.z);
                rbPush.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
                AudioManager.Instance.AudioPush();
            }

            if (btClick2 == true && btClick3 == false)
            {
                rbPush.velocity = new Vector3(0f, 0f, pushSpeed);
                pushTime += Time.deltaTime;
            }

            if (btClick3 == false && pushTime >= 1.2f)
                btClick3 = true;

            if (btClick2 == false)
                AudioManager.Instance.AudioRail();

            if (btClick3 == true)
                StartCoroutine("PushPush1");
        }
    }

    public void DollCountCheck()
    {
        if (countPushPush == 0)
        {
            countPushPush = 6;
            respawnTime = 0f;
            PushPushDollDrop.Instance.DollDropPushPush();
        }
    }
}