using UnityEngine;

public class InteractionAudioFeedback : MonoBehaviour
{
    [SerializeField] private AudioClip interactionClip;

    [SerializeField]
    [Range(0f, 1f)]
    private float enterVolume = 0.25f;

    [SerializeField]
    [Range(0f, 1f)]
    private float exitVolume = 0.18f;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        audioSource.playOnAwake = false;
        audioSource.loop = false;
        audioSource.spatialBlend = 0f;
    }

    public void PlayEnter()
    {
        if (interactionClip != null)
        {
            audioSource.PlayOneShot(
                interactionClip,
                enterVolume
            );
        }
    }

    public void PlayExit()
    {
        if (interactionClip != null)
        {
            audioSource.PlayOneShot(
                interactionClip,
                exitVolume
            );
        }
    }
}