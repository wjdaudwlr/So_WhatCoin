using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeEffect : MonoBehaviour
{
    [SerializeField]
    private float fadeTime; // ���̵� �Ǵ� �ð�
    private Text textFade;  // ���̵� ȿ���� ���Ǵ� �ؽ�Ʈ

    private void Awake()
    {
        textFade = GetComponent<Text>();
    }

    public void FadeText(float start, float end)
    {
        StartCoroutine(Fade(start, end));
    }

    private IEnumerator Fade(float start, float end)
    {
        float current = 0;
        float percent = 0;


        while (percent < 1)
        {
            current += Time.deltaTime;
            percent = current / fadeTime;

            Color color = textFade.color;
            color.a = Mathf.Lerp(start, end, percent);
            textFade.color = color;

            yield return null;
        }
    }
}
