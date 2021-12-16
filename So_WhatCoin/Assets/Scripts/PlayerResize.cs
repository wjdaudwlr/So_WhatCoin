using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerResize : MonoBehaviour
{
    private Vector3 startSize;      // ���� ũ�� (transform.localScale)
    [SerializeField]
    private Vector3 endSize;        // ���� ũ��
    [SerializeField]
    private float resizeTime;       // ũ�� ��ȭ�� �ҿ�Ǵ� �ð�

    public bool isclick = true;

    public IEnumerator ClickEffect()
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
