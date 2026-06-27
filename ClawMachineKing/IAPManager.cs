using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

public class IAPManager : MonoBehaviour
{
    private string playcount100 = "";
    private string playcount220 = "";
    private string playcount600 = "";

    public void OnPurchaseComplate(Product _product)
    {
        if (_product.definition.id == playcount100)
            GameManager.Instance.countPlay += 100;
        else if (_product.definition.id == playcount220)
            GameManager.Instance.countPlay += 220;
        else if (_product.definition.id == playcount600)
            GameManager.Instance.countPlay += 600;

        Shop.Instance.ButButtonState();
        GameManager.Instance.GameCountPlay();
        GameManager.Instance.Save();
    }

    public void BuyFail()
    {
        Debug.Log("구매 실패");
    }
}