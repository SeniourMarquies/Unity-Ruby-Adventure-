using System;
using UnityEngine;


public class Enemy : MonoBehaviour
{
	public float speed;
	public float timeToChange;
	public bool horizontal;

	public GameObject smokeParticleEffect;
	public ParticleSystem fixedParticleEffect;

	public AudioClip hitSound;
	public AudioClip fixedSound;
	
	Rigidbody2D robotRigidbody;
	float remainingTimeToChange;
	Vector2 direction = Vector2.right;
	bool IsRepaired = false;
	
	Animator robotAnimator;
	
	AudioSource audioSource;
	
	void Start ()
	{
		robotRigidbody = GetComponent<Rigidbody2D>();
		remainingTimeToChange = timeToChange;

		direction = horizontal ? Vector2.right : Vector2.down;

		robotAnimator = GetComponent<Animator>();

		audioSource = GetComponent<AudioSource>();
	}
	
	void Update()
	{
		if(IsRepaired)
			return;
		
		remainingTimeToChange -= Time.deltaTime;

		if (remainingTimeToChange <= 0)
		{
			remainingTimeToChange += timeToChange;
			direction *= -1;
		}

		robotAnimator.SetFloat("ForwardX", direction.x);
		robotAnimator.SetFloat("ForwardY", direction.y);
	}

	void FixedUpdate()
	{
		robotRigidbody.MovePosition(robotRigidbody.position + direction * speed * Time.deltaTime);
	}

	void OnCollisionStay2D(Collision2D other)
	{
		if(IsRepaired)
			return;
		
		RubyController enemy = other.collider.GetComponent<RubyController>();
		
		if(enemy != null)
			enemy.ChangeHealth(-1);
	}

	public void RepairMe()
	{
		robotAnimator.SetTrigger("Fixed");
		IsRepaired = true;
		
		smokeParticleEffect.SetActive(false);

		Instantiate(fixedParticleEffect, transform.position + Vector3.up * 0.5f, Quaternion.identity);

		robotRigidbody.simulated = false;
		
		audioSource.Stop();
		audioSource.PlayOneShot(hitSound);
		audioSource.PlayOneShot(fixedSound);
	}
}
