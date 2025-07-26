using System.Collections;
using Player.Scripts;
using UnityEngine;

namespace Items.Weapons
{
    public class WeaponKickback : MonoBehaviour
    {
        [SerializeField] private Vector2 direction;
        [SerializeField] private float force;
        [SerializeField] private float kickSpeed;
        [SerializeField] private float recoverySpeed;

        private Vector3 position => gun.localPosition;
        private Vector3 targetPosition => direction.normalized * force;
        
        private PlayerStateMachine player;
        private RectTransform gun;

        private Vector3 velocity;
        
        private void Start()
        {
            player = PlayerStateMachine.instance;
            gun = GetComponent<RectTransform>();
            
            player.playerShootGun.OnShoot.AddListener(() =>
            {
                if (!player.isAiming)
                {
                    StopAllCoroutines();
                    StartCoroutine(TriggerKickback());
                }
            });
        }

        private IEnumerator TriggerKickback()
        {
            while (position.magnitude <= force - 0.1f)
            {
                gun.localPosition = Vector3.SmoothDamp(position, targetPosition, ref velocity, kickSpeed);
                yield return null;
            }
            
            while (position.magnitude >= 0.1f)
            {
                gun.localPosition = Vector3.SmoothDamp(position, Vector3.zero, ref velocity, recoverySpeed);
                yield return null;
            }

            gun.localPosition = Vector3.zero;
        }
    }
}
