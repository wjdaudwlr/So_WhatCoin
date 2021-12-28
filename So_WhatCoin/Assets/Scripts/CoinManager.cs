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

            coinQuantityText[coin.number].text = "x " + GameManager.Instance.player.playerData.coinDict[coinkey].ToString();
        }
    }

    public void CoinPurchase(string coinName)
    {
        Coin coin = coinMap[coinName];
        if (GameManager.Instance.player.playerData.playerMoney < coin.price) return;

        GameManager.Instance.player.playerData.coinDict[coinName] += int.Parse(coinInputField.text);
        GameManager.Instance.player.playerData.playerMoney -= coin.price * uint.Parse(coinInputField.text);

        coinQuantityText[coin.number].text ="x " + GameManager.Instance.player.playerData.coinDict[coinName].ToString();
    }

    public void CoinSale(string coinName)
    {
        Coin coin = coinMap[coinName];
        if (GameManager.Instance.player.playerData.coinDict[coinName] < 1) return;

        GameManager.Instance.player.playerData.coinDict[coinName] -= int.Parse(coinInputField.text);
        GameManager.Instance.player.playerData.playerMoney += coin.price * uint.Parse(coinInputField.text);

        coinQuantityText[coin.number].text = "x " + GameManager.Instance.player.playerData.coinDict[coinName].ToString();
    }

}
