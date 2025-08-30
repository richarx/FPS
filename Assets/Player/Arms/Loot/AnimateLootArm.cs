using Player.Scripts;
using UnityEngine;

namespace Player.Arms.Loot
{
    public class AnimateLootArm : MonoBehaviour
    {
        [SerializeField] private Transform pivot;
        [SerializeField] private float speed;
        [SerializeField] private float disappearingHeight;

        private void Update()
        {
            pivot.localPosition += Vector3.down * (speed * Time.deltaTime);
            
            if (pivot.localPosition.y <= disappearingHeight)
                PlayerStateMachine.instance.playerArms.RemoveLootArms();
        }
    }
}
