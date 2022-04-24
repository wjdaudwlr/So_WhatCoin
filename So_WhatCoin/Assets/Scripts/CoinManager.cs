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

    [Header("Sound")]
    [SerializeField]
    private AudioClip coinClip;
    private void Awake()
    {
        coinMap.Add("onegradecoin", new Coin("onegradecoin", 99999999999, 0));
        coinMap.Add("twogradecoin", new Coin("twogradecoin", 99999999999, 1));
        coinMap.Add("threegradecoin", new Coin("threegradecoin", 99999999999, 2));
        coinMap.Add("gameclubcoin", new Coin("gameclubcoin", 99999999999, 3));
        coinMap.Add("cloudclubcoin", new Coin("cloudclubcoin", 99999999999, 4));
        coinMap.Add("securityclubcoin", new Coin("securityclubcoin", 99999999999, 5));
        coinMap.Add("roboticsclubcoin", new Coin("roboticsclubcoin", 99999999999, 6));
        coinMap.Add("networkclubcoin", new Coin("networkclubcoin", 99999999999, 7));
        coinMap.Add("healthcoin", new Coin("healthcoin", 99999999999, 8));
        coinMap.Add("gsmcoin", new Coin("gsmcoin", 99999999999, 9));
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

        SoundManager.instance.SFXPlay("coin", coinClip);
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

        SoundManager.instance.SFXPlay("coin", coinClip);
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
            case "onegradecoin":
                coinNameText.text = "1학년 코인";
                break;
            case "twogradecoin":
                coinNameText.text = "2학년 코인";
                break;
            case "threegradecoin":
                coinNameText.text = "3학년 코인";
                break;
            case "gameclubcoin":
                coinNameText.text = "게임개발 코인";
                break;
            case "cloudclubcoin":
                coinNameText.text = "클라우드 코인";
                break;
            case "securityclubcoin":
                coinNameText.text = "정보보안 코인";
                break;
            case "roboticsclubcoin":
                coinNameText.text = "로보틱스 코인";
                break;
            case "networkclubcoin":
                coinNameText.text = "네트워크 코인";
                break;
            case "healthcoin":
                coinNameText.text = "체력단련 코인";
                break;
            case "gsmcoin":
                coinNameText.text = "GSM 코인";
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
