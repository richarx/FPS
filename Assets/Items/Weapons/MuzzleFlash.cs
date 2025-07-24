using System.Collections.Generic;
using Player.Scripts;
using UnityEngine;

namespace Items.Weapons
{
    public class MuzzleFlash : MonoBehaviour
    {
        [SerializeField] private Transform muzzlePivot;
        [SerializeField] private List<GameObject> flashPrefabs;
        [SerializeField] private Vector3 muzzlePositionHip;
        [SerializeField] private Vector3 muzzlePositionAim;
        
        private Transform muzzleFlashHolder;
        
        private void Start()
        {
            PlayerStateMachine.instance.playerShootGun.OnShoot.AddListener(TriggerMuzzleFlash);
            PlayerStateMachine.instance.playerAiming.OnChangeAimState.AddListener((isAiming) =>
            {
                muzzlePivot.localPosition = isAiming ? muzzlePositionAim : muzzlePositionHip;
            });
            muzzleFlashHolder = PlayerStateMachine.instance.muzzleFlashHolder;
            muzzlePivot.localPosition = muzzlePositionHip;
        }

        private void TriggerMuzzleFlash()
        {
            for (int i = 0; i < muzzlePivot.childCount; i++)
            {
                Transform pivot = muzzlePivot.GetChild(i);

                int index = Random.Range(0, flashPrefabs.Count);
                Instantiate(flashPrefabs[index], pivot.position, pivot.rotation, muzzleFlashHolder);
            }
        }
    }
}
