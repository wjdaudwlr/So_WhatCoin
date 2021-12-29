using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinManager : MonoBehaviour
{

    [Header("Coin")]
    [SerializeField]
    private Text[] coinQuantityText;

    public InputField coinInputField;

    Dictionary<string, Coin> coinMap = new Dictionary<string, Coin>();

    private void Awake()
    {
        coinMap.Add("GSMCoin", new Coin("GSMCoin", 5000000, 0));
    }

    public void InitCoin()
    {
        foreach (string coinkey in GameManager.Instance.player.playerData.coinDict.Keys)
        {
            Coin coin = coinMap[coinkey];

            coinQuantityText[coin.number].text = "보유 : " + string.Format("{0:n0}", GameManager.Instance.player.playerData.coinDict[coinkey]);
        }
    }

    public void CoinPurchase(string coinName)
    {
        int result = 0;
        if (!int.TryParse(coinInputField.text, out result) || int.Parse(coinInputField.text) <= 0) return;

        uint coinInput = uint.Parse(coinInputField.text);

        Coin coin = coinMap[coinName];
        if ( GameManager.Instance.player.playerData.playerMoney < coin.price * coinInput) return;

        GameManager.Instance.player.playerData.coinDict[coinName] += (int)coinInput;
        GameManager.Instance.player.playerData.playerMoney -= coin.price * coinInput;

        coinQuantityText[coin.number].text = "보유 : " + string.Format("{0:n0}", GameManager.Instance.player.playerData.coinDict[coinName]);
    }

    public void CoinSale(string coinName)
    {
        int result = 0;
        if (!int.TryParse(coinInputField.text, out result) || int.Parse(coinInputField.text) <= 0) return;

        uint coinInput = uint.Parse(coinInputField.text);

        Coin coin = coinMap[coinName];

        if (GameManager.Instance.player.playerData.coinDict[coinName] < coinInput) return;

        GameManager.Instance.player.playerData.coinDict[coinName] -= (int)coinInput;
        GameManager.Instance.player.playerData.playerMoney += coin.price * coinInput;

        coinQuantityText[coin.number].text = "보유 : " + string.Format("{0:n0}", GameManager.Instance.player.playerData.coinDict[coinName]);
    }

}
