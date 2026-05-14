using System.Collections;
using DG.Tweening;
using UnityEngine;
public class CameraMove : MonoBehaviour
{
    [SerializeField] private Vector2[] Destinations;
    [SerializeField] private float smoothTime = 0.35f;
    [SerializeField] private float arriveThreshold = 0.02f;

    private int currentDestination = 1;
    private bool isMoving;
    private Vector3 targetWorld;
    private Vector3 smoothVelocity;

    private void Update()
    {
        if (!isMoving)
            return;

        transform.position = Vector3.SmoothDamp(
            transform.position,
            targetWorld,
            ref smoothVelocity,
            smoothTime);

        if (Vector3.Distance(transform.position, targetWorld) <= arriveThreshold)
        {
            transform.position = targetWorld;
            smoothVelocity = Vector3.zero;
            isMoving = false;
            currentDestination++;
        }
    }

    /// <summary>
    /// 若 currentDestination 越界则什么也不做；若正在移动中则忽略本次调用（避免连点打乱进度）。
    /// </summary>
    public void MoveToDestination()
    {
        if (isMoving)
            return;

        if (Destinations == null || currentDestination < 0 || currentDestination >= Destinations.Length)
            return;

        Vector2 d = Destinations[currentDestination];
        targetWorld = new Vector3(d.x, d.y, transform.position.z);
        smoothVelocity = Vector3.zero;
        isMoving = true;

        //显影3下（3s）
        var mainPlayer = GameObject.FindWithTag("MainPlayer").GetComponent<Player>();
        mainPlayer.KeepShow(3);
    }
}
