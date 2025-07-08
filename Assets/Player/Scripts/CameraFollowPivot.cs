using UnityEngine;

namespace Player.Scripts
{
    public class CameraFollowPivot : MonoBehaviour
    {
        [SerializeField] private Transform target;
    
        private void LateUpdate()
        {
            transform.position = target.position;
        }

        [ContextMenu("Go To Target")]
        public void GoToTarget()
        {
            transform.position = target.position;
        }
    }
}
