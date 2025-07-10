using UnityEngine;

namespace Enemies
{
    public class ChasePlayer : MonoBehaviour
    {
        [SerializeField] private float aggroRange;
        [SerializeField] private float stopMovingRange;
        
        private EnemyMover mover;

        private void Start()
        {
            mover = GetComponent<EnemyMover>();
        }

        private void LateUpdate()
        {
            float distance = mover.DistanceToPlayer();

            if (distance >= aggroRange || distance <= stopMovingRange)
                mover.StopMoving();
            else
                mover.MoveInDirection(mover.DirectionToPlayerFlatten());
        }
    }
}
