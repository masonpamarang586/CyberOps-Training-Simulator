using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.Interaction
{
    public class PlayerInteraction : MonoBehaviour
    {
        [SerializeField] private Camera playerCamera;
        [SerializeField] private float interactionDistance = 3f;
        [SerializeField] private ScenarioMenuController scenarioMenuController;

        private MonitorInteractable focusedMonitor;

        private void Awake()
        {
            if (playerCamera == null)
            {
                playerCamera = GetComponent<Camera>();
            }
        }

        private void Update()
        {
            if (Keyboard.current == null)
            {
                return;
            }

            if (Keyboard.current.escapeKey.wasPressedThisFrame)
            {
                // Priority 1:
                // If the pause menu is open, Escape resumes.
                if (scenarioMenuController != null &&
                    scenarioMenuController.IsMenuOpen)
                {
                    scenarioMenuController.ResumeGame();
                    return;
                }

                // Priority 2:
                // If we're using any monitor, Escape exits it.
                if (focusedMonitor != null)
                {
                    focusedMonitor.ExitFocus();
                    focusedMonitor = null;
                    return;
                }

                // Priority 3:
                // Otherwise Escape opens the pause menu.
                if (scenarioMenuController != null)
                {
                    scenarioMenuController.OpenMenu();
                    return;
                }
            }

            // Don't allow world interaction while paused.
            if (scenarioMenuController != null &&
                scenarioMenuController.IsMenuOpen)
            {
                return;
            }

            // While using a monitor, don't process E interactions.
            if (focusedMonitor != null)
            {
                return;
            }

            if (Keyboard.current.eKey.wasPressedThisFrame)
            {
                TryInteract();
            }
        }

        private void OnEnable()
        {
            if (scenarioMenuController != null)
            {
                scenarioMenuController.MenuClosed += RestoreMonitorFocus;
            }
        }

        private void OnDisable()
        {
            if (scenarioMenuController != null)
            {
                scenarioMenuController.MenuClosed -= RestoreMonitorFocus;
            }
        }

        private void RestoreMonitorFocus()
        {
            if (focusedMonitor != null)
            {
                focusedMonitor.EnterFocus();
            }
        }

        private void TryInteract()
        {
            Ray ray = playerCamera.ScreenPointToRay(
                Mouse.current.position.ReadValue()
            );

            if (Physics.Raycast(ray, out RaycastHit hit, interactionDistance))
            {
                MonitorInteractable monitor =
                    hit.collider.GetComponentInParent<MonitorInteractable>();

                if (monitor != null)
                {
                    focusedMonitor = monitor;
                    focusedMonitor.EnterFocus();
                }
            }
        }
    }
}