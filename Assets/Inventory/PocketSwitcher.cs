using Player.Scripts;
using static Inventory.BackpackStorage;

namespace Inventory
{
    public class PocketSwitcher
    {
        private readonly PlayerStateMachine player;

        public PocketSwitcher()
        {
            player = PlayerStateMachine.instance;
        }

        public (bool, Pocket) CheckForPocketInput()
        {
            if (player.inputPackage.GetMenuUp.wasPressedThisFrame)
                return (true, Pocket.tools);
            if (player.inputPackage.GetMenuDown.wasPressedThisFrame)
                return (true, Pocket.component);
            if (player.inputPackage.GetMenuLeft.wasPressedThisFrame)
                return (true, Pocket.ammo);
            if (player.inputPackage.GetMenuRight.wasPressedThisFrame)
                return (true, Pocket.medicine);

            return (false, Pocket.tools);
        }
    }
}
