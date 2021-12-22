using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDestroyerByTime : MonoBehaviour
{

    public float destroyTime;

    void Start()
    {
        Destroy(gameObject, destroyTime);
    }
}
