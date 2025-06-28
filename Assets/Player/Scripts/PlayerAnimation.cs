using UnityEngine;

namespace Player.Scripts
{
    public class PlayerAnimation : MonoBehaviour
    {
        [SerializeField] private Animator graphics;
        
        private PlayerGun playerGun;

        private void Start()
        {
            playerGun = GetComponent<PlayerGun>();
            playerGun.OnShoot.AddListener(() => graphics.Play("Shoot", 0, 0.0f));
        }
    }
}
