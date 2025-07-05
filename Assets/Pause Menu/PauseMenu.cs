using System.Collections;
using Player.Scripts;
using Tools_and_Scripts;
using UnityEngine;
using UnityEngine.UI;

namespace Pause_Menu
{
    public class PauseMenu : MonoBehaviour
    {
        [SerializeField] private Image blackScreen;
        [SerializeField] private GameObject pausePanel;
        [SerializeField] private SettingSlider mouseSlider;
        [SerializeField] private SettingSlider joystickXSlider;
        [SerializeField] private SettingSlider joystickYSlider;
        [SerializeField] private SettingSlider aimSlider;
        [SerializeField] private SettingSlider fovSlider;
        [SerializeField] private Camera mainCamera;

        public float mouseSensitivity => mouseSlider.GetSliderValue();
        public float joystickXSensitivity => joystickXSlider.GetSliderValue();
        public float joystickYSensitivity => joystickYSlider.GetSliderValue();
        public float aimSensitivityMultiplier => aimSlider.GetSliderValue();
        public float currentFov => fovSlider.GetSliderValue();
        
        public static PauseMenu instance;

        private PlayerStateMachine player;
        
        private bool isPaused;
        public bool IsPaused => isPaused;

        private void Awake()
        {
            instance = this;
            blackScreen.gameObject.SetActive(false);
            pausePanel.SetActive(false);
        }

        private void Start()
        {
            player = PlayerStateMachine.instance;
            
            mouseSlider.Initialize(player.playerData.mouseSensitivity, 0.0f, player.playerData.mouseSensitivity * 3.0f);
            joystickXSlider.Initialize(player.playerData.joystickSensitivityX, 0.0f, player.playerData.joystickSensitivityX * 2.0f);
            joystickYSlider.Initialize(player.playerData.joystickSensitivityY, 0.0f, player.playerData.joystickSensitivityY * 2.0f);
            aimSlider.Initialize(player.playerData.aimSensitivityMultiplier, 0.1f, 2.0f);
            fovSlider.Initialize(mainCamera.fieldOfView, 30.0f, 170.0f);
            
            fovSlider.slider.onValueChanged.AddListener((value) => mainCamera.fieldOfView = fovSlider.GetSliderValue());
        }

        private void Update()
        {
            if (PlayerInputs.GetStartButton())
            {
                isPaused = !isPaused;
                
                StopAllCoroutines();
                if (isPaused)
                    StartCoroutine(PauseGame());
                else
                    StartCoroutine(ResumeGame());
            }
        }

        private IEnumerator PauseGame()
        {
            Time.timeScale = 0.0f;
            yield return Tools.Fade(blackScreen, 0.15f, true, 0.8f, false);
            pausePanel.SetActive(true);
            
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
        }

        private IEnumerator ResumeGame()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            
            pausePanel.SetActive(false);
            yield return Tools.Fade(blackScreen, 0.3f, false, 0.8f, false);
            Time.timeScale = 1.0f;
        }
    }
}
