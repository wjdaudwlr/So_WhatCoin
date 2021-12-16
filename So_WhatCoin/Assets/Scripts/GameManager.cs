using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Player player;
    [SerializeField]
    private Text playerMoneyText;

    public static GameManager Instance;     // ΩÃ±€≈Ê

    private void Awake()
    {
        if(Instance == null)
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
        playerMoneyText.text = Money.ToString(player.playerMoney);
    }



}
