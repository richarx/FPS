using UnityEngine;

namespace Player.Arms.Ledge_Arms
{
    public class AnimateLedgeArms : MonoBehaviour
    {
        [SerializeField] private Transform pivot;
        [SerializeField] private float speed;

        private void Update()
        {
            pivot.localPosition += Vector3.down * (speed * Time.deltaTime);
        }
    }
}
