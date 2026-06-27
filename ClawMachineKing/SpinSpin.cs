using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpinSpin : MonoBehaviour
{
    public GameObject btExit;
    public GameObject Arrow;
    public GameObject Floor;

    public int countSpinSpin = 10;

    public bool isPlayState = false;

    public float respawnTime = 0f;

    private Rigidbody rbArrow;

    private bool isSpin1 = false;
    private bool isSpin2 = false;

    private float arrowSpeed = 15.0f;
    private float floorSpeed = 30.0f;

    private static SpinSpin instance;
    public static SpinSpin Instance
    {
        get
        {
            if (instance == null)
            {
                var obj = FindObjectOfType<SpinSpin>();

                if (obj != null)
                    instance = obj;
                else
                {
                    var newObj = new GameObject().AddComponent<SpinSpin>();

                    instance = newObj;
                }
            }
            return instance;
        }
    }

    private void Awake()
    {
        var objs = FindObjectsOfType<SpinSpin>();

        if (objs.Length != 1)
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        rbArrow = Arrow.GetComponent<Rigidbody>();
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

        if (isPlayState == false)
            Floor.transform.Rotate(new Vector3(0f, -floorSpeed * Time.deltaTime, 0f));

        SpinMove();
       // DollCountCheck();
    }

    IEnumerator SpinSpin1()
    {
        yield return null;

        if (Arrow.transform.localPosition.y >= 205.0f)
        {
            Arrow.transform.localPosition = new Vector3(0f, 205.0f, 0f);
            isSpin2 = true;
            StartCoroutine("SpinSpin2");
        }
        else
            rbArrow.velocity = new Vector3(0f, arrowSpeed, 0f);
    }

    IEnumerator SpinSpin2()
    {
        yield return null;

        isPlayState = false;
        isSpin1 = false;
        isSpin2 = false;

        rbArrow.velocity = new Vector3(0f, 0f, 0f);
        btExit.GetComponent<Button>().interactable = true;
    }

    public void SpinSpinGameStart()
    {
        if (GameManager.Instance.countPlay > 0 && isPlayState == false)
        {
            isPlayState = true;
            btExit.GetComponent<Button>().interactable = false;
            GameManager.Instance.countPlay -= 1;
            GameManager.Instance.GameCountPlay();
            GameManager.Instance.Save();
            AudioManager.Instance.AudioPush();
        }
    }

    public void SpinMove()
    {
        if (isPlayState == true)
        {
            if (isSpin1 == false)
                rbArrow.velocity = new Vector3(0f, -arrowSpeed, 0f);

            if (isSpin1 == false && Arrow.transform.localPosition.y < 190.0f)
                isSpin1 = true;

            if (isSpin1 == true)
                StartCoroutine("SpinSpin1");
        }
    }

    public void DollCountCheck()
    {
        if (countSpinSpin == 0)
        {
            var len = SpinSpinDollDrop.Instance.list_Claw.Count;

            for (int i = 0; i < len; i++)
            {
                var clawPos = SpinSpinDollDrop.Instance.list_Claw[i].transform;

                clawPos.localPosition = new Vector3(clawPos.localPosition.x, clawPos.localPosition.y, -1.0f);
                clawPos.Rotate(new Vector3(12.0f, clawPos.rotation.y, clawPos.rotation.z));
            }

            countSpinSpin = 10;
            respawnTime = 0f;
            SpinSpinDollDrop.Instance.DollDropSpinSpin();
        }
    }
}