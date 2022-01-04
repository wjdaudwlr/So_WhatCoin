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

            coinQuantityText[coin.number].text = "보유 : " + string.Format("{0:n0}", GameManager.Instance.player.playerData.coinDict[coinkey]);
        }
    }

    public void Update()
    {

        if (socketClient.coinDatas.Count == 0) return;
        
        foreach (string coinkey in GameManager.Instance.player.playerData.coinDict.Keys)
        {
            coinMap[coinkey].price = (ulong)(socketClient.coinDatas[coinMap[coinkey].number].price);
            coinPriceTexts[coinMap[coinkey].number].text = string.Format("{0:n0}", coinMap[coinkey].price) + "원";
        }

        if (coinTransactionPanel.activeSelf)
            coinPriceText.text = "가격 : " + string.Format("{0:n0}", coinMap[currentCoinName].price) + "원";
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
        purchasedPriceText.text = "구매한 가격 : " + string.Format("{0:n0}", purchasedPrice[coin.number]);

        coinQuantityText[coin.number].text = "보유 : " + string.Format("{0:n0}", GameManager.Instance.player.playerData.coinDict[currentCoinName]);
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

        coinQuantityText[coin.number].text = "보유 : " + string.Format("{0:n0}", GameManager.Instance.player.playerData.coinDict[currentCoinName]);
        CoinPurchaseSaleText(coin.price * coinInput,new Color(151,255,0));

    }

    public void CoinAllSale()
    {
        Coin coin = coinMap[currentCoinName];

        int num = GameManager.Instance.player.playerData.coinDict[currentCoinName];
        Debug.Log(num);

        GameManager.Instance.player.playerData.coinDict[currentCoinName] -= (int)num;
        GameManager.Instance.player.playerData.playerMoney += coin.price * (ulong)num;

        curCoinQuantityText.text = "보유 : " + string.Format("{0:n0}", GameManager.Instance.player.playerData.coinDict[currentCoinName]);
        coinQuantityText[coin.number].text = "보유 : " + string.Format("{0:n0}", GameManager.Instance.player.playerData.coinDict[currentCoinName]);
        purchasedPriceText.text = "구매한 가격 : " + 0;
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
        purchasedPriceText.text = "구매한 가격 : " + string.Format("{0:n0}", purchasedPrice[coin.number]);

        curCoinQuantityText.text = "보유 : " + string.Format("{0:n0}", GameManager.Instance.player.playerData.coinDict[currentCoinName]);
        coinQuantityText[coin.number].text = "보유 : " + string.Format("{0:n0}", GameManager.Instance.player.playerData.coinDict[currentCoinName]);
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

        purchasedPriceText.text = "구매한 가격 : " + string.Format("{0:n0}", purchasedPrice[coin.number]);
        curCoinQuantityText.text = "보유 : " + string.Format("{0:n0}", GameManager.Instance.player.playerData.coinDict[currentCoinName]);
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
                coinNameText.text = "김동동 코인";
                break;
            case "whattodocoin":
                coinNameText.text = "어쩔코인 코인";
                break;
            case "gsmcoin":
                coinNameText.text = "GSM 코인";
                break;
            case "choigangmincoin":
                coinNameText.text = "초이강민 코인";
                break;
            case "gemgaejiyecoin":
                coinNameText.text = "겜개지예 코인";
                break;
            case "hyeonttungcoin":
                coinNameText.text = "현뚱 코인";
                break;
            case "ijuncoin":
                coinNameText.text = "이준 코인";
                break;
            case "eunseongcoin":
                coinNameText.text = "흑마법사 코인";
                break;
            case "jjunjjunacoin":
                coinNameText.text = "쭌쭈나 코인";
                break;
            case "sihuncoin":
                coinNameText.text = "시훈 코인";
                break;
            case "haembeogseungmincoin":
                coinNameText.text = "햄벅승민 코인";
                break;
            case "yusiopeucoin":
                coinNameText.text = "유시오프 코인";
                break;
            case "geonucoin":
                coinNameText.text = "건우 코인";
                break;
            case "manghaessseonghuncoin":
                coinNameText.text = "망했성훈 코인";
                break;
            case "chanwoocoin":
                coinNameText.text = "프란또찬우 코인";
                break;
            case "studentcouncilcoin":
                coinNameText.text = "학생회 코인";
                break;
            case "eunyoungcoin":
                coinNameText.text = "여신은영 코인";
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
