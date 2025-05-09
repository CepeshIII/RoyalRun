using UnityEngine;
using UnityEngine.Audio;

public static class RequiredAudioMethods
{
    public static void PlayResourcesAtPoint(AudioResource audioResource, Vector3 position, 
        AudioMixerGroup audioMixerGroup = null, float volume = 1f, float spatialBlend = 1)
    {
        GameObject gameObject = new GameObject("One shot audio");
        gameObject.transform.position = position;

        AudioSource audioSource = (AudioSource)gameObject.AddComponent(typeof(AudioSource));
        audioSource.outputAudioMixerGroup = audioMixerGroup;
        audioSource.resource = audioResource;
        audioSource.spatialBlend = spatialBlend;
        audioSource.dopplerLevel = 0f;
        audioSource.volume = volume;
        audioSource.Play();

        var destroyer = (ConditionalDestroyer)gameObject.AddComponent(typeof(ConditionalDestroyer));
        destroyer.Initialize(x => x.isPlaying, audioSource);
    }
    public static void PlayResourcesAtPoint(AudioData audioData, Vector3 position, 
                                               float volume = 1f, float spatialBlend = 1)
    {
        PlayResourcesAtPoint(audioData.audioResource, position, 
                                audioData.audioMixerGroup, volume, spatialBlend);
    }



}
