using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class IAPItem : MonoBehaviour
{
    public Button button;
    public TextMeshProUGUI textMeshPro;

    public void Setup(string iapCode)
    {
        ShopProductNames currentProduct = (ShopProductNames)System.Enum.Parse(typeof(ShopProductNames), iapCode);
        textMeshPro.text = IAPManager.Instance.GetLocalizedPriceString(currentProduct);
        Debug.Log("Price : " + IAPManager.Instance.GetLocalizedPriceString(currentProduct));
    }
}
