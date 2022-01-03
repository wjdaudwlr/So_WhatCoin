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
