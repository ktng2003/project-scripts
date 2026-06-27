using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;

public class IAPManager : MonoBehaviour, IDetailedStoreListener
{
    private static IStoreController controller;
    private static IExtensionProvider extensions;

    private string removeAllAds = "";

    private static IAPManager instance;
    public static IAPManager Instance
    {
        get
        {
            if (instance == null)
            {
                var obj = FindObjectOfType<IAPManager>();

                if (obj != null)
                    instance = obj;
                else
                {
                    var newObj = new GameObject().AddComponent<IAPManager>();

                    instance = newObj;
                }
            }
            return instance;
        }
    }

    private void Awake()
    {
        var objs = FindObjectsOfType<IAPManager>();
        if (objs.Length != 1)
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        if (controller == null)
            InitializePurchasing();
    }

    public void InitializePurchasing()
    {
        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
    
        builder.AddProduct(removeAllAds, ProductType.NonConsumable);

        UnityPurchasing.Initialize(this, builder);
    }
    
    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        IAPManager.controller = controller;
        IAPManager.extensions = extensions;
    
        Product product = controller.products.WithID(removeAllAds);
        
        if (product != null && product.hasReceipt)
        {
            GameManager.Instance.isRemoveAllAds = true;
            BannerAdsManager.Instance.HideBanner();
        }
        else
        {
            RewardAdsManager.Instance.InitAds();
            BannerAdsManager.Instance.InitAds();
        }
    }
  
    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.Log("IAP 초기화 실패: " + error);
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        if (args.purchasedProduct.definition.id == removeAllAds)
        {
            GameManager.Instance.isRemoveAllAds = true;
            BannerAdsManager.Instance.HideBanner();
        }

        return PurchaseProcessingResult.Complete;
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        Debug.Log("결제 실패: " + failureReason);
    }

    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {
        Debug.Log($"IAP 초기화 실패: {error}, 메시지: {message}");
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureDescription failureDescription)
    {
        Debug.Log($"결제 실패: {failureDescription}");
    }

    public void PurchaseRemoveAllAds()
    {
        if (controller == null) return;

        Product product = controller.products.WithID(removeAllAds);
        if (product != null && product.availableToPurchase)
            controller.InitiatePurchase(product);
    }
}