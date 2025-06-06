using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioClip[] audios;
    [SerializeField] AudioSource audioSource;
    float originalPitch;
    public void PlayAudioThere(Transform location, int audioId)
    {
        AudioSource.PlayClipAtPoint(audios[audioId], location.position,1);
    }
    public void PlayAudio(int audioId)
    {
        if (!audioSource.isActiveAndEnabled) return;
        audioSource.pitch = originalPitch;
        audioSource.pitch += Random.Range(-0.2f, 0.2f);
        audioSource.clip = audios[audioId];
        audioSource.Play();
    }
    private void Start()
    {
        if (!audioSource) audioSource = GetComponent<AudioSource>();
        if (!audioSource) ThisInstanceDoesNotHaveAudioSource();
        if (audioSource) originalPitch = audioSource.pitch;
    }
    void ThisInstanceDoesNotHaveAudioSource()
    {
        Debug.LogError($"This {gameObject.name}, located at {transform.position} does not contain an AudioSource, correct or delete this");
        Destroy(this.gameObject);
    }
}
