using Photon.Pun;
using UnityEngine;

public class HealthPackPUN : MonoBehaviourPun, IItem
{
    public float health = 50;

    public void Use(GameObject target)
    {
        LivingEntity life = target.GetComponent<LivingEntity>();

        if(life != null)
        {
            life.RestoreHealth(health);
        }

        PhotonNetwork.Destroy(gameObject);
    }
}
