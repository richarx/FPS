namespace Items
{
    public class InteractableTrigger : Interactable
    {
        public override void Interact()
        {
            OnTrigger?.Invoke();
        }
    }
}
