using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.Interaction
{
    public class PlayerInteraction : MonoBehaviour
    {
        [SerializeField] private Camera playerCamera;
        [SerializeField] private float interactionDistance = 3f;

        private LinuxMonitorInteractable focusedMonitor;

        private void Awake()
        {
            if (playerCamera == null)
            {
                playerCamera = GetComponent<Camera>();
            }
        }

        private void Update()
        {
            // if we're using the monitor, Esc exits it
            if (focusedMonitor != null)
            {
                if (Keyboard.current.escapeKey.wasPressedThisFrame)
                {
                    focusedMonitor.ExitFocus();
                    focusedMonitor = null;
                }

                return;
            }

            // If we're not, then E attempts interacting with it
            if (Keyboard.current.eKey.wasPressedThisFrame)
            {
                TryInteract();
            }
        }

        private void TryInteract()
        {
            Ray ray = playerCamera.ScreenPointToRay(
                Mouse.current.position.ReadValue()
            );

            if (Physics.Raycast(ray, out RaycastHit hit, interactionDistance))
            {
                LinuxMonitorInteractable monitor =
                    hit.collider.GetComponentInParent<LinuxMonitorInteractable>();

                if (monitor != null)
                {
                    focusedMonitor = monitor;
                    focusedMonitor.EnterFocus();
                }
            }
        }
    }
}