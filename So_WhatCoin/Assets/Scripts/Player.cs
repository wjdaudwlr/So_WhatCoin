using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField]
    private Text playerMoneyText;
    [SerializeField]
    private Sprite[] laptopImages;

    private SpriteRenderer sptrieRenderer;
    private PlayerResize playerResize;

    public ulong playerMoney = 0;
    public ulong clickMoney = 10;
    private int currentLaptopSprite = 0;

    private void Awake()
    {
        playerResize = GetComponent<PlayerResize>();
        sptrieRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        sptrieRenderer.sprite = laptopImages[currentLaptopSprite];
    }

    public void ClickToPlayer()
    {
        if (playerResize.isclick)
            StartCoroutine(playerResize.ClickEffect());
        playerMoney += clickMoney;
        playerMoneyText.text = Money.ToString(playerMoney);
        currentLaptopSprite = (currentLaptopSprite % (laptopImages.Length - 1)) + 1;
        sptrieRenderer.sprite = laptopImages[currentLaptopSprite];
    }
}
