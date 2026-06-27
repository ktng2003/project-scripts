using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SpinSpinDollDrop : MonoBehaviour
{
    public List<GameObject> list_Doll = new List<GameObject>();

    public List<Transform> spwan_Point = new List<Transform>();
    public List<Transform> list_Claw = new List<Transform>();

    private static SpinSpinDollDrop instance;
    public static SpinSpinDollDrop Instance
    {
        get
        {
            if (instance == null)
            {
                var obj = FindObjectOfType<SpinSpinDollDrop>();

                if (obj != null)
                    instance = obj;
                else
                {
                    var newObj = new GameObject().AddComponent<SpinSpinDollDrop>();

                    instance = newObj;
                }
            }
            return instance;
        }
    }

    private void Awake()
    {
        var objs = FindObjectsOfType<SpinSpinDollDrop>();

        if (objs.Length != 1)
        {
            Destroy(gameObject);
            return;
        }
    }

    public void DollDropSpinSpin()
    {
        var len = spwan_Point.Count;
        for (int i = 0; i < len; i++)
        {
            var num = Random.Range(0, Doll.Instance.doll_SpinSpin.Count);
            list_Doll[i] = Instantiate(Doll.Instance.doll_SpinSpin[num], spwan_Point[i]);
        }
    }
}