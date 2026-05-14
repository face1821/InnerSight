using System;
using UnityEngine;

public class DestroyWhenRelease : MonoBehaviour
{
    private void Awake()
    {
        if (!Application.isEditor)
            Destroy(gameObject);
    }
}