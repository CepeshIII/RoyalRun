using UnityEngine;
using UnityEngine.Audio;
using static RequiredAudioMethods;

public class CollectableItem : MonoBehaviour
{
    [SerializeField] private AudioResource _audioResource;
    [SerializeField] private AudioMixerGroup _audioMixerGroup;


    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            PlayResourcesAtPoint(_audioResource, transform.position, _audioMixerGroup);
            gameObject.SetActive(false);
        }   
    }

}
