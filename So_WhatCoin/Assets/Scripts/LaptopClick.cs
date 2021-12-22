using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LaptopClick : MonoBehaviour
{
    private Vector3 startSize;              // ���� ũ�� (transform.localScale)
    [SerializeField]
    private Vector3 endSize;                // ���� ũ��
    [SerializeField]
    private float resizeTime;               // ũ�� ��ȭ�� �ҿ�Ǵ� �ð�
    [SerializeField]
    private Sprite[] laptopImages;          // �ٲ�� ��Ʈ�� �̹���
    [SerializeField]
    private GameObject clickMoneyText;
    [SerializeField]
    private Transform clickPos;
    [SerializeField]
    private GameObject clickEffect;

    [Header("Sound")]
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip[] keyboardSound;

    private SpriteRenderer sptrieRenderer;

    public bool isclick = true;

    private int currentLaptopSprite = 0;    // ���� ��Ʈ�� �̹���

    private float soundTime= 0;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        sptrieRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        sptrieRenderer.sprite = laptopImages[currentLaptopSprite];
    }


    private void Update()
    {
        if (isclick)
        {
            soundTime += Time.deltaTime;
            if (soundTime > 1)
                audioSource.Stop();
        }
        else
            soundTime = 0;
    }

    public void ClickToPlayer()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.clip = keyboardSound[Random.Range(0, 3)];
            audioSource.Play();
        }

        if (isclick)
            StartCoroutine(ClickEffect());
        GameManager.Instance.player.playerData.playerMoney += GameManager.Instance.player.playerData.clickMoney;
        currentLaptopSprite = (currentLaptopSprite % (laptopImages.Length));
        sptrieRenderer.sprite = laptopImages[currentLaptopSprite++];


        if (GameManager.Instance.player.playerData.itemDict["doge"])
        {
            GameObject moneyText = Instantiate(clickMoneyText);
            moneyText.transform.position = clickPos.position + new Vector3(Random.Range(-1.1f, 1.1f), Random.Range(-0.2f, 0.2f), 0);
            ClickMoneyText clickMoneyTextCom = moneyText.GetComponent<ClickMoneyText>();
            clickMoneyTextCom.SetUp();

            if (GameManager.Instance.player.playerData.itemDict["statikk"])
            {
                if(Random.Range(0,100) < 7)
                {
                    clickMoneyTextCom.text.color = new Color(255, 0, 0);
                    clickMoneyTextCom.text.fontSize = 4f;
                    clickMoneyTextCom.money = GameManager.Instance.player.playerData.clickMoney * 2;
                    GameManager.Instance.player.playerData.playerMoney += GameManager.Instance.player.playerData.clickMoney;
                }
                else
                    clickMoneyTextCom.money = GameManager.Instance.player.playerData.clickMoney;
            }
            else
                clickMoneyTextCom.money = GameManager.Instance.player.playerData.clickMoney;
        }
        if (GameManager.Instance.player.playerData.itemDict["keyboard"])
        {
            Instantiate(clickEffect, Camera.main.ScreenToWorldPoint(Input.mousePosition) + new Vector3(0, 0, 0.5f), Quaternion.identity);
        }
    }

    private IEnumerator ClickEffect()
    {
        yield return isclick = false;
        startSize = transform.localScale;
        
         // resizeTime �ð� ���� start���� end�� ũ�� ��ȭ
         yield return StartCoroutine(Resize(startSize, endSize, resizeTime));

         // resizeTime �ð� ���� end���� start�� ũ�� ��ȭ
         yield return StartCoroutine(Resize(endSize, startSize, resizeTime));
         yield return isclick = true;
    }


    private IEnumerator Resize(Vector3 start, Vector3 end, float time)
    {
        float current = 0;
        float percent = 0;

        while (percent < 1)
        {
            current += Time.deltaTime;
            percent = current / time;

            transform.localScale = Vector3.Lerp(start, end, percent);

            yield return null;
        }
    }
}
