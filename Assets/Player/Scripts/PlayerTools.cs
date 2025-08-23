using Items;
using Tools_and_Scripts;
using UnityEngine;
using UnityEngine.Events;

namespace Player.Scripts
{
    public class PlayerTools : MonoBehaviour
    {
        [SerializeField] private ItemData glowStick;

        [HideInInspector] public UnityEvent OnThrowItem = new UnityEvent();
        
        private PlayerStateMachine player;

        private Transform currentTool;

        private bool isInputReset;
        private float lastThrowTimestamp;
        
        private void Start()
        {
            player = PlayerStateMachine.instance;
        }

        private void Update()
        {
            if (PlayerInputs.GetDownArrow())
                EquipTool(glowStick);
            
            if (CanThrow() && PlayerInputs.GetRightTrigger(isHeld: true))
            {
                ThrowItem();
                isInputReset = false;
            }
            
            if (!isInputReset && !PlayerInputs.GetRightTrigger(isHeld: true))
                isInputReset = true;
        }

        private void ThrowItem()
        {
            Debug.Log("Throw Item");
            lastThrowTimestamp = Time.time;
            OnThrowItem?.Invoke();
        }

        private bool CanThrow()
        {
            if (player.playerArms.currentArmType != PlayerArms.ArmType.Throw)
                return false;

            if (lastThrowTimestamp > 0.0f && Time.time - lastThrowTimestamp <= 1.0f)
                return false;
            
            return isInputReset;
        }

        private void EquipTool(ItemData item)
        {
            currentTool = player.playerArms.EquipThrowTool(item);
        }
    }
}
