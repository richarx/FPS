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
        [SerializeField] private Slider mouseSlider;
        [SerializeField] private Slider joystickXSlider;
        [SerializeField] private Slider joystickYSlider;
        [SerializeField] private Slider fovSlider;
        [SerializeField] private Camera mainCamera;

        [HideInInspector] public float mouseSensitivity;
        private float mouseMin;
        private float mouseMax;
        
        [HideInInspector] public float joystickXSensitivity;
        private float joystickXMin;
        private float joystickXMax;
        
        [HideInInspector] public float joystickYSensitivity;
        private float joystickYMin;
        private float joystickYMax;

        private float currentFov = 60.0f;
        private float minFov = 20.0f;
        private float maxFov = 120.0f;

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
            mouseSensitivity = player.playerData.mouseSensitivity;
            mouseMin = 0.0f;
            mouseMax = mouseSensitivity * 2.0f;
            
            joystickXSensitivity = player.playerData.joystickSensitivityX;
            joystickXMin = 0.0f;
            joystickXMax = joystickXSensitivity * 2.0f;
            
            joystickYSensitivity = player.playerData.joystickSensitivityY;
            joystickYMin = 0.0f;
            joystickYMax = joystickYSensitivity * 2.0f;

            currentFov = mainCamera.fieldOfView;
            fovSlider.onValueChanged.AddListener((value) => mainCamera.fieldOfView = Tools.NormalizeValueInRange(value, 0.0f, 1.0f, minFov, maxFov));
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
                {
                    ApplyNewParameters();
                    StartCoroutine(ResumeGame());
                }
            }
        }

        private IEnumerator PauseGame()
        {
            Time.timeScale = 0.0f;
            yield return Tools.Fade(blackScreen, 0.15f, true, 0.8f, false);
            SetupParameters();
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
        
        private void SetupParameters()
        {
            mouseSlider.value = Tools.NormalizeValue(mouseSensitivity, mouseMin, mouseMax);
            joystickXSlider.value = Tools.NormalizeValue(joystickXSensitivity, joystickXMin, joystickXMax);
            joystickYSlider.value = Tools.NormalizeValue(joystickYSensitivity, joystickYMin, joystickYMax);
            fovSlider.value = Tools.NormalizeValue(currentFov, minFov, maxFov);
        }

        private void ApplyNewParameters()
        {
            mouseSensitivity = Tools.NormalizeValueInRange(mouseSlider.value, 0.0f, 1.0f, mouseMin, mouseMax);
            joystickXSensitivity = Tools.NormalizeValueInRange(joystickXSlider.value, 0.0f, 1.0f, joystickXMin, joystickXMax);
            joystickYSensitivity = Tools.NormalizeValueInRange(joystickYSlider.value, 0.0f, 1.0f, joystickYMin, joystickYMax);
            currentFov = Tools.NormalizeValueInRange(fovSlider.value, 0.0f, 1.0f, minFov, maxFov);
        }
    }
}
