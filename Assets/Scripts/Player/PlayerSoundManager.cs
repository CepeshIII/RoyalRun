using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.PlayerLoop;

public class PlayerSoundManager : MonoBehaviour
{
    [SerializeField] private Player _player;

    [SerializeField] private AudioSource _walkAudioSource;

    void OnEnable()
    {
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        _player.OnPlayerWalk += PlayWalkSound;
    }


    private void PlayWalkSound()
    {
        _walkAudioSource?.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy()
    {
        if (_player != null) 
        { 
            if (_player.OnPlayerFall != null)
                _player.OnPlayerFall -= PlayWalkSound;
        }
    }

}
