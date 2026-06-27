using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PushPushDollDrop : MonoBehaviour
{
    public List<GameObject> list_Doll = new List<GameObject>();

    public List<Transform> spwan_Point = new List<Transform>();

    private static PushPushDollDrop instance;
    public static PushPushDollDrop Instance
    {
        get
        {
            if (instance == null)
            {
                var obj = FindObjectOfType<PushPushDollDrop>();

                if (obj != null)
                    instance = obj;
                else
                {
                    var newObj = new GameObject().AddComponent<PushPushDollDrop>();

                    instance = newObj;
                }
            }
            return instance;
        }
    }

    private void Awake()
    {
        var objs = FindObjectsOfType<PushPushDollDrop>();

        if (objs.Length != 1)
        {
            Destroy(gameObject);
            return;
        }
    }

    public void DollDropPushPush()
    {
        var len = spwan_Point.Count;
        for (int i = 0; i < len; i++)
        {
            var num = Random.Range(0, Doll.Instance.doll_PushPush.Count);
            list_Doll[i] = Instantiate(Doll.Instance.doll_PushPush[num], spwan_Point[i]);
        }
    }
}