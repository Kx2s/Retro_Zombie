using Photon.Pun;
using UnityEngine;

public class PlayerShooter : MonoBehaviourPun
{
    public Gun gun;
    public Transform gunPivot;
    public Transform leftHandMount;
    public Transform rightHandMount;

    private PlayerInput playerInput;
    private Animator playerAnimator;

    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        playerAnimator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        gun.gameObject.SetActive(true);
    }

    private void OnDisable()
    {
        gun.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (!photonView.IsMine)
            return;

        if (playerInput.fire)
        {
            gun.Fire();
        }
        else if (playerInput.reload)
        {
            if (gun.Reload())
            {
                playerAnimator.SetTrigger("Reload");
            }
        }

        UpdateUI();
    }

    private void UpdateUI()
    {
        if(gun != null && UIManager.instance != null)
        {
            UIManager.instance.UpdateAmmoText(gun.magAmmo, gun.ammoRemain);
        }
    }

    private void OnAnimatorIK(int layerIndex)
    {
        gunPivot.position =  playerAnimator.GetIKHintPosition(AvatarIKHint.RightElbow);

        playerAnimator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1f);
        playerAnimator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1f);

        playerAnimator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandMount.position);
        playerAnimator.SetIKRotation(AvatarIKGoal.LeftHand, leftHandMount.rotation);
        
        playerAnimator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1f);
        playerAnimator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1f);

        playerAnimator.SetIKPosition(AvatarIKGoal.RightHand, rightHandMount.position);
        playerAnimator.SetIKRotation(AvatarIKGoal.RightHand, rightHandMount.rotation);
    }
}
