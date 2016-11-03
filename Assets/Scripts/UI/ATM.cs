using UnityEngine;
using System.Collections;

public class ATM : MonoBehaviour
{
    public GameObject InstructionObject;
    public GameObject MoneyDisplay;
    public float Range;
    private bool Active;
    private static int CurrentMoney;
    public int MaxMoney;
    private TextMesh displayTextMesh;

    void Start()
    {
        displayTextMesh = MoneyDisplay.GetComponent<TextMesh>();
    }

    void Update()
    {
        displayTextMesh.text = CurrentMoney + " / " + MaxMoney;

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

        if (Input.GetAxis("Vertical") > 0 && Active)
        {
            int FreeSpace = MaxMoney - CurrentMoney;
            if (Player.money <= FreeSpace)
            {
                CurrentMoney += Player.money;
                Player.money = 0;
            }
            else
            {
                Player.money -= FreeSpace;
                CurrentMoney += FreeSpace;
            }
        }
        if (Input.GetAxis("Vertical") < 0 && Active)
        {
            Player.money += CurrentMoney;
            CurrentMoney = 0;
        }

    }
}
