using UnityEngine;
using UnityEngine.Events;

namespace Player.Scripts
{
    public class PlayerAmmo : MonoBehaviour
    {
        [HideInInspector] public UnityEvent OnRefillAmmo = new UnityEvent();
        
        private PlayerStateMachine player;

        private int currentAmmo;
        public int CurrentAmmo => currentAmmo;
        public bool IsEmpty => currentAmmo < 1;

        private void Start()
        {
            player = GetComponent<PlayerStateMachine>();
            currentAmmo = player.playerData.startingAmmo;
        }

        public void ConsumeAmmo()
        {
            currentAmmo -= 1;
        }

        public void RefillAmmo()
        {
            currentAmmo = player.playerData.startingAmmo;
            OnRefillAmmo?.Invoke();
        }
    }
}
