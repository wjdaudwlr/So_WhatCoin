using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    
    [SerializeField]
    private Player player;
    [SerializeField]
    private Text playerMoneyText;
    [SerializeField]
    private Text typingSpeedLevelText;
    [SerializeField]
    private Text typingSpeedUpgradeCostText;

    public static GameManager Instance;     // ½Ì±ÛÅæ

    private ulong typingSpeedUpgradeCost;
    private ulong typingSpeedUpgradeMoney;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }


    private void Start()
    {
        DontDestroyOnLoad(this);
        typingSpeedUpgradeCost = 1000;
        typingSpeedUpgradeMoney = 15;

        typingSpeedUpgradeCostText.text = $"<size=32>" + string.Format("{0:n0}", typingSpeedUpgradeCost) + " ¿ø</size>\n\n+ " + string.Format("{0:n0}", typingSpeedUpgradeMoney) + "GOLD";
    }

    private void Update()
    {
        playerMoneyText.text = Money.ToString(player.playerMoney);
    }

    public void TypingSpeedUpgrade()
    {
        if (player.playerMoney < typingSpeedUpgradeCost) return;

        player.playerMoney -= typingSpeedUpgradeCost;
        typingSpeedUpgradeCost += typingSpeedUpgradeCost / 10;

        player.clickMoney += typingSpeedUpgradeMoney;
        typingSpeedUpgradeMoney += typingSpeedUpgradeMoney / 15;

        player.typingSpeedLevel += 1;
        typingSpeedLevelText.text = "Lvl " + player.typingSpeedLevel;

        typingSpeedUpgradeCostText.text = $"<size=32>" + string.Format("{0:n0}", typingSpeedUpgradeCost) +  " ¿ø</size>\n\n+ " + string.Format("{0:n0}", typingSpeedUpgradeMoney) +  "GOLD";
    }

}
