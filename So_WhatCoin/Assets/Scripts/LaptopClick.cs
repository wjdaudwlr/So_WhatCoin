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
    [SerializeField]
    private GameObject heeManclickEffect;
    

    [Header("Sound")]
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip[] keyboardSound;

    private SpriteRenderer sptrieRenderer;

    ClickMoneyText clickMoneyTextCom;

    bool isclick = true;
    public bool isHeeManSkill = false;
    public bool isSkill = false;

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
            if (soundTime > 0.5)
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
            clickMoneyTextCom = moneyText.GetComponent<ClickMoneyText>();
            clickMoneyTextCom.SetUp();

            clickMoneyTextCom.money = GameManager.Instance.player.playerData.clickMoney;

            if (isHeeManSkill)
            {
                Critical(100);
                sptrieRenderer.sprite = laptopImages[0];
            }
            else if (GameManager.Instance.player.playerData.itemDict["statikk"])
            {
                Critical(7);
            }
        }
        if (GameManager.Instance.player.playerData.itemDict["keyboard"])
        {
            Instantiate(isHeeManSkill ? heeManclickEffect : clickEffect, Camera.main.ScreenToWorldPoint(Input.mousePosition) + new Vector3(0, 0, 0.15f), Quaternion.identity);
        }
    }

    void Critical(int percent)
    {
        if(Random.Range(0, 100) < percent)
        {
            clickMoneyTextCom.text.color = new Color(255, 0, 0);
            clickMoneyTextCom.text.fontSize = 4f;
            clickMoneyTextCom.money = GameManager.Instance.player.playerData.clickMoney * 3;
            GameManager.Instance.player.playerData.playerMoney += GameManager.Instance.player.playerData.clickMoney * 2;
        }
        else
            clickMoneyTextCom.money = GameManager.Instance.player.playerData.clickMoney;
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
