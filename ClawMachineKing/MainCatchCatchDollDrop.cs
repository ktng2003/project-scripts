using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MainCatchCatchDollDrop : MonoBehaviour
{
    public List<Transform> spwan_Point = new List<Transform>();

    private List<int> base_Point = new List<int>();

    void Start()
    {
        for (int i = 0; i < spwan_Point.Count; i++)
            base_Point.Add(i);

        StartCoroutine("DollDropMainCatchCatch");
    }

    IEnumerator DollDropMainCatchCatch()
    {
        for (int i = 0; i < 7; i++)
        {
            yield return new WaitForSeconds(0.5f);
            List<int> tmp_Point = new List<int>(base_Point);
            for (int j = 0; j < 3; j++)
            {
                var point = Random.Range(0, tmp_Point.Count);
                var num = Random.Range(0, Doll.Instance.doll_CatchCatch.Count);
                var doll = Instantiate(Doll.Instance.doll_CatchCatch[num], spwan_Point[tmp_Point[point]]);
                StartCoroutine(SetKinematicAfterLanding(doll.GetComponent<Rigidbody>()));
                tmp_Point.RemoveAt(point);
                yield return null;
            }
        }
    }

    IEnumerator SetKinematicAfterLanding(Rigidbody rb)
    {
        yield return new WaitUntil(() => rb.velocity.magnitude < 0.1f);
        yield return new WaitForSeconds(10f);
        Destroy(rb);
    }
}