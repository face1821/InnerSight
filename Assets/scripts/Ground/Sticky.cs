using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sticky : MonoBehaviour
{

    //为了角色在移动平台上无法传送的bug于是把这些功能全删了

    // private void OnCollisionEnter2D(Collision2D collision)
    // {
    //     if(null != collision.gameObject.GetComponent<Player>())
    //     {
    //         if( GameManager.instance.isOutSticky == false )
    //             collision.gameObject.transform.SetParent(transform);
    //     }
    // }

    // private void OnCollisionExit2D(Collision2D collision)
    // {
    //     if (null != collision.gameObject.GetComponent<Player>())
    //     {
    //         collision.gameObject.transform.SetParent(null);
    //     }
    // }
}
