using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicManager : Singleton
{
    [SerializeField] private AudioSource _musicSource;


    void Start()
    {
        DontDestroyOnLoad(gameObject);
        _musicSource = GetComponent<AudioSource>();
        _musicSource.Play();
    }

    private void OnDestroy()
    {
        Destroy(gameObject);
    }
}
