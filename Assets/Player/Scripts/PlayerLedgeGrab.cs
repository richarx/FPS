using UnityEngine;

namespace Player.Scripts
{
    public class PlayerLedgeGrab : IPlayerBehaviour
    {
        private Vector3 startingPosition;
        private float forwardDistance;
        private float height;
        
        private float climbVelocity;
        private Vector3 stickVelocity;
        private Vector3 forwardVelocity;
        
        public void StartBehaviour(PlayerStateMachine player, BehaviourType previous)
        {
            Debug.Log("LEDGE GRAB");

            climbVelocity = 0.0f;
            stickVelocity = Vector3.zero;
            forwardVelocity = Vector3.zero;
            
            startingPosition = player.position;
            startingPosition.y = height;
            
            player.moveVelocity = Vector3.zero;
            player.ApplyMovement();

            player.playerArms.DisplayLedgeGrabArms();
        }

        public void UpdateBehaviour(PlayerStateMachine player)
        {
        }

        public void FixedUpdateBehaviour(PlayerStateMachine player)
        {
            if (player.position.y < height)
            {
                Vector3 position = player.rb.position;
                position = StickToWall(player, position);
                position = ClimbUp(player, position);
                player.rb.MovePosition(position);
            }
            else if (Vector3.Distance(startingPosition, player.position) < forwardDistance + 0.5f)
            {
                Vector3 position = player.rb.position;
                position = MoveForward(player, position);
                player.rb.MovePosition(position);
            }
            else
                player.ChangeBehaviour(player.playerRun);
        }

        private Vector3 ClimbUp(PlayerStateMachine player, Vector3 position)
        {
            float currentHeight = position.y;
            
            float targetHeight = height + 0.1f;

            float finalHeight = Mathf.SmoothDamp(currentHeight, targetHeight, ref climbVelocity, player.playerData.climbTime);

            Vector3 finalPosition = position;
            finalPosition.y = finalHeight;

            return finalPosition;
        }

        private Vector3 StickToWall(PlayerStateMachine player, Vector3 position)
        {
            Vector3 currentPosition = position;
            Vector3 forward = player.orientationPivot.forward;
            
            Vector3 targetPosition = currentPosition;
            targetPosition += forward * (forwardDistance - player.playerData.ledgeStickForwardDistance);
            
            Vector3 finalPosition = Vector3.SmoothDamp(currentPosition, targetPosition, ref stickVelocity, player.playerData.climbTime);

            return finalPosition;
        }
        
        private Vector3 MoveForward(PlayerStateMachine player, Vector3 position)
        {
            Vector3 currentPosition = position;
            Vector3 forward = player.orientationPivot.forward;
            
            Vector3 targetPosition = startingPosition;
            targetPosition += forward * (forwardDistance + 0.6f);
            
            Vector3 finalPosition = Vector3.SmoothDamp(currentPosition, targetPosition, ref forwardVelocity, player.playerData.climbTime);

            return finalPosition;
        }

        public bool DetectLedgeGrab(PlayerStateMachine player)
        {
            Vector3 forward = player.orientationPivot.forward;
            Vector3 position = player.position + (Vector3.up * player.playerData.ledgeDetectionStartingHeight);
            
            bool frontHit = Physics.Raycast(position, forward, out RaycastHit hitForward, player.playerData.ledgeDetectionMaxDistance, ~player.playerData.layersToIgnoreForGroundCheck);

            if (frontHit && hitForward.distance >= player.playerData.ledgeDetectionMinDistance)
            {
                Vector3 abovePosition = position;
                abovePosition += forward * (hitForward.distance + 0.3f);
                abovePosition += Vector3.up * 2.0f;
                bool downHit = Physics.Raycast(abovePosition, Vector3.down, out RaycastHit hitDown, player.playerData.ledgeDetectionMaxHeight + 2.0f, ~player.playerData.layersToIgnoreForGroundCheck);

                if (downHit && hitDown.distance >= player.playerData.ledgeDetectionMinHeight + 1.0f)
                {
                    position += Vector3.up * 2.5f;
                    bool topHit = Physics.Raycast(position, forward, out RaycastHit hitUp, hitForward.distance + 0.5f, ~player.playerData.layersToIgnoreForGroundCheck);

                    if (!topHit)
                    {
                        forwardDistance = hitForward.distance;
                        height = abovePosition.y - hitDown.distance;
                        return true;
                    }
                }
            }

            return false;
        }

        public void StopBehaviour(PlayerStateMachine player, BehaviourType next)
        {
            player.playerArms.RemoveLedgeGrabArms();
        }

        public BehaviourType GetBehaviourType()
        {
            return BehaviourType.LedgeGrab;
        }
    }
}
