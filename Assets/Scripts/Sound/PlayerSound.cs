using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    [Space, Header("Sound Settings")]
    [SerializeField] private AudioEvent ouchAudioEvent = null;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.spatialBlend = 1.0f;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Pickable>() != null)
            ouchAudioEvent.Play(audioSource);
    }
}
