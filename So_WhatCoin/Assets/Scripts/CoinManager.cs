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

    ulong[] purchasedPrice = new ulong[14];

    public InputField coinInputField;

    public SocketClient socketClient;

    public Dictionary<string, Coin> coinMap = new Dictionary<string, Coin>();

    string currentCoinName;

    private void Awake()
    {
        coinMap.Add("kimdongdong", new Coin("kimdongdong", 5000000, 0));
        coinMap.Add("whattodocoin", new Coin("whattodocoin", 5000000, 1));
        coinMap.Add("gsmcoin", new Coin("gsmcoin", 5000000, 2));
        coinMap.Add("choigangmincoin", new Coin("choigangmincoin", 5000000, 3));
        coinMap.Add("gemgaejiyecoin", new Coin("gemgaejiyecoin", 5000000, 4));
        coinMap.Add("hyeonttungcoin", new Coin("hyeonttungcoin", 5000000, 5));
        coinMap.Add("ijuncoin", new Coin("ijuncoin", 5000000, 6));
        coinMap.Add("eunseongcoin", new Coin("eunseongcoin", 5000000, 7));
        coinMap.Add("jjunjjunacoin", new Coin("jjunjjunacoin", 5000000, 8));
        coinMap.Add("sihuncoin", new Coin("sihuncoin", 5000000, 9));
        coinMap.Add("haembeogseungmincoin", new Coin("haembeogseungmincoin", 5000000, 10));
        coinMap.Add("yusiopeucoin", new Coin("yusiopeucoin", 5000000, 11));
        coinMap.Add("geonucoin", new Coin("geonucoin", 5000000, 12));
        coinMap.Add("manghaessseonghuncoin", new Coin("manghaessseonghuncoin", 5000000, 13));
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
    }

    public void CoinTransactionPanelOnOff(string coinName)
    {
        if (coinTransactionPanel.activeSelf) coinTransactionPanel.SetActive(false);
        else coinTransactionPanel.SetActive(true);
        currentCoinName = coinName;

        Coin coin = coinMap[coinName];

        purchasedPriceText.text = "구매한 가격 : " + string.Format("{0:n0}", purchasedPrice[coin.number]);
    }


}
