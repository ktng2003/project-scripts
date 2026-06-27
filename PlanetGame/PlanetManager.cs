using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

public class PlanetManager : MonoBehaviour
{
    public Dictionary<int, IObjectPool<GameObject>> pools = new Dictionary<int, IObjectPool<GameObject>>();

    public List<Sprite> next_Planet = new List<Sprite>();
    public List<GameObject> list_Planet = new List<GameObject>();  
    
    public Color32 blackHoleBaseColor;
    public Color32 blackHoleWarningColor;

    public Image nextPlanet;

    public Transform selectedPlanet;
    public Transform blackHoleField;

    public Material blackHoleRangeMax; 

    public int nextIndex;

    public bool isWarning = false;
    public bool isWarningAds = false;
    public bool isEscape = false;

    public float warningRadius = 33.0f;
    public float escapeRadius = 37.0f;
    public float warningTimer = 0f;
    public float escapeTimer = 0f;

    private static PlanetManager instance;
    public static PlanetManager Instance
    {
        get
        {
            if (instance == null)
            {
                var obj = FindObjectOfType<PlanetManager>();

                if (obj != null)
                    instance = obj;
                else
                {
                    var newObj = new GameObject().AddComponent<PlanetManager>();

                    instance = newObj;
                }
            }
            return instance;
        }
    }

    private void Awake()
    {
        var objs = FindObjectsOfType<PlanetManager>();
        if (objs.Length != 1)
        {
            Destroy(gameObject);
            return;
        }

        SelectPlanet();
    }

    private void Start()
    {
        blackHoleRangeMax.color = blackHoleBaseColor;
    }

    void Update()
    {
        CheckBlackHoleWarning();
        CheckBlackHoleEscape();
    }

    public void InitPlanet(GameObject _planet, Transform _parent, bool _isSelected, bool _isMerged)
    {
        _planet.transform.SetParent(_parent);

        _planet.GetComponent<Planet>().isSelected = _isSelected;
        _planet.GetComponent<Planet>().isMerged = _isMerged;

        _planet.GetComponent<SphereCollider>().enabled = true;
    }

    public GameObject SelectPlanet()
    {
        int index;
        if (GameManager.Instance.isFirstStart == false)
        {
            index = Random.Range(0, 5);
            nextIndex = Random.Range(0, 5);
            nextPlanet.sprite = next_Planet[nextIndex];

            GameManager.Instance.isFirstStart = true;
        }
        else
            index = nextIndex;

        if (!pools.ContainsKey(index))
        {
            pools[index] = new ObjectPool<GameObject>(
                () => CreatePooledItem(index),
                OnTakeFromPool,
                OnReturnedToPool,
                OnDestroyPoolObject,
                true, 0, 50
            );
        }
        GameObject planet = pools[index].Get();

        InitPlanet(planet, selectedPlanet, true, false);

        planet.transform.localPosition = Vector3.zero;
        planet.transform.localRotation = Quaternion.identity;
        planet.GetComponent<Rigidbody>().velocity = Vector3.zero;
        planet.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        planet.GetComponent<Rigidbody>().angularDrag = 0f;

        nextIndex = Random.Range(0, 5);
        nextPlanet.sprite = next_Planet[nextIndex];

        return planet;
    }

    public void ChangeSelectedPlanetRandomly()
    {
        GameObject selectedPlanetChild = selectedPlanet.GetChild(0).gameObject;

        InitPlanet(selectedPlanetChild, blackHoleField, false, false);

        selectedPlanetChild.GetComponent<Planet>().pool.Release(selectedPlanetChild.gameObject);
        selectedPlanetChild.transform.localPosition = Vector3.zero;
        selectedPlanetChild.transform.localRotation = Quaternion.identity;
        selectedPlanetChild.GetComponent<Rigidbody>().velocity = Vector3.zero;
        selectedPlanetChild.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

        int index = Random.Range(0, 5);
        if (!pools.ContainsKey(index))
        {
            pools[index] = new ObjectPool<GameObject>(
                () => CreatePooledItem(index),
                OnTakeFromPool,
                OnReturnedToPool,
                OnDestroyPoolObject,
                true, 0, 50
            );
        }
        GameObject planet = pools[index].Get();

        InitPlanet(planet, selectedPlanet, true, false);

        planet.transform.localPosition = Vector3.zero;
        planet.transform.localRotation = Quaternion.identity;
        planet.GetComponent<Rigidbody>().velocity = Vector3.zero;
        planet.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        planet.GetComponent<Rigidbody>().angularDrag = 0f;

        GameManager.Instance.isSelectedPlanetRandomlyChanged = true;
        ButtonController.Instance.changePlanetButton.interactable = false;
    }

    public GameObject MergePlanet(int _index, Vector3 _pos, GameObject _selfObj, GameObject _collidedObj)
    {
        if (!pools.ContainsKey(_index))
        {
            pools[_index] = new ObjectPool<GameObject>(
                () => CreatePooledItem(_index),
                OnTakeFromPool,
                OnReturnedToPool,
                OnDestroyPoolObject,
                true, 0, 50
            );
        }
        GameObject planet = pools[_index].Get();

        InitPlanet(planet, blackHoleField, false, false);

        planet.transform.position = _pos;

        _selfObj.GetComponent<SphereCollider>().enabled = false;
        _collidedObj.GetComponent<SphereCollider>().enabled = false;
        _selfObj.GetComponent<Planet>().pool.Release(_selfObj);
        _collidedObj.GetComponent<Planet>().pool.Release(_collidedObj);

        return planet;
    }

    public void CheckBlackHoleWarning()
    {
        bool isAnyPlanetOutOfRange = false;

        for (int i = 1; i < blackHoleField.childCount; i++)
        {
            if (blackHoleField.GetChild(i).gameObject.activeSelf == false)
                continue;
            if (blackHoleField.GetChild(i).GetComponent<Rigidbody>().angularDrag != 2.0f)
                continue;

            float dist = Vector3.Distance(blackHoleField.position, blackHoleField.GetChild(i).position);
            float planetRadius = blackHoleField.GetChild(i).GetComponent<SphereCollider>().radius;

            if (dist + planetRadius > warningRadius)
            {
                isAnyPlanetOutOfRange = true;
                break;
            }
        }

        if (isAnyPlanetOutOfRange)
        {
            warningTimer += Time.deltaTime;

            if (!isWarning && warningTimer >= 1.0f)
            {
                blackHoleRangeMax.color = blackHoleWarningColor;
                isWarning = true;
            }
        }
        else
        {
            warningTimer = 0f;

            if (isWarning)
            {
                blackHoleRangeMax.color = blackHoleBaseColor;
                isWarning = false;
            }
        }
    }

    public void CheckBlackHoleEscape()
    {
        bool isAnyPlanetOutOfRange1 = false;

        for (int i = 1; i < blackHoleField.childCount; i++)
        {
            if (blackHoleField.GetChild(i).gameObject.activeSelf == false)
                continue;
            if (blackHoleField.GetChild(i).GetComponent<Rigidbody>().angularDrag != 2.0f)
                continue;

            float dist = Vector3.Distance(blackHoleField.position, blackHoleField.GetChild(i).position);
            float planetRadius = blackHoleField.GetChild(i).GetComponent<SphereCollider>().radius;

            if (dist + planetRadius > escapeRadius)
            {
                isAnyPlanetOutOfRange1 = true;
                break;
            }
        }

        if (isAnyPlanetOutOfRange1)
        {
            escapeTimer += Time.deltaTime;

            if (!isEscape && escapeTimer >= 3.0f)
            {
                isWarningAds = true;
                RewardAdsManager.Instance.ShowAdsForRestartGame();
                escapeTimer = 0f;
                isEscape = true;
            }
        }
        else
        {
            escapeTimer = 0f;
            isEscape = false;
        }
    }

    private GameObject CreatePooledItem(int index)
    {
        GameObject poolGo = Instantiate(list_Planet[index], transform);
        poolGo.GetComponent<Planet>().pool = pools[index];
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
}