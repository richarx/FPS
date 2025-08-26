using System;
using System.Collections;
using Inventory.StateMachine;
using Player.Scripts;
using UnityEngine;
using UnityEngine.Events;
using static Inventory.BackpackStorage;

namespace Inventory
{
    public class InventoryDisplay : MonoBehaviour
    {
        [SerializeField] private PocketDisplay componentPocket;
        [SerializeField] private PocketDisplay toolsPocket;
        [SerializeField] private PocketDisplay ammoPocket;
        [SerializeField] private PocketDisplay medicinePocket;
        [SerializeField] private float displayDelay;

        [HideInInspector] public UnityEvent OnDisplayNewPocket = new UnityEvent();
        
        private BackpackStorage backpackStorage;
        
        private PocketDisplay currentPocket;
        public PocketDisplay CurrentPocket => currentPocket;

        private bool isDisplayed;
        public bool IsDisplayed => isDisplayed;
        
        private void Start()
        {
            PlayerStateMachine player = PlayerStateMachine.instance;
            backpackStorage = player.backpackStorage;
            
            InventoryStateMachine.instance.OnSwitchPocketTarget.AddListener(SwitchPocket);
            
            componentPocket.HideInstant();
            toolsPocket.HideInstant();
            ammoPocket.HideInstant();
            medicinePocket.HideInstant();
        }

        public void DisplayPocket(Pocket pocket)
        {
            StopAllCoroutines();
            StartCoroutine(DisplayPocketCoroutine(pocket));
        }

        private IEnumerator DisplayPocketCoroutine(Pocket pocket)
        {
            currentPocket = ComputePocket(pocket);
            currentPocket.Setup(backpackStorage.GetPocketStorage(Pocket.tools).GetPocketItems);
            yield return new WaitForSeconds(displayDelay);
            currentPocket.Display();
            OnDisplayNewPocket?.Invoke();
            isDisplayed = true;
        }

        private void SwitchPocket(Pocket pocket)
        {
            StopAllCoroutines();
            currentPocket.Hide();
            currentPocket = ComputePocket(pocket);
            currentPocket.Setup(backpackStorage.GetPocketStorage(pocket).GetPocketItems);
            currentPocket.Display();
            OnDisplayNewPocket?.Invoke();
        }
        
        public void HidePocket()
        {
            StopAllCoroutines();
            currentPocket.Hide();
            isDisplayed = false;
        }

        private PocketDisplay ComputePocket(Pocket pocket)
        {
            switch (pocket)
            {
                case Pocket.component:
                    return componentPocket;
                case Pocket.tools:
                    return toolsPocket;
                case Pocket.ammo:
                    return ammoPocket;
                case Pocket.medicine:
                    return medicinePocket;
                default:
                    throw new ArgumentOutOfRangeException(nameof(pocket), pocket, null);
            }
        }
    }
}
