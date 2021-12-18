using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeEffect : MonoBehaviour
{
    [SerializeField]
    private float fadeTime; // 페이드 되는 시간
    private Text textFade;  // 페이드 효과에 사용되는 텍스트

    private void Awake()
    {
        textFade = GetComponent<Text>();

        StartCoroutine(FadeInOut());
    }

    private IEnumerator FadeInOut()
    {
        while (true)
        {
            yield return StartCoroutine(Fade(1, 0.2f));

            yield return StartCoroutine(Fade(0.2f, 1));
        }
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
