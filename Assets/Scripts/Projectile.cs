using UnityEngine;


public class Projectile : MonoBehaviour
{
    Rigidbody2D ProjectileRigidbody;
    
    void Awake()
    {
        ProjectileRigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if(transform.position.magnitude > 1000.0f)
            Destroy(gameObject);
    }

   
    public void Launch(Vector2 direction, float force)
    {
        ProjectileRigidbody.AddForce(direction * force);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        var enemy = other.collider.GetComponent<Enemy>();

        if (enemy != null)  enemy.RepairMe();
       
        
        Destroy(gameObject);
    }
}
