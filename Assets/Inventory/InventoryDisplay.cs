using System;
using System.Collections;
using Backpack;
using Player.Scripts;
using UnityEngine;
using static Backpack.BackpackStorage;

namespace Inventory
{
    public class InventoryDisplay : MonoBehaviour
    {
        [SerializeField] private PocketDisplay componentPocket;
        [SerializeField] private PocketDisplay toolsPocket;
        [SerializeField] private PocketDisplay ammoPocket;
        [SerializeField] private PocketDisplay medicinePocket;
        [SerializeField] private float displayDelay;

        private BackpackStorage backpackStorage;
        
        private PocketDisplay currentPocket;
        
        private void Start()
        {
            PlayerStateMachine player = PlayerStateMachine.instance;
            backpackStorage = player.backpackStorage;
            
            player.playerBackpack.OnOpenBag.AddListener(DisplayPocket);
            player.backpackDisplay.OnSwitchPocketTarget.AddListener(SwitchPocket);
            player.playerBackpack.OnCloseBag.AddListener(HidePocket);
            
            componentPocket.HideInstant();
            toolsPocket.HideInstant();
            ammoPocket.HideInstant();
            medicinePocket.HideInstant();
        }

        private void DisplayPocket()
        {
            StopAllCoroutines();
            StartCoroutine(DisplayPocketCoroutine());
        }

        private IEnumerator DisplayPocketCoroutine()
        {
            currentPocket = ComputePocket(Pocket.tools);
            currentPocket.Setup(backpackStorage.GetPocketStorage(Pocket.tools).GetPocketItems);
            yield return new WaitForSeconds(displayDelay);
            currentPocket.Display();
        }

        private void SwitchPocket(Pocket pocket)
        {
            StopAllCoroutines();
            currentPocket.Hide();
            currentPocket = ComputePocket(pocket);
            currentPocket.Setup(backpackStorage.GetPocketStorage(pocket).GetPocketItems);
            currentPocket.Display();
        }
        
        private void HidePocket()
        {
            StopAllCoroutines();
            currentPocket.Hide();
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
