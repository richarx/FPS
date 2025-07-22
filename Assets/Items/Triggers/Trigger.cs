using UnityEngine;
using UnityEngine.Events;

namespace Items.Triggers
{
    public class Trigger : MonoBehaviour
    {
        [HideInInspector] public UnityEvent OnTrigger = new UnityEvent();
    }
}
