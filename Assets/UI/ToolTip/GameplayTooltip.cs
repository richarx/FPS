using Player.Scripts;
using Tools_and_Scripts;

namespace UI.ToolTip
{
    public class GameplayTooltip : Tooltip
    {
        private string gamepadText;
        private string keyboardText;

        public void Setup(string gamepad, string keyboard, float destroyAfterDuration)
        {
            gamepadText = gamepad;
            keyboardText = keyboard;

            InputPacker.OnChangeInputType.AddListener((inputType) =>
            {
                textMeshProUGUI.text = ComputeTooltipText(ComputeTextFromInput(inputType));
            });
            
            base.Setup(ComputeTextFromInput(PlayerStateMachine.instance.inputPackage.lastInputType), destroyAfterDuration);
        }

        private string ComputeTextFromInput(InputType inputType)
        {
            if (inputType == InputType.Gamepad)
                return gamepadText;
            else
                return keyboardText;
        }
    }
}
