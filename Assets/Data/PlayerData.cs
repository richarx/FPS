using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/PlayerData")]
    public class PlayerData : ScriptableObject
    {
        [Header("Movement - Ground")]
        public float groundMaxSpeed;
        public float groundAcceleration;
        public float groundDeceleration;
        
        
        [Header("Gun")]
        public float fireRate;
    }
}
