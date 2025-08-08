using UnityEngine;

namespace Player.Scripts
{
    public class PlayerGunKickback : MonoBehaviour
    {
        [SerializeField] private PlayerRecoil playerRecoil;

        private PlayerGun playerGun;
        private PlayerAiming playerAiming;

        private void Start()
        {
            playerGun = GetComponent<PlayerGun>();
            playerAiming = GetComponent<PlayerAiming>();
        }

        public void Kickback()
        {
            float xKickBack = Tools.RandomPositiveOrNegative(Tools.RandomAround(playerGun.CurrentWeapon.xRecoil, 0.3f));
            float yKickBack = Tools.RandomAround(playerGun.CurrentWeapon.yRecoil, 0.15f);

            if (playerAiming.IsAiming)
            {
                xKickBack *= 0.3f;
                yKickBack *= 0.3f;
            }
            
            playerRecoil.KickBack(xKickBack, yKickBack);
        }
    }
}
