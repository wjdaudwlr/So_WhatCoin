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
    private GameObject coinTransactionPanel;

    public InputField coinInputField;

    Dictionary<string, Coin> coinMap = new Dictionary<string, Coin>();

    string currentCoinName;

    private void Awake()
    {
        coinMap.Add("gsmcoin", new Coin("gsmcoin", 5000000, 0));
        coinMap.Add("kimdongdong", new Coin("kimdongdong", 5000000, 0));
        coinMap.Add("whattodocoin", new Coin("whattodocoin", 5000000, 0));
    }



    public void InitCoin()
    {
        foreach (string coinkey in GameManager.Instance.player.playerData.coinDict.Keys)
        {
            Coin coin = coinMap[coinkey];

            coinQuantityText[coin.number].text = "���� : " + string.Format("{0:n0}", GameManager.Instance.player.playerData.coinDict[coinkey]);
        }
    }

    public void CoinPurchase()
    {
        int result = 0;
        if (!int.TryParse(coinInputField.text, out result) || int.Parse(coinInputField.text) <= 0) return;

        uint coinInput = uint.Parse(coinInputField.text);

        Coin coin = coinMap[currentCoinName];
        if ( GameManager.Instance.player.playerData.playerMoney < coin.price * coinInput) return;

        GameManager.Instance.player.playerData.coinDict[currentCoinName] += (int)coinInput;
        GameManager.Instance.player.playerData.playerMoney -= coin.price * coinInput;

        coinQuantityText[coin.number].text = "���� : " + string.Format("{0:n0}", GameManager.Instance.player.playerData.coinDict[currentCoinName]);
    }

    public void CoinSale()
    {
        int result = 0;
        if (!int.TryParse(coinInputField.text, out result) || int.Parse(coinInputField.text) <= 0) return;

        uint coinInput = uint.Parse(coinInputField.text);

        Coin coin = coinMap[currentCoinName];

        if (GameManager.Instance.player.playerData.coinDict[currentCoinName] < coinInput) return;

        GameManager.Instance.player.playerData.coinDict[currentCoinName] -= (int)coinInput;
        GameManager.Instance.player.playerData.playerMoney += coin.price * coinInput;

        coinQuantityText[coin.number].text = "���� : " + string.Format("{0:n0}", GameManager.Instance.player.playerData.coinDict[currentCoinName]);
    }

    public void CoinTransactionPanelOnOff(string coinName)
    {
        if (coinTransactionPanel.activeSelf) coinTransactionPanel.SetActive(false);
        currentCoinName = coinName;
        coinTransactionPanel.SetActive(true);
    }


}
