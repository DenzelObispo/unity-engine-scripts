using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy : MonoBehaviour
{
    public float lifeTime = 1;

    void Update()
    {
        Destroy(gameObject, lifeTime);
    }
}
