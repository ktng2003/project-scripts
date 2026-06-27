using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CatchCatchDollDrop : MonoBehaviour
{
    public List<GameObject> list_Doll = new List<GameObject>();

    public List<Transform> spwan_Point = new List<Transform>();

    private static CatchCatchDollDrop instance;
    public static CatchCatchDollDrop Instance
    {
        get
        {
            if (instance == null)
            {
                var obj = FindObjectOfType<CatchCatchDollDrop>();

                if (obj != null)
                    instance = obj;
                else
                {
                    var newObj = new GameObject().AddComponent<CatchCatchDollDrop>();

                    instance = newObj;
                }
            }
            return instance;
        }
    }

    private void Awake()
    {
        var objs = FindObjectsOfType<CatchCatchDollDrop>();

        if (objs.Length != 1)
        {
            Destroy(gameObject);
            return;
        }
    }

    IEnumerator DollDropCatchCatch()
    {
        var tmp = 0;

        for (int i = 0; i < 4; i++)
        {
            yield return new WaitForSeconds(0.75f);

            List<int> tmp_Point = Enumerable.Range(0, spwan_Point.Count).ToList();

            for (int j = 0; j < 3; j++)
            {
                var point = Random.Range(0, tmp_Point.Count);
                var num = Random.Range(0, Doll.Instance.doll_CatchCatch.Count);

                list_Doll[tmp] = Instantiate(Doll.Instance.doll_CatchCatch[num], spwan_Point[tmp_Point[point]]);
                tmp_Point.RemoveAt(point);
                tmp++;

                yield return null;
            }
        }
    }
}