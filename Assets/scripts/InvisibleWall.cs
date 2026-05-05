using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvisibleWall : MonoBehaviour
{
    [SerializeField] private CameraMove myCamera;

    private void OnTriggerExit2D(Collider2D collision)
    {
        gameObject.GetComponent<Collider2D>().isTrigger = false;
        myCamera.MoveToDestination();
    }
}
