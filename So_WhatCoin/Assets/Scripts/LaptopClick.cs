using System.Collections;
using System.Collections.Generic;
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

    private SpriteRenderer sptrieRenderer;

    public bool isclick = true;

    private int currentLaptopSprite = 0;    // ���� ��Ʈ�� �̹���

    private void Awake()
    {
        sptrieRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        sptrieRenderer.sprite = laptopImages[currentLaptopSprite];
    }


    public void ClickToPlayer()
    {
        if (isclick)
            StartCoroutine(ClickEffect());
        GameManager.Instance.player.playerData.playerMoney += GameManager.Instance.player.playerData.clickMoney;
        currentLaptopSprite = (currentLaptopSprite % (laptopImages.Length));
        sptrieRenderer.sprite = laptopImages[currentLaptopSprite++];

        Instantiate(clickEffect, Camera.main.ScreenToWorldPoint(Input.mousePosition) + new Vector3(0,0,0.5f), Quaternion.identity);

        if (GameManager.Instance.player.playerData.itemDict["doge"])
        {
            GameObject moneyText = Instantiate(clickMoneyText);
            moneyText.transform.position = clickPos.position + new Vector3(Random.Range(-1.1f, 1.1f), Random.Range(-0.2f, 0.2f), 0);
            moneyText.GetComponent<ClickMoneyText>().money = GameManager.Instance.player.playerData.clickMoney;
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
