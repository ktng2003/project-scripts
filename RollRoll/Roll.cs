using GooglePlayGames;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Roll : MonoBehaviour
{
    public IObjectPool<GameObject> Pool { get; set; } 

    public List<GameObject> list_RollSpawn = new List<GameObject>();

    public int direction;

    public bool isSwipeSuccessful = false;

    public float timer = 0f;

    private float destroyTime;

    private void OnEnable()
    {
        timer = 0f;
        destroyTime = RollSpawner.Instance.despawnTime / RollSpawner.Instance.animationSpeed;
        isSwipeSuccessful = false;

        int len = Shop.Instance.list_Skin.Count;
        for (int i = 0; i < len; i++)
            transform.GetChild(i).gameObject.SetActive(false);

        list_RollSpawn[GameManager.Instance.selectSkin].SetActive(true);
        list_RollSpawn[GameManager.Instance.selectSkin].GetComponent<Animator>().speed = RollSpawner.Instance.animationSpeed;

        if (GameManager.Instance.selectSkin == 1)
            transform.GetChild(1).GetComponent<Renderer>().material.color = Random.ColorHSV();
    }
  
    void Update()
    {
        if (timer <= destroyTime)
            timer += Time.deltaTime;
        else
        {
            if (isSwipeSuccessful == true)
                HideRoll();
            else
            {
                HideRoll();
                ButtonController.Instance.GameResult();
            }
        }
    }

    public void HideRoll()
    {
        Pool.Release(gameObject);
        RollSpawner.Instance.pool_Queue.RemoveAt(0);

        if (GameManager.Instance.vibration == 1)
            Handheld.Vibrate();
    }
}