using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeCamera : MonoBehaviour
{
    // ��Ŭ�� ó���� ���� instance ���� ����
    private static ShakeCamera instance;
    // �ܺο��� Get ���ٸ� �����ϵ��� Instance ������Ƽ ����
    public static ShakeCamera Instance => instance;

    private float shakeTime;
    private float shakeIntensity;

    /// <summary>
    /// Main Camera ������Ʈ�� ������Ʈ�� �����ϸ�
    /// ������ ������ �� �޸� �Ҵ� / ������ �޼ҵ� ����
    /// �� �� �ڱ� �ڽ��� ������ instance ������ ����
    /// </summary>
    public ShakeCamera()
    {
        // �ڱ� �ڽſ� ���� ������ static ������ instance ������ �����ؼ�
        // �ܺο��� ���� ������ �� �ֵ��� ��
        instance = this;
    }


    public void OnShakeCamera(float shakeTime = 1.0f, float shakeIntensity = 0.1f)
    {
        this.shakeTime = shakeTime;
        this.shakeIntensity = shakeIntensity;

        StopCoroutine(ShakeByRotation());
        StartCoroutine(ShakeByRotation());
    }




    private IEnumerator ShakeByRotation()
    {
        // ��鸮�� ������ ȸ�� ��
        Vector3 startRotation = transform.eulerAngles;
        // ȸ���� ��� shakeIntensity�� �� ū ���� �ʿ��ϱ� ������ ������ �������
        // (Ŭ���� ��������� ������ �ܺο��� ���� ����)
        float power = 10f;

        while (shakeTime > 0.0f)
        {
            // ȸ���ϱ� ���ϴ� �ุ �����ؼ� ��� (ȸ������ ���� ���� 0���� ����)
            // (Ŭ���� ��������� ������ �ܺο��� �����ϸ� �� ����)
            float x = 0;
            float y = 0;
            float z = Random.Range(-1f, 1f);
            transform.rotation = Quaternion.Euler(startRotation + new Vector3(x, y, z) * shakeIntensity * power);

            // �ð� ����
            shakeTime -= Time.deltaTime;

            yield return null;
        }

        // ��鸮�� ������ ȸ�� ������ ����
        transform.rotation = Quaternion.Euler(startRotation);
    }
}
