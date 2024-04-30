using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class PlayerHealth : LivingEntity {
    public Slider healthSlider;

    public AudioClip deathClip;
    public AudioClip hitClip;
    public AudioClip itemPickupClip;

    private AudioSource playerAudioPlayer;
    private Animator playerAnimator;

    private PlayerMovement playerMovement;
    private PlayerShooter playerShooter;

    private void Awake()
    {
        playerAudioPlayer = GetComponent<AudioSource>();
        playerAnimator = GetComponent<Animator>();

        playerMovement = GetComponent<PlayerMovement>();
        playerShooter = GetComponent<PlayerShooter>();                                                     
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        healthSlider.enabled = true;
        healthSlider.maxValue = startingHealth;
        healthSlider.value = health;

        playerMovement.enabled = true;
        playerShooter.enabled = true;
    }

    [PunRPC]
    public override void RestoreHealth(float newHealth)
    {
        base.RestoreHealth(newHealth);

        healthSlider.value = health;
    }

    [PunRPC]
    public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        if (!dead)
        {
            playerAudioPlayer.PlayOneShot(hitClip);
        }

        base.OnDamage(damage, hitPoint, hitNormal);
        healthSlider.value = health;

    }

    public override void Die()
    {
        base.Die();

        healthSlider.gameObject.SetActive(false);

        playerAudioPlayer.PlayOneShot(deathClip);
        playerAnimator.SetTrigger("Die");

        playerMovement.enabled = false;
        playerShooter.enabled = false;

        Invoke("Respawn", 5f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (dead)
            return;

        IItem item = other.GetComponent<IItem>();

        if (item != null)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                item.Use(gameObject);
            }

            playerAudioPlayer.PlayOneShot(itemPickupClip);
        }
    }

    public void Respawn()
    {
        if (photonView.IsMine)
        {
            Vector3 randomSpawnPos = Random.insideUnitSphere * 5f;
            randomSpawnPos.y = 0f;

            transform.position = randomSpawnPos;
        }

        gameObject.SetActive(false);
        gameObject.SetActive(true);

    }
}
