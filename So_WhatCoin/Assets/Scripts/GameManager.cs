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
    public Player player;

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
    public Dictionary<string, Item> itemMap = new Dictionary<string, Item>();
    [SerializeField]
    private GameObject[] skillImages;

    [SerializeField]
    private CoinManager coinManager;

    private ulong typingSpeedUpgradeCost;
    private ulong typingSpeedUpgradeMoney;

    float automatcIncomeTime = 0;

    public static GameManager Instance;     // 싱글톤

    private void Awake()
    {
        StartCoroutine(setData());

        if (Instance == null)
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

    private void InitTypingSpeedUpgrade()
    {
        typingSpeedUpgradeCost = 1000;
        typingSpeedUpgradeMoney = 15;

        typingSpeedLevelText.text = "Lvl " + player.playerData.typingSpeed;
        for(int i = 1; i< player.playerData.typingSpeed; i++)
        {
            typingSpeedUpgradeCost += typingSpeedUpgradeCost / 10;
            typingSpeedUpgradeMoney += 3;
        }
        typingSpeedUpgradeCostText.text = $"<size=32>" + string.Format("{0:n0}", typingSpeedUpgradeCost) + " 원</size>\n\n+ " + string.Format("{0:n0}", typingSpeedUpgradeMoney) + "GOLD";
    }

    private void InitItem()
    {
        foreach (string key in player.playerData.itemDict.Keys)
        {
            if (player.playerData.itemDict[key])
            {
                Debug.Log(key);
                Item item = itemMap[key];
                item.gameObject.SetActive(true);
                itemPurchaseButton[item.number].interactable = false;
                itemPurchaseButton[item.number].gameObject.GetComponentInChildren<Text>().text = "<size=50>구매완료</size>";
            }
        }
    }

    public void InitSkill()
    {
        if (player.playerData.itemDict["gay"])
            skillImages[0].SetActive(true);
        if (player.playerData.itemDict["speaker"])
            skillImages[1].SetActive(true);
    }


    public void TypingSpeedUpgrade()
    {
        if (player.playerData.playerMoney < typingSpeedUpgradeCost) return;

        player.playerData.playerMoney -= typingSpeedUpgradeCost;
        typingSpeedUpgradeCost += typingSpeedUpgradeCost / 13;

        player.playerData.clickMoney += typingSpeedUpgradeMoney;
        typingSpeedUpgradeMoney += 3;

        player.playerData.typingSpeed += 1;
        typingSpeedLevelText.text = "Lvl " + player.playerData.typingSpeed;

        typingSpeedUpgradeCostText.text = $"<size=32>" + string.Format("{0:n0}", typingSpeedUpgradeCost) +  " 원</size>\n\n+ " + string.Format("{0:n0}", typingSpeedUpgradeMoney) +  "GOLD";
    }

    public void ItemPurchase(string itemName)   
    {
        Item item = itemMap[itemName];
        if (player.playerData.playerMoney < item.price) return;

        player.playerData.automatcIncome += item.automatcIncome;
        player.playerData.playerMoney -= item.price;

        item.gameObject.SetActive(true);
        itemPurchaseButton[item.number].interactable = false;
        itemPurchaseButton[item.number].gameObject.GetComponentInChildren<Text>().text = "<size=50>구매완료</size>";
        player.playerData.itemDict[itemName] = true;
        ShakeCamera.Instance.OnShakeCamera(0.2f, 0.07f);
        InitSkill();
    }

    IEnumerator setData()
    {
        player.LoadPlayerDataToJson();

        itemMap.Add("frog", items[0].GetComponent<Item>());
        itemMap.Add("monsta", items[1].GetComponent<Item>());
        itemMap.Add("doge", items[2].GetComponent<Item>());
        itemMap.Add("keyboard", items[3].GetComponent<Item>());
        itemMap.Add("statikk", items[4].GetComponent<Item>());
        itemMap.Add("speaker", items[5].GetComponent<Item>());
        itemMap.Add("gay", items[6].GetComponent<Item>());
        itemMap.Add("ekko", items[7].GetComponent<Item>());
        itemMap.Add("gram", items[8].GetComponent<Item>());

        yield return new WaitForSeconds(0.2f);
        coinManager.InitCoin();
        InitItem();
        InitSkill();
        InitTypingSpeedUpgrade();
    }

    void OnApplicationQuit()
    {
        /* 앱이 종료 될 때 처리 */
        itemMap.Clear();
        player.SavePlayerDataToJson();
    }
}
