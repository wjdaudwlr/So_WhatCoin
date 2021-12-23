using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpObject : MonoBehaviour
{
    public float moveSpeed;
    public float alphaSpeed;
    public float destroyTime;
    protected Color alpha;

    public virtual void Start()
    {
        Invoke("DestroyObject", destroyTime);
    }

    // Update is called once per frame
    public virtual void Update()
    {
        transform.Translate(new Vector3(0, Random.Range(moveSpeed, moveSpeed + 1) * Time.deltaTime, 0)); // 텍스트 위치
        alpha.a = Mathf.Lerp(alpha.a, 0, Time.deltaTime * alphaSpeed); // 텍스트 알파값
    }

    private void DestroyObject()
    {
        Destroy(gameObject);
    }
}
