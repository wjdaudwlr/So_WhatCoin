using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeCamera : MonoBehaviour
{
    // 싱클톤 처리를 위한 instance 변수 선언
    private static ShakeCamera instance;
    // 외부에서 Get 접근만 가능하도록 Instance 프로퍼티 선언
    public static ShakeCamera Instance => instance;

    private float shakeTime;
    private float shakeIntensity;

    /// <summary>
    /// Main Camera 오브젝트에 컴포넌트로 적용하면
    /// 게임을 실행할 때 메모리 할당 / 생성자 메소드 실행
    /// 이 때 자기 자신의 정보를 instance 변수에 저장
    /// </summary>
    public ShakeCamera()
    {
        // 자기 자신에 대한 정보를 static 형태의 instance 변수에 저장해서
        // 외부에서 쉽게 접근할 수 있도록 함
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
        // 흔들리기 직전의 회전 값
        Vector3 startRotation = transform.eulerAngles;
        // 회전의 경우 shakeIntensity에 더 큰 값이 필요하기 때문에 변수로 만들었음
        // (클래스 멤버변수로 선언해 외부에서 조작 가능)
        float power = 10f;

        while (shakeTime > 0.0f)
        {
            // 회전하길 원하는 축만 지정해서 사용 (회전하지 않을 축은 0으로 설정)
            // (클래스 멤버변수로 선언해 외부에서 조작하면 더 좋다)
            float x = 0;
            float y = 0;
            float z = Random.Range(-1f, 1f);
            transform.rotation = Quaternion.Euler(startRotation + new Vector3(x, y, z) * shakeIntensity * power);

            // 시간 감소
            shakeTime -= Time.deltaTime;

            yield return null;
        }

        // 흔들리기 직전의 회전 값으로 설정
        transform.rotation = Quaternion.Euler(startRotation);
    }
}
