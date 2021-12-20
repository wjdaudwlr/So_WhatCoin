using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Player")]
    [SerializeField]
    private Text playerMoneyText;
    [SerializeField]
    private Player player;

    [Header("UPGRADE")]
    [SerializeField]
    private Text typingSpeedLevelText;
    [SerializeField]
    private Text typingSpeedUpgradeCostText;

    [Header("ITEM")]
    [SerializeField]
    private GameObject[] items;
    [SerializeField]
    private Button[] itemPurchaseButton;
    Dictionary<string, Item> itemMap;

    private ulong typingSpeedUpgradeCost;
    private ulong typingSpeedUpgradeMoney;

    float automatcIncomeTime = 0;

    public static GameManager Instance;     // 싱글톤

    private void Awake()
    {
        player.LoadPlayerDataToJson();
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

        typingSpeedUpgradeCostText.text = $"<size=32>" + string.Format("{0:n0}", typingSpeedUpgradeCost) + " 원</size>\n\n+ " + string.Format("{0:n0}", typingSpeedUpgradeMoney) + "GOLD";

        itemMap = new Dictionary<string, Item>();
        itemMap.Add("frog", items[0].GetComponent<Item>());
    }

    private void Update()
    {
        automatcIncomeTime += Time.deltaTime;
        if(automatcIncomeTime >= 1)
        {
            player.playerData.playerMoney += player.playerData.automatcIncome;
            automatcIncomeTime = 0;
        }
        playerMoneyText.text = Money.ToString(player.playerData.playerMoney);

    }

    public void TypingSpeedUpgrade()
    {
        if (player.playerData.playerMoney < typingSpeedUpgradeCost) return;

        player.playerData.playerMoney -= typingSpeedUpgradeCost;
        typingSpeedUpgradeCost += typingSpeedUpgradeCost / 10;

        player.playerData.clickMoney += typingSpeedUpgradeMoney;
        typingSpeedUpgradeMoney += 2;

        player.playerData.typingSpeedLevel += 1;
        typingSpeedLevelText.text = "Lvl " + player.playerData.typingSpeedLevel;

        typingSpeedUpgradeCostText.text = $"<size=32>" + string.Format("{0:n0}", typingSpeedUpgradeCost) +  " 원</size>\n\n+ " + string.Format("{0:n0}", typingSpeedUpgradeMoney) +  "GOLD";
    }

    public void ItemPurchase(string name)
    {
        Item item = itemMap[name];
        if (player.playerData.playerMoney < item.cost) return;
        player.playerData.automatcIncome += item.automatcIncome;
        player.playerData.playerMoney -= item.cost;
        item.gameObject.SetActive(true);
        itemPurchaseButton[item.number].interactable = false;
        itemPurchaseButton[item.number].gameObject.GetComponentInChildren<Text>().text = "<size=55>구매완료</size>";
    }


    void OnApplicationQuit()
    {
        /* 앱이 종료 될 때 처리 */
        player.SavePlayerDataToJson();
    }

}
