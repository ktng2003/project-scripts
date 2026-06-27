using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MainPushPushDollDrop : MonoBehaviour
{
    public List<Transform> spwan_Point = new List<Transform>();

    void Start()
    {
        DollDropMainPushPush();
    }

    void DollDropMainPushPush()
    {
        var dolls = Doll.Instance.doll_PushPush;
        int dollCount = dolls.Count;
        int len = spwan_Point.Count;
        for (int i = 0; i < len; i++)
        {
            int num = Random.Range(0, dollCount);
            var doll = Instantiate(dolls[num], spwan_Point[i]);
            Destroy(doll.GetComponent<Rigidbody>());
        }
    }
}