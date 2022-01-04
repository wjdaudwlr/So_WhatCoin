using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using Newtonsoft.Json;

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
    [SerializeField]
    private GameObject backPanel;

    private ulong typingSpeedUpgradeCost;
    private ulong typingSpeedUpgradeMoney;

    float automatcIncomeTime = 0;
    float saveTiem = 0;

    public static GameManager Instance;     // �̱���

    private void Awake()
    {
        player.LoadPlayerDataToJson();

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
        StartCoroutine(setData());

        DontDestroyOnLoad(this);
    }

    private void Update()
    {
        
        automatcIncomeTime += Time.deltaTime;
        if(automatcIncomeTime >= 1)
        {
            player.playerData.playerMoney += player.playerData.automatcIncome; 
            automatcIncomeTime = 0;
            saveTiem += 1;
        }
        if(saveTiem >= 60)
        {
            StartCoroutine(DataPostSave());
            saveTiem = 0;
            Debug.Log("Save");
        }

        playerMoneyText.text = Money.ToString(player.playerData.playerMoney);

        if(Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKey(KeyCode.Escape) && backPanel.activeSelf == false)
            {
                backPanel.SetActive(true);
            }
        }
    }

    private void InitTypingSpeedUpgrade()
    {
        typingSpeedUpgradeCost = 1000;
        typingSpeedUpgradeMoney = 10;

        typingSpeedLevelText.text = "Lvl " + player.playerData.typingSpeed;
        for(int i = 1; i< player.playerData.typingSpeed; i++)
        {
            typingSpeedUpgradeCost += typingSpeedUpgradeCost / 10;
            typingSpeedUpgradeMoney += 3;
        }
        typingSpeedUpgradeCostText.text = $"<size=32>" + string.Format("{0:n0}", typingSpeedUpgradeCost) + " ��</size>\n\n+ " + string.Format("{0:n0}", typingSpeedUpgradeMoney) + "GOLD";
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
                itemPurchaseButton[item.number].gameObject.GetComponentInChildren<Text>().text = "<size=50>���ſϷ�</size>";
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

        typingSpeedUpgradeCostText.text = $"<size=32>" + string.Format("{0:n0}", typingSpeedUpgradeCost) +  " ��</size>\n\n+ " + string.Format("{0:n0}", typingSpeedUpgradeMoney) +  "GOLD";
    }

    public void ItemPurchase(string itemName)   
    {
        Item item = itemMap[itemName];
        if (player.playerData.playerMoney < item.price) return;

        player.playerData.automatcIncome += item.automatcIncome;
        player.playerData.playerMoney -= item.price;

        item.gameObject.SetActive(true);
        itemPurchaseButton[item.number].interactable = false;
        itemPurchaseButton[item.number].gameObject.GetComponentInChildren<Text>().text = "<size=50>���ſϷ�</size>";
        player.playerData.itemDict[itemName] = true;
        ShakeCamera.Instance.OnShakeCamera(0.2f, 0.07f);
        InitSkill();
    }

    IEnumerator setData()
    {

        itemMap.Add("frog", items[0].GetComponent<Item>());
        itemMap.Add("monsta", items[1].GetComponent<Item>());
        itemMap.Add("doge", items[2].GetComponent<Item>());
        itemMap.Add("keyboard", items[3].GetComponent<Item>());
        itemMap.Add("statik", items[4].GetComponent<Item>());
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

    public void ContinueGame()
    {
        backPanel.SetActive(false);
    }
    

    public void Quit()
    {   
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();

        #endif
    }

    public void GameExit()
    {
        StartCoroutine(GameExitSave());
    }

    IEnumerator GameExitSave()
    {
        yield return StartCoroutine(DataPostSave());
        coinManager.coinMap.Clear();
        itemMap.Clear();
        Quit();
    }

    IEnumerator DataPostSave()
    {
        string url = "http://10.120.74.70:3001/auth/save";
        string jsonData = JsonConvert.SerializeObject(player.playerData);

        WWWForm form = new WWWForm();

        Debug.Log(jsonData);

        form.AddField("userdata", jsonData);

        UnityWebRequest www = UnityWebRequest.Post(url, form);

        Debug.Log("1");
        yield return www.SendWebRequest(); // ������ �ö����� ��ٸ���
        Debug.Log("1");

        if (www.isNetworkError)
        {
            Debug.Log(www.error);
        }
        else if (www.downloadHandler.text != "false")
        {
            Debug.Log("����");
        }
    }

    void OnApplicationQuit()
    {
        GameExit();
        coinManager.coinMap.Clear();
        itemMap.Clear();
    }

    public void Logout()
    {
        PlayerPrefs.SetString("playerEmail", null);
        PlayerPrefs.SetString("playerPassword", null);
        GameExit();
    }

}
