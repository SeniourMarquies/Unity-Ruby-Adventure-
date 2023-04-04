using UnityEngine;


public class HealthCollectible : MonoBehaviour 
{
    void OnTriggerEnter2D(Collider2D other)
    {
        var ruby = other.GetComponent<RubyController>();

        if (ruby != null)
        {
            ruby.ChangeHealth(1);
            Destroy(gameObject);
        }
    }
}
