using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public string coinName;             // �̸�
    public ulong price;              // ����
    public int number;

    public Coin(string _coinName, ulong _price, int _number)
    {
        this.coinName = _coinName;
        this.price = _price;
        this.number = _number;
    }

}
