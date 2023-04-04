using System;
using UnityEngine;

public class RubyController : MonoBehaviour
{
    public float speed = 4;
    
    
    public int maxHealth = 5;
    public float timeInvincible = 2.0f;
    public Transform respawnPosition;
    public ParticleSystem hitParticle;
    
    public GameObject projectilePrefab;

    public AudioClip hitSound;
    public AudioClip shootingSound;
    
    public int health
    {
        get { return currentHealth; }
    }
    
    Rigidbody2D rubyRigidbody;
    Vector2 currentInput;
    
    int currentHealth;
    float invincibleTimer;
    bool isInvincible;
   
    Animator animator;
    Vector2 lookDirection = new Vector2(1, 0);
    
    AudioSource audioSource;
    
    void Start()
    {
        rubyRigidbody = GetComponent<Rigidbody2D>();
                
        invincibleTimer = -1.0f;
        currentHealth = maxHealth;
        
        animator = GetComponent<Animator>();
        
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0)
                isInvincible = false;
        }

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
                
        Vector2 move = new Vector2(horizontal, vertical);
        
        if(!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            lookDirection.Set(move.x, move.y);
            lookDirection.Normalize();
        }

        currentInput = move;



        animator.SetFloat("Look X", lookDirection.x);
        animator.SetFloat("Look Y", lookDirection.y);
        animator.SetFloat("Speed", move.magnitude);


        if (Input.GetKeyDown(KeyCode.C))
            LaunchProjectile();
        
        if (Input.GetKeyDown(KeyCode.X))
        {
            RaycastHit2D hit = Physics2D.Raycast(rubyRigidbody.position + Vector2.up * 0.2f, lookDirection, 1.5f,
                1 << LayerMask.NameToLayer("NPC"));
            if (hit.collider != null)
            {
                NonPlayerCharacter character = hit.collider.GetComponent<NonPlayerCharacter>();
                if (character != null)
                {
                    character.DisplayMessageBox();
                }  
            }
        }
 
    }

    void FixedUpdate()
    {
        Vector2 position = rubyRigidbody.position;
        
        position = position + currentInput * speed * Time.deltaTime;
        
        rubyRigidbody.MovePosition(position);
    }

    public void ChangeHealth(int amount)
    {
        if (amount < 0)
        { 
            if (isInvincible)
                return;
            
            isInvincible = true;
            invincibleTimer = timeInvincible;
            
            animator.SetTrigger("Hit");
            audioSource.PlayOneShot(hitSound);

            Instantiate(hitParticle, transform.position + Vector3.up * 0.5f, Quaternion.identity);
        }
        
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        
        if(currentHealth == 0)
            Respawn();
        
        UIHealthBar.Instance.SetValue(currentHealth / (float)maxHealth);
    }
    
    void Respawn()
    {
        ChangeHealth(maxHealth);
        transform.position = respawnPosition.position;
    }
    
    void LaunchProjectile()
    {
        GameObject projectileObject = Instantiate(projectilePrefab, rubyRigidbody.position + Vector2.up * 0.5f, Quaternion.identity);

        Projectile projectile = projectileObject.GetComponent<Projectile>();
        projectile.Launch(lookDirection, 300);
        
        animator.SetTrigger("Launch");
        audioSource.PlayOneShot(shootingSound);
    }
    

    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
}
