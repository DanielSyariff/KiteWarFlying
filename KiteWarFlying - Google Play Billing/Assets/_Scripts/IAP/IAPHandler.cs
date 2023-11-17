using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IAPHandler : MonoBehaviour
{
    public List<IAPItemStruct> iapButtonList;
    public IAPItem iapItem;
    public Transform iapParent;
    // Start is called before the first frame update
    void Start()
    {
        IAPManager.Instance.InitializeIAPManager(OnInitComplete);
    }

    public void OnInitComplete(IAPOperationStatus status, string message, List<StoreProduct> shopProducts)
    {
        if (status == IAPOperationStatus.Success)
        {
            //Start Show
            ShowIAPButton();
        }
        else
        {
            Debug.Log("IAP ERROR");
            //Show Error
        }
    }

    public void ShowIAPButton()
    {
        foreach (IAPItemStruct iapButton in iapButtonList)
        {
            IAPItem iAPItem = Instantiate(iapItem, iapParent);
            iapItem.Setup(iapButton.IAPCode);
            iapItem.button.onClick.AddListener(delegate { BuyItem(iapButton.IAPCode); });
        }
    }

    public void BuyItem(string iapCode)
    {
        //Buy Item
        Debug.Log("Buying");
    }
}

[System.Serializable]
public struct IAPItemStruct
{
    public string IAPCode;
}
