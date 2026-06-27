using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;

public class RollSpawner : MonoBehaviour
{
    public IObjectPool<GameObject> Pool { get; private set; }

    public List<GameObject> pool_Queue = new List<GameObject>();

    public GameObject preRoll;

    public int direction;    

    public float timer; 
    public float despawnTime = 2.5f;
    public float spwanTimer = 4.0f;
    public float checkTimer = 0.6f;
    public float animationSpeed = 1.0f;

    private static RollSpawner instance;
    public static RollSpawner Instance
    {
        get
        {
            if (instance == null)
            {
                var obj = FindObjectOfType<RollSpawner>();

                if (obj != null)
                    instance = obj;
                else
                {
                    var newObj = new GameObject().AddComponent<RollSpawner>();
                    instance = newObj;
                }
            }
            return instance;
        }
    }

    private void Awake()
    {
        var objs = FindObjectsOfType<RollSpawner>();
        if (objs.Length != 1)
        {
            Destroy(gameObject);
            return;
        }

        Pool = new ObjectPool<GameObject>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool,
        OnDestroyPoolObject, true, 0, 100);
    }

    void Update()
    {
        if (GameManager.Instance.gameState == GameManager.GameState.Play)
        {
            GameBalance();
            if (GameManager.Instance.isRoll == false)
            {
                if (timer <= spwanTimer)
                    timer += Time.deltaTime;
                else
                {
                    timer = 0f;
                    RollRandomPattern();
                }
            }
        }
    }

    private GameObject CreatePooledItem()
    {
        GameObject poolGo = Instantiate(preRoll, transform);
        poolGo.GetComponent<Roll>().Pool = Pool;
        return poolGo;
    }
 
    private void OnTakeFromPool(GameObject poolGo)
    {
        poolGo.SetActive(true);
    }
   
    private void OnReturnedToPool(GameObject poolGo)
    {
        poolGo.SetActive(false);
    }
  
    private void OnDestroyPoolObject(GameObject poolGo)
    {
        Destroy(poolGo);
    }

    public void GameBalance()
    {
        int level = GameManager.Instance.score / 10;
      
        if (level < 4)
        {
            spwanTimer = Mathf.Max(0.5f, 4.0f - 0.35f * level);
            checkTimer = Mathf.Max(0.2f, 0.6f - 0.04f * level);
            animationSpeed = 1.0f + 0.07f * level;
        }
        else
        {
            spwanTimer = Mathf.Max(0.5f, 2.6f * Mathf.Pow(0.95f, level - 4));
            checkTimer = Mathf.Max(0.2f, 0.44f * Mathf.Pow(0.95f, level - 4));
            animationSpeed = Mathf.Min(2.0f, 1.28f + 0.06f * (level - 4));
        }                      
    }

    public void RollSpawnerRight()
    {
        GameObject roll = Pool.Get();
        pool_Queue.Add(roll);

        roll.transform.localPosition = new Vector3(5.0f, 0f, 0f);
        roll.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        roll.GetComponent<Roll>().direction = 0;
    }

    public void RollSpawnerDown()
    {
        GameObject roll = Pool.Get();
        pool_Queue.Add(roll);

        roll.transform.localPosition = new Vector3(0f, 0f, -5.0f);
        roll.transform.rotation = Quaternion.Euler(0f, 90.0f, 0f);
        roll.GetComponent<Roll>().direction = 1;
    }

    public void RollSpawnerLeft()
    {
        GameObject roll = Pool.Get();
        pool_Queue.Add(roll);

        roll.transform.localPosition = new Vector3(-5.0f, 0f, 0f);
        roll.transform.rotation = Quaternion.Euler(0f, 180.0f, 0f);
        roll.GetComponent<Roll>().direction = 2;
    }

    public void RollSpawnerUp()
    {
        GameObject roll = Pool.Get();
        pool_Queue.Add(roll);

        roll.transform.localPosition = new Vector3(0f, 0f, 5.0f);
        roll.transform.rotation = Quaternion.Euler(0f, 270.0f, 0f);
        roll.GetComponent<Roll>().direction = 3;
    }

    public void RollRandomPattern()
    {
        GameManager.Instance.isRoll = true;

        float randomValue = Random.Range(0f, 1.0f);

        if (randomValue < 0.25f)
            StartCoroutine(RollSpawnerPattern1());
        else if (randomValue < 0.45f)
            StartCoroutine(RollSpawnerPattern2());
        else if (randomValue < 0.65f)
            StartCoroutine(RollSpawnerPattern3());
        else if (randomValue < 0.8f)
            StartCoroutine(RollSpawnerPattern4());
        else if (randomValue < 0.85f)
            StartCoroutine(RollSpawnerPattern5());
        else if (randomValue < 0.9f)
            StartCoroutine(RollSpawnerPattern6());
        else if (randomValue < 0.95f)
            StartCoroutine(RollSpawnerPattern7());
        else if (randomValue < 1.0f)
            StartCoroutine(RollSpawnerPattern8());
    }

    public void StartRollSpawner(int _direction)
    {
        switch (_direction)
        {
            case 0:
                RollSpawnerRight();
                break;
            case 1:
                RollSpawnerDown();
                break;
            case 2:
                RollSpawnerLeft();
                break;
            case 3:
                RollSpawnerUp();
                break;
        }
    }

    IEnumerator RollSpawnerPattern1()// 1개
    {
        int randomDirection = Random.Range(0, 4);
        StartRollSpawner(randomDirection);

        GameManager.Instance.isRoll = false;

        yield break;
    }

    IEnumerator RollSpawnerPattern2()// 2개
    {
        int randomDirection = Random.Range(0, 4);
        StartRollSpawner(randomDirection);

        randomDirection = Random.Range(0, 4);
        yield return new WaitForSeconds(1.0f);
        StartRollSpawner(randomDirection);

        GameManager.Instance.isRoll = false;
    }

    IEnumerator RollSpawnerPattern3()// 3개
    {
        int randomDirection = Random.Range(0, 4);
        StartRollSpawner(randomDirection);

        randomDirection = Random.Range(0, 4);
        yield return new WaitForSeconds(1.0f);
        StartRollSpawner(randomDirection);

        randomDirection = Random.Range(0, 4);
        yield return new WaitForSeconds(1.0f);
        StartRollSpawner(randomDirection);

        GameManager.Instance.isRoll = false;
    }

    IEnumerator RollSpawnerPattern4()// 시계방향
    {
        int randomDirection = Random.Range(0, 4);
        StartRollSpawner(randomDirection);

        yield return new WaitForSeconds(1.0f);
        randomDirection = (randomDirection + 1) % 4;
        StartRollSpawner(randomDirection);

        yield return new WaitForSeconds(1.0f);
        randomDirection = (randomDirection + 1) % 4;
        StartRollSpawner(randomDirection);

        yield return new WaitForSeconds(1.0f);
        randomDirection = (randomDirection + 1) % 4;
        StartRollSpawner(randomDirection);

        GameManager.Instance.isRoll = false;
    }

    IEnumerator RollSpawnerPattern5()// 2+3 짝짝 짝짝짝
    {
        int randomDirection = Random.Range(0, 4);
        StartRollSpawner(randomDirection);
        yield return new WaitForSeconds(1.0f);
        StartRollSpawner(randomDirection);

        randomDirection = Random.Range(0, 4);
        yield return new WaitForSeconds(1.0f);
        StartRollSpawner(randomDirection);
        yield return new WaitForSeconds(0.75f);
        StartRollSpawner(randomDirection);
        yield return new WaitForSeconds(0.75f);
        StartRollSpawner(randomDirection);

        GameManager.Instance.isRoll = false;
    }

    IEnumerator RollSpawnerPattern6()// 대한민국 짝짝 짝 짝짝
    {
        int randomDirection = Random.Range(0, 4);
        StartRollSpawner(randomDirection);
        yield return new WaitForSeconds(1.0f);
        StartRollSpawner(randomDirection);

        randomDirection = Random.Range(0, 4);
        yield return new WaitForSeconds(1.5f);
        StartRollSpawner(randomDirection);

        randomDirection = Random.Range(0, 4);
        yield return new WaitForSeconds(1.0f);
        StartRollSpawner(randomDirection);
        yield return new WaitForSeconds(1.0f);
        StartRollSpawner(randomDirection);

        GameManager.Instance.isRoll = false;
    }

    IEnumerator RollSpawnerPattern7()// 4+2 읏따읏따 짝짝
    {
        int randomDirection = Random.Range(0, 4);
        StartRollSpawner(randomDirection);
        yield return new WaitForSeconds(1.25f);
        StartRollSpawner(randomDirection);
        yield return new WaitForSeconds(1.25f);
        StartRollSpawner(randomDirection);
        yield return new WaitForSeconds(1.25f);
        StartRollSpawner(randomDirection);

        randomDirection = Random.Range(0, 4);
        yield return new WaitForSeconds(1.25f);
        StartRollSpawner(randomDirection);
        yield return new WaitForSeconds(1.0f);
        StartRollSpawner(randomDirection);

        GameManager.Instance.isRoll = false;
    }

    IEnumerator RollSpawnerPattern8()// 3+3+2 따읏따 읏따읏 읏읏
    {
        int randomDirection = Random.Range(0, 4);
        StartRollSpawner(randomDirection);
        yield return new WaitForSeconds(1.25f);
        StartRollSpawner(randomDirection);
        yield return new WaitForSeconds(1.25f);
        StartRollSpawner(randomDirection);

        randomDirection = Random.Range(0, 4);
        yield return new WaitForSeconds(1.25f);
        StartRollSpawner(randomDirection);
        yield return new WaitForSeconds(1.25f);
        StartRollSpawner(randomDirection);
        yield return new WaitForSeconds(1.25f);
        StartRollSpawner(randomDirection);

        randomDirection = Random.Range(0, 4);
        yield return new WaitForSeconds(1.25f);
        StartRollSpawner(randomDirection);
        yield return new WaitForSeconds(1.0f);
        StartRollSpawner(randomDirection);

        GameManager.Instance.isRoll = false;
    }
}