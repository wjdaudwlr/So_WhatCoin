using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerResize : MonoBehaviour
{
    private Vector3 startSize;      // 시작 크기 (transform.localScale)
    [SerializeField]
    private Vector3 endSize;        // 종료 크기
    [SerializeField]
    private float resizeTime;       // 크기 변화에 소요되는 시간

    public bool isclick = true;

    public IEnumerator ClickEffect()
    {
        yield return isclick = false;
        startSize = transform.localScale;

        
         // resizeTime 시간 동안 start에서 end로 크기 변화
         yield return StartCoroutine(Resize(startSize, endSize, resizeTime));

         // resizeTime 시간 동안 end에서 start로 크기 변화
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
