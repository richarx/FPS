using UnityEngine;

namespace Player.Scripts
{
    public class PlayerLeg : MonoBehaviour
    {
        [SerializeField] private float animationSmoothTime;
        [SerializeField] private float slideShakePowerX;
        [SerializeField] private float slideShakePowerY;
        
        private PlayerStateMachine player;
        
        private Vector3 basePosition;
        private Vector3 slidePosition = new Vector3(0.0f, -200.0f, 0.0f);
        private Vector3 offset;

        private Vector3 targetPosition => isSliding ? slidePosition + offset : basePosition;
        private Vector3 velocity;

        private bool isSliding;
        
        private void Start()
        {
            player = PlayerStateMachine.instance;
            basePosition = transform.localPosition;

            player.playerSlide.OnStartSlide.AddListener((_) => isSliding = true);
            player.playerSlide.OnStopSlide.AddListener((_) => isSliding = false);
        }

        private void Update()
        {
            if (Vector3.Distance(transform.localPosition, targetPosition) >= 0.01f)
                transform.localPosition = Vector3.SmoothDamp(transform.localPosition, targetPosition, ref velocity, animationSmoothTime);

            if (isSliding)
                ShakeLeg();
        }

        private void ShakeLeg()
        {
            Vector3 position = Random.insideUnitCircle.ToVector3();
            position.x *= slideShakePowerX;
            position.y *= slideShakePowerY;
            
            offset = position;
        }
    }
}
