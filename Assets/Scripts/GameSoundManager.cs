using UnityEngine;
using UnityEngine.Audio;

public class GameSoundManager : Singleton<GameSoundManager>
{
    [SerializeField] private AudioData startGameSoundData;
    [SerializeField] private AudioData checkPointPassSoundData;
    [SerializeField] private AudioData gameOverSoundData;

    public void PlayStartGameSound()
    {
        Play(startGameSoundData);
    }

    public void PlayCheckPointPassSound()
    {
        Play(checkPointPassSoundData);
    }

    public void PlayGameOverSound()
    {
        Play(gameOverSoundData);
    }

    public void Play(AudioData audioData)
    {
        RequiredAudioMethods.PlayResourcesAtPoint(audioData, Vector3.zero, 1f, 0f);
    }
}
