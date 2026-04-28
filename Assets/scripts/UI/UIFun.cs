using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFun : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sr;

    public void showMianPlayer()
    {
        sr.enabled = !sr.enabled;
        
    }

}
