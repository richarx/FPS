using Player.Scripts;
using UnityEngine;

namespace Items
{
    public class ThrowableItem : MonoBehaviour
    {
        [SerializeField] private float throwPower;
        [SerializeField] private float deceleration;
        [SerializeField] private float gravity;
        [SerializeField] private float maxFallSpeed;
        
        private Rigidbody rb;
        private Vector3 velocity;
        
        private bool isSetup;
        
        public void Setup(Vector3 direction)
        {
            rb = GetComponent<Rigidbody>();
            velocity = direction.normalized * throwPower + PlayerStateMachine.instance.rb.velocity;
            rb.velocity = velocity;
            
            isSetup = true;
        }

        private void FixedUpdate()
        {
            if (!isSetup)
                return;
            
            UpdateSpeed();
            UpdateGravity();

            rb.velocity = velocity;
        }

        private void UpdateSpeed()
        {
            velocity.x = Mathf.MoveTowards(velocity.x, 0.0f, deceleration * Time.fixedDeltaTime);
            velocity.z = Mathf.MoveTowards(velocity.z, 0.0f, deceleration * Time.fixedDeltaTime);
        }

        private void UpdateGravity()
        {
            velocity.y = Mathf.MoveTowards(velocity.y, -maxFallSpeed, gravity * Time.fixedDeltaTime);
        }
    }
}
