using Enemies;

namespace Items.Triggers
{
    public class DeathTrigger : Trigger
    {
        private void Start()
        {
            GetComponent<Damageable>().OnDeath.AddListener(() => OnTrigger?.Invoke());
        }
    }
}
