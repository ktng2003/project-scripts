using GoogleMobileAds.Api;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization.Settings;
using UnityEngine.Purchasing;

public class Shop : MonoBehaviour
{
    [System.Serializable]
    public struct Skin
    {
        public string name;
        public Sprite image;
    }

    public List<Skin> list_Skin = new List<Skin>();

    public List<GameObject> Slot_Skin = new List<GameObject>();

    public GameObject blockRemoveAllAds;
    public GameObject preSkin;
    public GameObject parentSkin;
    public GameObject scrollSize;

    public Button btRemoveAllAds;

    public Transform scrollPos;

    private static Shop instance;
    public static Shop Instance
    {
        get
        {
            if (instance == null)
            {
                var obj = Resources.FindObjectsOfTypeAll<Shop>().FirstOrDefault();

                if (obj != null)
                    instance = obj;
                else
                    Debug.LogError("instance not found");
            }
            return instance;
        }
    }

    private void Awake()
    {
        var objs = Resources.FindObjectsOfTypeAll<Shop>();

        if (objs.Length > 1)
        {
            Destroy(gameObject);
            return;
        }

        if (instance == null)
            instance = this;
    }

    void Start()
    {
        CreateSlot();
    }

    public void CreateSlot()
    {
        var len = list_Skin.Count;
        for (int i = 0; i < len; i++)
        {
            if (i > 0)
            {
                Slot_Skin.Add(Instantiate(preSkin, scrollPos));
                Slot_Skin[i].SetActive(true);
                Slot_Skin[i].name = i.ToString();
                Slot_Skin[i].transform.GetChild(1).GetComponent<Image>().sprite = list_Skin[i].image;
                Slot_Skin[i].transform.GetChild(2).GetChild(0).GetComponent<Text>().text = list_Skin[i].name;

                if (GameManager.Instance.is_Skin[i] == true)
                    Slot_Skin[i].transform.GetChild(4).gameObject.SetActive(false);
            }

            if (GameManager.Instance.selectSkin == i)
                Slot_Skin[i].transform.GetChild(3).gameObject.SetActive(true);
        }

        scrollSize.GetComponent<RectTransform>().sizeDelta = new Vector2(scrollSize.GetComponent<RectTransform>().rect.width, 875.0f + (len - 1) / 4 * 320.0f);
    }
}