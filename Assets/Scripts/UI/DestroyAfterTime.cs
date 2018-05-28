using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{

    public float t;

    // Use this for initialization
    void Start()
    {
        Destroy(gameObject, t);
    }

}
