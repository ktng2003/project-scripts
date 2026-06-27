using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Purchasing;
using UnityEngine.UI;
using UnityEngineInternal;

public class GameManager : MonoBehaviour
{
    public GameObject arrow;
    public GameObject arrowHead;
    public GameObject arrowTail;
    public GameObject arrowBodyStart;
    public GameObject arrowBodyEnd;

    public Text tScore;
    public Text tBestScore;

    public Transform blackHole;

    public RectTransform fire;

    public int score = 0;
    public int bestScore = 0;

    public bool isRemoveAllAds = false;
    public bool isFirstStart = false;
    public bool isSelectedPlanetRandomlyChanged = false;
    public bool isFireReady = true;

    public float minfireForce = 1500.0f;
    public float maxfireForce = 3000.0f;
    public float minDragDistance = 100.0f;
    public float maxDragDistance = 200.0f;
    public float minZForce = 0f;
    public float maxZForce = 2500.0f;

    private Vector2 lastMousePos;

    private bool isButtonHeld = false;


    public bool isAnyPlanetOutOfRange = false;

    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                var obj = FindObjectOfType<GameManager>();

                if (obj != null)
                    instance = obj;
                else
                {
                    var newObj = new GameObject().AddComponent<GameManager>();

                    instance = newObj;
                }
            }
            return instance;
        }
    }

    private void Awake()
    {
        var objs = FindObjectsOfType<GameManager>();
        if (objs.Length != 1)
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        CheckBestScore();
    }

    void Update()
    {
        if (isButtonHeld == true)
        {
            Vector2 uiObjectScreenPos = RectTransformUtility.WorldToScreenPoint(null, fire.position);
            Vector2 mousePos = Input.mousePosition;

            float distance = Vector2.Distance(mousePos, uiObjectScreenPos);

            if (isFireReady == true && distance >= minDragDistance)
            {
                arrow.SetActive(true);

                Vector2 dir = mousePos - uiObjectScreenPos;
                Vector2 dirNorm = dir.normalized;
                float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                arrow.transform.rotation = Quaternion.Euler(0f, 0f, angle + 90.0f);
                arrow.transform.position = uiObjectScreenPos;

                float clampedDistance = Mathf.Clamp(distance, minDragDistance, maxDragDistance);
                float t = (clampedDistance - minDragDistance) / (maxDragDistance - minDragDistance);

                arrowHead.transform.localPosition = Vector3.Lerp(new Vector3(0, 190.0f, 0), new Vector3(0, 330.0f, 0), t);
                arrowTail.transform.localPosition = Vector3.Lerp(new Vector3(0, -100.0f, 0), new Vector3(0, -170.0f, 0), t);
                arrowBodyStart.GetComponent<Image>().fillAmount = Mathf.Lerp(0.55f, 1.0f, t);
                arrowBodyEnd.GetComponent<Image>().fillAmount = Mathf.Lerp(0.55f, 1.0f, t);
            }
            else
                arrow.SetActive(false);
        }
    }

    public void CheckBestScore()
    {
        if (PlayerPrefs.HasKey("BestScore") == true)
            bestScore = PlayerPrefs.GetInt("BestScore");
        else
            PlayerPrefs.SetInt("BestScore", bestScore);

        tBestScore.text = bestScore.ToString();
    }

    public void SetBestScore(int _bestScore)
    {
        PlayerPrefs.SetInt("BestScore", _bestScore);

        bestScore = _bestScore;
        tBestScore.text = bestScore.ToString();
    }

    public void ButtonPointerDown()
    {
        isButtonHeld = true;
        lastMousePos = Input.mousePosition;
    }

    public void ButtonPointerUp()
    {
        isButtonHeld = false;

        if (isFireReady == false)
            return;

        Vector2 uiObjectScreenPos = RectTransformUtility.WorldToScreenPoint(null, fire.position);
        Vector2 mousePos = Input.mousePosition;

        float distance = Vector2.Distance(mousePos, uiObjectScreenPos);
        if (distance < minDragDistance)
            return;

        Vector2 direction = mousePos - uiObjectScreenPos;
        direction.Normalize();

        Vector3 direction3D = new Vector3(direction.x, 0f, direction.y);

        float clampedDistance = Mathf.Clamp(distance, minDragDistance, maxDragDistance);
        float t = (clampedDistance - minDragDistance) / (maxDragDistance - minDragDistance);
        float finalForce = Mathf.Lerp(minfireForce, maxfireForce, t);
        float zForce = Mathf.Lerp(minZForce, maxZForce, t);

        Vector2 screenDirection = direction.normalized;
        Vector3 worldDirection = Camera.main.transform.right * screenDirection.x + Camera.main.transform.up * screenDirection.y;

        Transform selectedPlanet = PlanetManager.Instance.selectedPlanet.GetChild(0);
        Rigidbody rb = selectedPlanet.GetComponent<Rigidbody>();

        Vector3 horizontalForce = -worldDirection.normalized * finalForce;
        Vector3 zForceVector = selectedPlanet.transform.forward.normalized * zForce;
        Vector3 totalForce = horizontalForce + zForceVector;
        rb.AddForce(totalForce);

        Vector3 fireDir = -worldDirection.normalized;
        Vector3 localFireDir = selectedPlanet.InverseTransformDirection(fireDir);

        float torquePower = finalForce * 0.01f;
        Vector3 torque = Vector3.zero;
        torque += Vector3.right * localFireDir.y * torquePower;
        torque += Vector3.up * (-localFireDir.x) * torquePower;

        Vector3 worldTorque = selectedPlanet.TransformDirection(torque);
        rb.AddTorque(worldTorque, ForceMode.Impulse);

        selectedPlanet.SetParent(PlanetManager.Instance.blackHoleField);
        selectedPlanet.GetComponent<Planet>().isSelected = false;

        isFireReady = false;

        SoundManager.Instance.PlayFireSound();
        Invoke("UpdatePlanetState", 0.5f);

        arrow.SetActive(false);
    }

    private void UpdatePlanetState()
    {
        PlanetManager.Instance.SelectPlanet();
        isFireReady = true;
    }

    public void RestartGame()
    {
        Transform selectedPlanet = PlanetManager.Instance.selectedPlanet.GetChild(0);
        selectedPlanet.SetParent(PlanetManager.Instance.blackHoleField);

        var blackHoleField = PlanetManager.Instance.blackHoleField;
        int len = blackHoleField.childCount;
        for (int i = 1; i < len; i++)
        {
            if (blackHoleField.GetChild(i).gameObject.activeSelf == true)
                blackHoleField.GetChild(i).GetComponent<Planet>().pool.Release(blackHoleField.GetChild(i).gameObject);

            blackHoleField.GetChild(i).localPosition = Vector3.zero;
            blackHoleField.GetChild(i).rotation = Quaternion.identity;
            blackHoleField.GetChild(i).GetComponent<Planet>().isSelected = false;
            blackHoleField.GetChild(i).GetComponent<Planet>().isMerged = false;
            blackHoleField.GetChild(i).GetComponent<SphereCollider>().enabled = true;
            blackHoleField.GetChild(i).GetComponent<Rigidbody>().velocity = Vector3.zero;
            blackHoleField.GetChild(i).GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        }

        PlanetManager.Instance.nextIndex = Random.Range(0, 5);
        PlanetManager.Instance.SelectPlanet();

        if (PlanetManager.Instance.isWarningAds == true)
        {
            if (score > bestScore)
            {
                bestScore = score;
                tBestScore.text = bestScore.ToString();
                PlayerPrefs.SetInt("BestScore", bestScore);
                GPGSManager.Instance.AddScore();
            }
        }

        score = 0;
        tScore.text = score.ToString();

        isSelectedPlanetRandomlyChanged = false;
        ButtonController.Instance.changePlanetButton.interactable = true;
        PlanetManager.Instance.isWarningAds = false;
    }
}