using Items;
using UnityEngine;

namespace Dialog_System
{
    public class DialogTrigger : MonoBehaviour
    {
        [SerializeField] private Transform lookTarget;
        
        private void Start()
        {
            GetComponent<InteractableTrigger>().OnTrigger.AddListener(TriggerDialog);
        }

        private void TriggerDialog()
        {
            DialogManager.instance.TriggerDialog(lookTarget);
        }
    }
}
