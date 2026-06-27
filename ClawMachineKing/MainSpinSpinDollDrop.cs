using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MainSpinSpinDollDrop : MonoBehaviour
{
    public List<Transform> spwan_Point = new List<Transform>();

    void Start()
    {
        DollDropMainSpinSpin();
    }

    void DollDropMainSpinSpin()
    {
        var dolls = Doll.Instance.doll_SpinSpin;
        int dollCount = dolls.Count;
        for (int i = 0; i < spwan_Point.Count; i++)
        {
            int num = Random.Range(0, dollCount);
            var doll = Instantiate(dolls[num], spwan_Point[i]);
            Destroy(doll.GetComponent<Rigidbody>());
        }
    }
}