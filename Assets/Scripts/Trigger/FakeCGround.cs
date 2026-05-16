using UnityEngine;

public class FakeCGround : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(null != other.GetComponent<Player>())
        {
            Destroy(gameObject);
        }

    }
}