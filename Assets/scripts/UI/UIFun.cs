// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class UIFun : MonoBehaviour
// {
//     [SerializeField] private SpriteRenderer sr;

//     public void showMianPlayer()
//     {
//         sr.enabled = !sr.enabled;
        
//     }

// }

using UnityEngine;

public class UIFun : MonoBehaviour
{
    [SerializeField] private SpriteRenderer[] spriteRenderers;

    public void showMianPlayer()
    {
        if (spriteRenderers == null || spriteRenderers.Length == 0)
            return;
        SpriteRenderer first = null;
        foreach (var r in spriteRenderers)
        {
            if (r != null)
            {
                first = r;
                break;
            }
        }
        if (first == null)
            return;
        bool newEnabled = !first.enabled;
        foreach (var r in spriteRenderers)
        {
            if (r != null)
                r.enabled = newEnabled;
        }
    }
}
