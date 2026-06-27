using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DollInformation : MonoBehaviour
{
    public List<GameObject> list_DollInfo = new List<GameObject>();
    public List<GameObject> dollInfo_CatchCatch = new List<GameObject>();
    public List<GameObject> dollInfo_PushPush = new List<GameObject>();
    public List<GameObject> dollInfo_SpinSpin = new List<GameObject>();

    private static DollInformation instance;
    public static DollInformation Instance
    {
        get
        {
            if (instance == null)
            {
                var obj = FindObjectOfType<DollInformation>();

                if (obj != null)
                    instance = obj;
                else
                {
                    var newObj = new GameObject().AddComponent<DollInformation>();

                    instance = newObj;
                }
            }
            return instance;
        }
    }

    private void Awake()
    {
        var objs = FindObjectsOfType<DollInformation>();

        if (objs.Length != 1)
        {
            Destroy(gameObject);
            return;
        }
    } 
    void Update()
    {
        DollRotate();
    }

    public void DollRotate()
    {
        if (Collection.Instance.collectionState == 0)
            list_DollInfo[0].transform.GetChild(GameManager.Instance.dollCatchCatch).transform.Rotate(new Vector3(0f, 100.0f * Time.deltaTime, 0f));
        else if (Collection.Instance.collectionState == 1)
            list_DollInfo[1].transform.GetChild(GameManager.Instance.dollPushPush).transform.Rotate(new Vector3(0f, 100.0f * Time.deltaTime, 0f));
        else if (Collection.Instance.collectionState == 2)
            list_DollInfo[2].transform.GetChild(GameManager.Instance.dollSprinSpin).transform.Rotate(new Vector3(0f, 100.0f * Time.deltaTime, 0f));
    }

    public void OffDoll()
    {
        if (Collection.Instance.collectionState == 0)
        {
            list_DollInfo[0].transform.GetChild(GameManager.Instance.dollCatchCatch).transform.rotation = Quaternion.Euler(0f, -30.0f, 0f);
            list_DollInfo[0].transform.GetChild(GameManager.Instance.dollCatchCatch).gameObject.SetActive(false);
        }
        else if (Collection.Instance.collectionState == 1)
        {
            list_DollInfo[1].transform.GetChild(GameManager.Instance.dollPushPush).transform.rotation = Quaternion.Euler(0f, -30.0f, 0f);
            list_DollInfo[1].transform.GetChild(GameManager.Instance.dollPushPush).gameObject.SetActive(false);
        }
        else if (Collection.Instance.collectionState == 2)
        {
            list_DollInfo[2].transform.GetChild(GameManager.Instance.dollSprinSpin).transform.rotation = Quaternion.Euler(0f, -30.0f, 0f);
            list_DollInfo[2].transform.GetChild(GameManager.Instance.dollSprinSpin).gameObject.SetActive(false);
        }
    }
}