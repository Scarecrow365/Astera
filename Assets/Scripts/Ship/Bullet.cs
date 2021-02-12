using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float lifeTime;
    [SerializeField] private Rigidbody rb;

    public void Init()
    {
        rb.velocity = transform.forward * speed;
        Invoke("Deactivate", lifeTime);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("Asteroid"))
        {
            Deactivate();
        }
    }

    private void Deactivate()
    {
        gameObject.SetActive(false);
    }
}