using UnityEngine;
using UnityEngine.UI;

public class WeaponBuyUI : MonoBehaviour
{
    //replace gameobject with the weapon
    public Item[] weapons = new Item[1];
	public GameObject[] WeapnDescription = new GameObject[1];
    public int[] Prices = new int[1];

    public void ClickedWeapon(int WeaponID)
    {
        foreach (GameObject i in WeapnDescription)
        {
            i.SetActive(false);
        }
            WeapnDescription[WeaponID].SetActive(true);
    }

    public void ClickedBuy(int WeaponID)
    {
        if (Player.money >= Prices[WeaponID])
        {
            Player.Instance.SetItem(weapons[WeaponID]);
            WeapnDescription[WeaponID].GetComponentInChildren<Button>().interactable = false;
            Player.money -= Prices[WeaponID];
        }
    }
}
