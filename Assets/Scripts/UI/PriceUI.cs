using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PriceUI : MonoBehaviour
{
    public GameObject UIShopManager;
    public int WeaponID;

    void Start()
    {
        GetComponent<Text>().text = "      " + UIShopManager.GetComponent<WeaponBuyUI>().Prices[WeaponID];
    }
}
