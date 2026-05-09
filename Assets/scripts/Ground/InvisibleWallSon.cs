using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvisibleWallSon : MonoBehaviour
{
    [SerializeField] private Player notMainPlayer;

    void OnTriggerEnter2D(Collider2D collision)
    {
        InvisibleWall parent = GetComponentInParent<InvisibleWall>();

        parent.GetComponent<Collider2D>().isTrigger = false;
        parent.GetComponent<SpriteRenderer>().sprite = parent.sprite;
        GameManager.instance.currentSamllLevel++;
        parent.myCamera.MoveToDestination();

        GameObject notMainPlayerPositions = GameObject.Find("NotMainPlayerPositions");
        GameObject playerPosA = notMainPlayerPositions.transform.Find("PlayerPositions" + GameManager.instance.currentSamllLevel)?.gameObject;
        notMainPlayer.transform.position = playerPosA.transform.position;

        Destroy(gameObject);
    }

}
