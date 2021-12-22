using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ClickMoneyText : MonoBehaviour
{
    public float moveSpeed;
    public float alphaSpeed;
    private float destroyTime;
    public TextMeshPro text;
    Color alpha;
    public ulong money;

    public void SetUp()
    {
        text = GetComponent<TextMeshPro>();
    }

    public void Start()
    {
        destroyTime = 1.5f;
        alpha = text.color;
        text.text = string.Format("{0:n0}", money);
        Invoke("DestroyObject", destroyTime);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector3(0, Random.Range(moveSpeed,moveSpeed + 1) * Time.deltaTime, 0)); // 텍스트 위치

        alpha.a = Mathf.Lerp(alpha.a, 0, Time.deltaTime * alphaSpeed); // 텍스트 알파값
        text.color = alpha;
    }

    private void DestroyObject()
    {
        Destroy(gameObject);
    }
}
