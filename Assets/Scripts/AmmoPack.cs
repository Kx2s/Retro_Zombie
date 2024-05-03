using Photon.Pun;
using UnityEngine;

public class AmmoPack : MonoBehaviourPun, IItem
{
    public int ammo = 30;

    public void Use(GameObject target)
    {
        PlayerShooter playerShooter = target.GetComponent<PlayerShooter>();

        if (playerShooter != null && playerShooter.gun != null)
        {
            playerShooter.gun.photonView.RPC("AddAmmo", RpcTarget.All, ammo);
        }

        PhotonNetwork.Destroy(gameObject);
    }
}
