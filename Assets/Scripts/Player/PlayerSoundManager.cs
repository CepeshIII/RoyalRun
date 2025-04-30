using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.PlayerLoop;

public class PlayerSoundManager : MonoBehaviour
{
    [SerializeField] private Player _player;

    [SerializeField] private AudioSource _walkAudioSource;
    [SerializeField] private AudioData _stumbleAudioData;

    void OnEnable()
    {
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        _player.OnPlayerWalk += PlayWalkSound;
        _player.OnPlayerStumble += PlayStumbleSound;
    }

    private void PlayWalkSound()
    {
        _walkAudioSource?.Play();
    }

    private void PlayStumbleSound()
    {
        RequiredAudioMethods.PlayResourcesAtPoint(_stumbleAudioData, _player.transform.position);
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
