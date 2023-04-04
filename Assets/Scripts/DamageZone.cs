using UnityEngine;


public class DamageZone : MonoBehaviour 
{
    void OnTriggerStay2D(Collider2D other)
    {
        var ruby = other.GetComponent<RubyController>();

        if (ruby != null)
        {
            
            ruby.ChangeHealth(-1);
        }
    }
}
