using System.Collections.Generic;
using Player.Scripts;
using UnityEngine;

namespace Items.Weapons
{
    public class MuzzleFlash : MonoBehaviour
    {
        [SerializeField] private float lightFlashDuration;
        [SerializeField] private float lightFlashIntensity;
        [Space]
        [SerializeField] private float maxRandomAngle;
        [SerializeField] private Transform muzzlePivot;
        [SerializeField] private List<GameObject> flashPrefabs;
        [SerializeField] private Vector3 muzzlePositionHip;
        [SerializeField] private Vector3 muzzlePositionAim;

        private Light flash;
        private Transform muzzleFlashHolder;

        private void Start()
        {
            PlayerStateMachine player = PlayerStateMachine.instance;

            if (GetComponent<AnimateGun>().isAkimbo)
                player.playerShootGun.OnShootAkimbo.AddListener(TriggerMuzzleFlash);
            else
                player.playerShootGun.OnShoot.AddListener(TriggerMuzzleFlash);

            player.playerAiming.OnChangeAimState.AddListener(AssignMuzzlePosition);
            
            flash = player.muzzleFlashLight;
            muzzleFlashHolder = player.muzzleFlashHolder;
            AssignMuzzlePosition(false);
        }

        private void AssignMuzzlePosition(bool isAiming)
        {
            muzzlePivot.localPosition = isAiming ? muzzlePositionAim : muzzlePositionHip;
        }

        private void TriggerMuzzleFlash()
        {
            flash.gameObject.SetActive(true);
            flash.intensity = lightFlashIntensity;

            StopAllCoroutines();
            StartCoroutine(Tools.Fade(flash, lightFlashDuration, false, maxFade: lightFlashIntensity));
            
            for (int i = 0; i < muzzlePivot.childCount; i++)
            {
                Transform pivot = muzzlePivot.GetChild(i);

                int index = Random.Range(0, flashPrefabs.Count);
                Quaternion direction = Tools.DegreeToVector2(pivot.rotation.eulerAngles.z).AddRandomAngleToDirection(-maxRandomAngle, maxRandomAngle).ToRotation();
                Instantiate(flashPrefabs[index], pivot.position, direction, muzzleFlashHolder);
            }
        }
    }
}
