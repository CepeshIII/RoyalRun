using UnityEngine;

public class Fog : MonoBehaviour
{
    [SerializeField] ParticleSystem _particleSystem;
    [SerializeField] Player _player;

    [SerializeField] Vector2 _position;
    [SerializeField] float _distance = 40f;

    //Time to increase the density of fog 
    [SerializeField] float TimeToIncreaseDensityFog = 10f;
    [SerializeField] float maxDensityFog = 5f;
    [SerializeField] float currentDensityFog = 0f;
    [SerializeField] float startDensityFog = 1f;
    [SerializeField] float increaseDensityFogSpeed = 0.025f;

    float timeDelay = 0f;

    public void Initialize(Player player)
    {
        _player = player;
        transform.position = player.transform.position;
        _particleSystem = GetComponent<ParticleSystem>();
        currentDensityFog = startDensityFog;
    }

    private void FixedUpdate()
    {
        timeDelay += Time.fixedDeltaTime;
        if (_player != null)
            transform.position = new Vector3(_position.x, _position.y, _player.transform.position.z + _distance);

        if(timeDelay > TimeToIncreaseDensityFog)
        {
            timeDelay = 0f;
            currentDensityFog = Mathf.Min(currentDensityFog + increaseDensityFogSpeed, maxDensityFog);
        }

        var mainModule = _particleSystem.main;
        var minMaxLifetimeCurve = mainModule.startLifetime;
        minMaxLifetimeCurve.constant = currentDensityFog;
        mainModule.startLifetime = minMaxLifetimeCurve;
    }
}
