using Items;
using UnityEngine;

namespace Weapons.Throw
{
    public class HandHoldItem : MonoBehaviour
    {
        [SerializeField] private Transform pivot;

        public Transform EquipItem(ItemData itemData)
        {
            Transform newTool = Instantiate(itemData.inHandPrefab, Vector3.zero, Quaternion.identity, pivot).transform;
            newTool.localPosition = new Vector3(0.0f, 0.0f, 0.0f);

            return newTool;
        }
    }
}
