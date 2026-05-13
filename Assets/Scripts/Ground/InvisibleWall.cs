using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvisibleWall : MonoBehaviour
{
    public CameraMove myCamera;
    public Sprite sprite;

    private void OnTriggerExit2D(Collider2D collision)
    {
        // gameObject.GetComponent<Collider2D>().isTrigger = false;
        // GetComponent<SpriteRenderer>().sprite = sprite;
        // GameManager.instance.currentSamllLevel++;
        // myCamera.MoveToDestination();
    }
}
