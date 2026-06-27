using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Collection : MonoBehaviour
{
    public List<GameObject> list_Collection = new List<GameObject>();
    public List<GameObject> pre_Collection = new List<GameObject>();
    public List<GameObject> Slot_CatchCatch = new List<GameObject>();
    public List<GameObject> Slot_PushPush = new List<GameObject>();
    public List<GameObject> Slot_SpinSpin = new List<GameObject>();
    public List<GameObject> bt_CollectionType = new List<GameObject>();

    public List<TextMeshProUGUI> t_CollectionType = new List<TextMeshProUGUI>();

    public List<Transform> scroll_Pos = new List<Transform>();

    public Color32 selectedCollection;
    public Color32 unSelectedCollection;

    public TextMeshProUGUI collectionDollCount;

    private GameObject btBackCollectionType;
    private GameObject btNowCollectionType;

    private TextMeshProUGUI tBackCollectionType;
    private TextMeshProUGUI tNowCollectionType;

    public int collectionState;

    private static Collection instance;
    public static Collection Instance
    {
        get
        {
            if (instance == null)
            {
                var obj = FindObjectOfType<Collection>();

                if (obj != null)
                    instance = obj;
                else
                {
                    var newObj = new GameObject().AddComponent<Collection>();

                    instance = newObj;
                }
            }
            return instance;
        }
    }

    private void Awake()
    {
        var objs = FindObjectsOfType<Collection>();

        if (objs.Length != 1)
        {
            Destroy(gameObject);
            return;
        }
        CreateSlot();
    }

    private void OnEnable()
    {
        collectionState = 0;

        list_Collection[0].SetActive(true);
        bt_CollectionType[0].GetComponent<Image>().color = selectedCollection;
        bt_CollectionType[0].GetComponent<Button>().interactable = false;
        t_CollectionType[0].color = selectedCollection;

        btNowCollectionType = bt_CollectionType[0];
        tNowCollectionType = t_CollectionType[0];

        collectionDollCount.text = GameManager.Instance.acquiredCatchCatchCount.ToString() + " / 100";
    }

    public void CreateSlot()
    {
        var lenCatchCatch = Doll.Instance.doll_CatchCatch.Count;
        for (int i = 0; i < lenCatchCatch; i++)
        {
            Slot_CatchCatch.Add(Instantiate(pre_Collection[0], scroll_Pos[0]));
            Slot_CatchCatch[i].SetActive(true);
            Slot_CatchCatch[i].name = i.ToString();
            Slot_CatchCatch[i].transform.GetChild(0).GetComponent<Image>().sprite = Doll.Instance.atlasCatchCatch.GetSprite("img_Doll_CatchCatch" + (i + 1));
        }

        var lenPushPush = Doll.Instance.doll_PushPush.Count;
        for (int j = 0; j < lenPushPush; j++)
        {
            Slot_PushPush.Add(Instantiate(pre_Collection[1], scroll_Pos[1]));
            Slot_PushPush[j].SetActive(true);
            Slot_PushPush[j].name = j.ToString();
            Slot_PushPush[j].transform.GetChild(0).GetComponent<Image>().sprite = Doll.Instance.atlasPushPush.GetSprite("img_Doll_PushPush" + (j + 1));
        }

        var lenSpinSpin = Doll.Instance.doll_SpinSpin.Count;
        for (int k = 0; k < lenSpinSpin; k++)
        {
            Slot_SpinSpin.Add(Instantiate(pre_Collection[2], scroll_Pos[2]));
            Slot_SpinSpin[k].SetActive(true);
            Slot_SpinSpin[k].name = k.ToString();
            Slot_SpinSpin[k].transform.GetChild(0).GetComponent<Image>().sprite = Doll.Instance.atlasSpinSpin.GetSprite("img_Doll_SpinSpin" + (k + 1));
        }
    }

    public void TabClickCollection(int _index)
    {
        btBackCollectionType = btNowCollectionType;
        btNowCollectionType = bt_CollectionType[_index];

        tBackCollectionType = tNowCollectionType;
        tNowCollectionType = t_CollectionType[_index];

        collectionState = _index;

        scroll_Pos[_index].transform.localPosition = new Vector3(scroll_Pos[_index].transform.localPosition.x, 0f, 0f);

        int len = bt_CollectionType.Count;
        for (int i = 0; i < len; i++)
            list_Collection[i].gameObject.SetActive(false);

        list_Collection[_index].gameObject.SetActive(true);

        if (_index == 0)
            collectionDollCount.text = GameManager.Instance.acquiredCatchCatchCount.ToString() + " / 100";
        else if (_index == 1)
            collectionDollCount.text = GameManager.Instance.acquiredPushPushCount.ToString() + " / 50";
        else if (_index == 2)
            collectionDollCount.text = GameManager.Instance.acquiredSpinSpinCount.ToString() + " / 50";

        btBackCollectionType.GetComponent<Button>().interactable = true;
        btNowCollectionType.GetComponent<Button>().interactable = false;

        btBackCollectionType.GetComponent<Image>().color = unSelectedCollection;
        btNowCollectionType.GetComponent<Image>().color = selectedCollection;

        tBackCollectionType.color = unSelectedCollection;
        tNowCollectionType.color = selectedCollection;
    }
}