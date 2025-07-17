using UnityEngine;
using UnityEngine.Events;

namespace Items
{
    public class InteractableTrigger : Interactable
    {
        [HideInInspector] public UnityEvent OnTrigger = new UnityEvent();
        
        public override void Interact()
        {
            OnTrigger?.Invoke();
        }
    }
}
