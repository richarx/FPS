using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.Scripts
{
    public class PlayerTeleport : MonoBehaviour
    {
        [SerializeField] private GameObject teleporterPrefab;
        
        private PlayerStateMachine player;

        private GameObject currentTeleporter;
        
        private void Start()
        {
            player = PlayerStateMachine.instance;
        }

        private void Update()
        {
            if (Keyboard.current.tKey.wasPressedThisFrame)
                DropTeleporter();
            else if (Keyboard.current.yKey.wasPressedThisFrame)
                Teleport();
        }

        private void Teleport()
        {
            player.rb.MovePosition(currentTeleporter.transform.position + Vector3.up * 3.0f);
        }

        private void DropTeleporter()
        {
            if (currentTeleporter != null)
                Destroy(currentTeleporter);

            currentTeleporter = Instantiate(teleporterPrefab, player.position, Quaternion.identity);
        }
    }
}
