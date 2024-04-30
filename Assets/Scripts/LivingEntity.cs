using Photon.Pun;
using System;
using UnityEngine;

public class LivingEntity : MonoBehaviourPun, IDamageable
{
    public float startingHealth = 100f;
    public float health { get; protected set; }
    public bool dead { get; protected set; }
    public event Action onDeath;

    [PunRPC]
    public void ApplyUpdatedHealth(float newHealth, bool newDead)
    {
        health = newHealth;
        dead = newDead;
    }

    protected virtual void OnEnable()
    {
        dead = false;
        health = startingHealth;
    }

    [PunRPC]
    public virtual void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            health -= damage;

            photonView.RPC("ApplyUpdatedHealth", RpcTarget.Others, health, dead);
            photonView.RPC("OnDamage", RpcTarget.Others, damage, hitPoint, hitNormal);
        }

        if (health <= 0 && !dead)
        {
            Die();
        }
    }

    [PunRPC]
    public virtual void RestoreHealth(float newHealth)
    {
        if (dead)
            return;

        if (PhotonNetwork.IsMasterClient)
        {
            health += newHealth;
            photonView.RPC("ApplyUpdatedHealth", RpcTarget.Others, health, dead);
            photonView.RPC("RestoreHealth", RpcTarget.Others, newHealth);
        }
    }

    public virtual void Die()
    {
        if (onDeath != null)
        {
            onDeath();
        }
        dead = true;
    }
}
