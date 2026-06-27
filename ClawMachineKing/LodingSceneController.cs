using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using System;
using System.Text.RegularExpressions;
using System.Xml;

public class LodingSceneController : MonoBehaviour
{
    [SerializeField]
    Image progressBar;

    static string nextScene;

    void Start()
    {
        StartCoroutine("LoadSceneProcess");
    }

    public static void LoadScene(string _sceneName)
    {
        nextScene = _sceneName;
        SceneManager.LoadScene("GameScene");
    }

    IEnumerator LoadSceneProcess()
    {
        AsyncOperation op = SceneManager.LoadSceneAsync("GameScene");
        op.allowSceneActivation = false;

        float timer = 0f;
        while (!op.isDone)
        {
            yield return null;

            if (op.progress < 0.9f)
                progressBar.fillAmount = op.progress;
            else
            {
                timer += Time.unscaledDeltaTime;
                progressBar.fillAmount = Mathf.Lerp(0.9f, 1.0f, timer);

                if (progressBar.fillAmount >= 1.0f)
                {
                    op.allowSceneActivation = true;
                    yield break;
                }
            }
        }
    }
}