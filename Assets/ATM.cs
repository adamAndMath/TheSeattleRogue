using UnityEngine;
using System.Collections;

public class ATM : MonoBehaviour
{
    public GameObject InstructionObject;
    public float Range;
    private bool Active;
    private int CurrentMoney;
    public int MaxMoney;

    void Update()
    {
        if (Vector2.Distance(Player.Instance.transform.position, transform.position) <= Range)
        {
            Active = true;
            InstructionObject.SetActive(true);
        }
        else
        {
            Active = false;
            InstructionObject.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.W) && Active)
        {
            int FreeSpace = MaxMoney - CurrentMoney;
            if (Player.Instance.money <= FreeSpace)
            {
                CurrentMoney += Player.Instance.money;
                Player.Instance.money = 0;
            }
            else
            {
                Player.Instance.money -= FreeSpace;
                CurrentMoney += FreeSpace;
            }
        }
        if (Input.GetKeyDown(KeyCode.D) && Active)
        {
            Player.Instance.money += CurrentMoney;
            CurrentMoney = 0;
        }

    }
}
