using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public ulong playerMoney;
    public ulong clickMoney = 1;
    public int typingSpeedLevel = 1;


    private void Awake()
    {
        clickMoney = 30;
    }

    private void Start()
    {
        
    }

    
}
