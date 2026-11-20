using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.Interaction
{
    public class PlayerInteraction : MonoBehaviour
    {
        [SerializeField] private Camera playerCamera;
        [SerializeField] private float interactionDistance = 3f;
        [SerializeField] private ScenarioMenuController scenarioMenuController;
        [SerializeField] private InteractionPromptController interactionPromptController;
        [SerializeField] private InteractionAudioFeedback interactionAudioFeedback;

        private DeskInteractable hoveredInteractable;

        private DeskInteractable focusedInteractable;

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
                    scenarioMenuController.HandleEscape();
                    return;
                }

                // Priority 2:
                // If we're using any monitor, Escape exits it.
                if (focusedInteractable != null)
                {
                    interactionAudioFeedback?.PlayExit();

                    focusedInteractable.ExitFocus();
                    focusedInteractable = null;

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
                ClearHover();
                return;
            }

            // While using a monitor, don't process E interactions.
            if (focusedInteractable != null)
            {
                ClearHover();
                return;
            }

            UpdateHoveredInteractable();

            if (Keyboard.current.eKey.wasPressedThisFrame &&
                hoveredInteractable != null)
            {
                focusedInteractable =
                    hoveredInteractable;

                ClearHover();

                interactionAudioFeedback?.PlayEnter();

                focusedInteractable.EnterFocus();
            }
        }

        private void OnEnable()
        {
            if (scenarioMenuController != null)
            {
                scenarioMenuController.MenuClosed += RestoreInteractableFocus;
            }
        }

        private void OnDisable()
        {
            if (scenarioMenuController != null)
            {
                scenarioMenuController.MenuClosed -= RestoreInteractableFocus;
            }
        }

        private void RestoreInteractableFocus()
        {
            if (focusedInteractable != null)
            {
                focusedInteractable.EnterFocus();
            }
        }
        private void UpdateHoveredInteractable()
        {
            if (Mouse.current == null)
            {
                ClearHover();
                return;
            }

            Ray ray =
                playerCamera.ScreenPointToRay(
                    Mouse.current.position.ReadValue()
                );

            DeskInteractable newHovered = null;

            if (Physics.Raycast(
                ray,
                out RaycastHit hit,
                interactionDistance))
            {
                newHovered =
                    hit.collider
                        .GetComponentInParent<DeskInteractable>();
            }

            if (newHovered == hoveredInteractable)
            {
                return;
            }

            hoveredInteractable = newHovered;

            if (hoveredInteractable != null)
            {
                interactionPromptController?.ShowPrompt(
                    hoveredInteractable.InteractionLabel
                );
            }
            else
            {
                interactionPromptController?.HidePrompt();
            }
        }

        private void ClearHover()
        {
            hoveredInteractable = null;

            interactionPromptController?.HidePrompt();
        }
    }
}