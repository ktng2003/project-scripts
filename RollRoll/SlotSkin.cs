using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SlotSkin : MonoBehaviour
{
    public GameObject Select;
    public GameObject block;

    public void SelectSkin()
    {
        Shop.Instance.parentSkin.transform.GetChild(GameManager.Instance.selectSkin + 1).GetChild(3).gameObject.SetActive(false);

        Select.SetActive(true);
        GameManager.Instance.selectSkin = int.Parse(gameObject.name);
    }

    public void BuySkin()
    {
        if (GameManager.Instance.coin >= 200)
        {
            GameManager.Instance.coin -= 200;

            GameManager.Instance.tCoin.text = GameManager.Instance.coin.ToString();
            GameManager.Instance.getCoin.transform.GetChild(1).GetComponent<Text>().text = "- 200";
            GameManager.Instance.StartGetCoinSequence();
            block.SetActive(false);

            GameManager.Instance.is_Skin[int.Parse(gameObject.name)] = true;

            GameManager.Instance.Save();
        }
    }
}