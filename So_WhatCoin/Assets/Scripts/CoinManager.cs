using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinManager : MonoBehaviour
{

    [Header("Coin")]
    [SerializeField]
    private Text[] coinQuantityText;
    [SerializeField]
    public Text[] coinPriceTexts;
    [SerializeField]
    private Text purchasedPriceText;
    [SerializeField]
    private GameObject coinTransactionPanel;
    [SerializeField]
    private Text coinNameText;
    [SerializeField]
    private Text coinPriceText;
    [SerializeField]
    Text curCoinQuantityText;
    [SerializeField]
    private GameObject clickMoneyText;
    private ClickMoneyText2 clickMoneyTextCom;
    [SerializeField]
    private Transform clickPos;

    ulong[] purchasedPrice = new ulong[17];

    public InputField coinInputField;

    public SocketClient socketClient;

    public Dictionary<string, Coin> coinMap = new Dictionary<string, Coin>();

    string currentCoinName;

    private void Awake()
    {
        coinMap.Add("kimdongdongcoin", new Coin("kimdongdongcoin", 99999999999, 0));
        coinMap.Add("whattodocoin", new Coin("whattodocoin", 99999999999, 1));
        coinMap.Add("gsmcoin", new Coin("gsmcoin", 99999999999, 2));
        coinMap.Add("choigangmincoin", new Coin("choigangmincoin", 99999999999, 3));
        coinMap.Add("gemgaejiyecoin", new Coin("gemgaejiyecoin", 99999999999, 4));
        coinMap.Add("hyeonttungcoin", new Coin("hyeonttungcoin", 99999999999, 5));
        coinMap.Add("ijuncoin", new Coin("ijuncoin", 99999999999, 6));
        coinMap.Add("eunseongcoin", new Coin("eunseongcoin", 99999999999, 7));
        coinMap.Add("jjunjjunacoin", new Coin("jjunjjunacoin", 99999999999, 8));
        coinMap.Add("sihuncoin", new Coin("sihuncoin", 99999999999, 9));
        coinMap.Add("haembeogseungmincoin", new Coin("haembeogseungmincoin", 99999999999, 10));
        coinMap.Add("yusiopeucoin", new Coin("yusiopeucoin", 99999999999, 11));
        coinMap.Add("geonucoin", new Coin("geonucoin", 99999999999, 12));
        coinMap.Add("manghaessseonghuncoin", new Coin("manghaessseonghuncoin", 99999999999, 13));
        coinMap.Add("chanwoocoin", new Coin("chanwoocoin", 99999999999, 14));
        coinMap.Add("studentcouncilcoin", new Coin("studentcouncilcoin", 99999999999, 15));
        coinMap.Add("eunyoungcoin", new Coin("eunyoungcoin", 99999999999, 16));
    }



    public void InitCoin()
    {
        foreach (string coinkey in GameManager.Instance.player.playerData.coinDict.Keys)
        {
            Coin coin = coinMap[coinkey];

            coinQuantityText[coin.number].text = "���� : " + string.Format("{0:n0}", GameManager.Instance.player.playerData.coinDict[coinkey]);
        }
    }

    public void Update()
    {

        if (socketClient.coinDatas.Count == 0) return;
        
        foreach (string coinkey in GameManager.Instance.player.playerData.coinDict.Keys)
        {
            coinMap[coinkey].price = (ulong)(socketClient.coinDatas[coinMap[coinkey].number].price);
            coinPriceTexts[coinMap[coinkey].number].text = string.Format("{0:n0}", coinMap[coinkey].price) + "��";
        }

        if (coinTransactionPanel.activeSelf)
            coinPriceText.text = "���� : " + string.Format("{0:n0}", coinMap[currentCoinName].price) + "��";
    }

    public void CoinPurchase()
    {
        Debug.Log(currentCoinName);
        int result = 0;
        if (!int.TryParse(coinInputField.text, out result) || int.Parse(coinInputField.text) <= 0) {
            Debug.Log(1);
            return;
        };

        uint coinInput = uint.Parse(coinInputField.text);

        Coin coin = coinMap[currentCoinName];
        if ( GameManager.Instance.player.playerData.playerMoney < coin.price * coinInput) return;

        GameManager.Instance.player.playerData.coinDict[currentCoinName] += (int)coinInput;
        GameManager.Instance.player.playerData.playerMoney -= coin.price * coinInput;

        purchasedPrice[coin.number] = coin.price;
        purchasedPriceText.text = "������ ���� : " + string.Format("{0:n0}", purchasedPrice[coin.number]);

        coinQuantityText[coin.number].text = "���� : " + string.Format("{0:n0}", GameManager.Instance.player.playerData.coinDict[currentCoinName]);
        CoinPurchaseSaleText(coin.price * coinInput, new Color(255,0,0));
    }

    public void CoinSale()
    {
        Debug.Log(currentCoinName);
        int result = 0;
        if (!int.TryParse(coinInputField.text, out result) || int.Parse(coinInputField.text) <= 0) return;

        uint coinInput = uint.Parse(coinInputField.text);

        Coin coin = coinMap[currentCoinName];

        if (GameManager.Instance.player.playerData.coinDict[currentCoinName] < coinInput) return;

        GameManager.Instance.player.playerData.coinDict[currentCoinName] -= (int)coinInput;
        GameManager.Instance.player.playerData.playerMoney += coin.price * coinInput;

        coinQuantityText[coin.number].text = "���� : " + string.Format("{0:n0}", GameManager.Instance.player.playerData.coinDict[currentCoinName]);
        CoinPurchaseSaleText(coin.price * coinInput,new Color(151,255,0));

    }

    public void CoinAllSale()
    {
        Coin coin = coinMap[currentCoinName];

        int num = GameManager.Instance.player.playerData.coinDict[currentCoinName];
        Debug.Log(num);

        GameManager.Instance.player.playerData.coinDict[currentCoinName] -= (int)num;
        GameManager.Instance.player.playerData.playerMoney += coin.price * (ulong)num;

        curCoinQuantityText.text = "���� : " + string.Format("{0:n0}", GameManager.Instance.player.playerData.coinDict[currentCoinName]);
        coinQuantityText[coin.number].text = "���� : " + string.Format("{0:n0}", GameManager.Instance.player.playerData.coinDict[currentCoinName]);
        purchasedPriceText.text = "������ ���� : " + 0;
        CoinPurchaseSaleText(coin.price * (ulong)num, new Color(151, 255, 0));
    }

    public void CoinAllPurchase()
    {
        Coin coin = coinMap[currentCoinName];

        ulong num = GameManager.Instance.player.playerData.playerMoney / coin.price;
        Debug.Log(num);

        GameManager.Instance.player.playerData.coinDict[currentCoinName] += (int)num;
        GameManager.Instance.player.playerData.playerMoney -= coin.price * num;

        purchasedPrice[coin.number] = coin.price;
        purchasedPriceText.text = "������ ���� : " + string.Format("{0:n0}", purchasedPrice[coin.number]);

        curCoinQuantityText.text = "���� : " + string.Format("{0:n0}", GameManager.Instance.player.playerData.coinDict[currentCoinName]);
        coinQuantityText[coin.number].text = "���� : " + string.Format("{0:n0}", GameManager.Instance.player.playerData.coinDict[currentCoinName]);
        CoinPurchaseSaleText(coin.price * num,new Color(255,0,0));

    }


    public void CoinTransactionPanelOnOff(string coinName)
    {
        if (socketClient.coinDatas.Count == 0) return;

        if (coinTransactionPanel.activeSelf) coinTransactionPanel.SetActive(false);
        else coinTransactionPanel.SetActive(true);

        if (coinName == null) return;
        currentCoinName = coinName;

        Coin coin = coinMap[coinName];

        purchasedPriceText.text = "������ ���� : " + string.Format("{0:n0}", purchasedPrice[coin.number]);
        curCoinQuantityText.text = "���� : " + string.Format("{0:n0}", GameManager.Instance.player.playerData.coinDict[currentCoinName]);
        CoinNameTranslation(coinName);
    }

    public void CoinTransactionPanelOff()
    {
        coinTransactionPanel.SetActive(false);
    }

    void CoinNameTranslation(string coinName)
    {
        switch (coinName)
        {
            case "kimdongdongcoin":
                coinNameText.text = "�赿�� ����";
                break;
            case "whattodocoin":
                coinNameText.text = "��¿���� ����";
                break;
            case "gsmcoin":
                coinNameText.text = "GSM ����";
                break;
            case "choigangmincoin":
                coinNameText.text = "���̰��� ����";
                break;
            case "gemgaejiyecoin":
                coinNameText.text = "�װ����� ����";
                break;
            case "hyeonttungcoin":
                coinNameText.text = "���� ����";
                break;
            case "ijuncoin":
                coinNameText.text = "���� ����";
                break;
            case "eunseongcoin":
                coinNameText.text = "�渶���� ����";
                break;
            case "jjunjjunacoin":
                coinNameText.text = "���޳� ����";
                break;
            case "sihuncoin":
                coinNameText.text = "���� ����";
                break;
            case "haembeogseungmincoin":
                coinNameText.text = "�ܹ��¹� ����";
                break;
            case "yusiopeucoin":
                coinNameText.text = "���ÿ��� ����";
                break;
            case "geonucoin":
                coinNameText.text = "�ǿ� ����";
                break;
            case "manghaessseonghuncoin":
                coinNameText.text = "���߼��� ����";
                break;
            case "chanwoocoin":
                coinNameText.text = "���������� ����";
                break;
            case "studentcouncilcoin":
                coinNameText.text = "�л�ȸ ����";
                break;
            case "eunyoungcoin":
                coinNameText.text = "�������� ����";
                break;
        }
    }

    void CoinPurchaseSaleText(ulong text,Color color)
    {
        GameObject moneyText = Instantiate(clickMoneyText);
        moneyText.transform.position = clickPos.position;
        clickMoneyTextCom = moneyText.GetComponent<ClickMoneyText2>();
        clickMoneyTextCom.SetUp();
        clickMoneyTextCom.text.color = color;
        clickMoneyTextCom.money = text;
    }

}
