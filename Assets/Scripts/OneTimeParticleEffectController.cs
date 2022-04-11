using UnityEngine;

public class OneTimeParticleEffectController : MonoBehaviour
{
    [SerializeField] private ParticleSystem[] _particleSystems;
    [SerializeField] private Transform _mainTransform;
    [SerializeField] private bool _isPlayOnAwake = false;

    public void Play()
    {
        float maxParticleSystemDuration = 0f;
        foreach(var item in _particleSystems)
        {
            if(item.main.duration > maxParticleSystemDuration)
                maxParticleSystemDuration = item.main.duration;
            item.Play();
        }
        Destroy(_mainTransform.gameObject, maxParticleSystemDuration);
    }

    private void Awake()
    {
        if(_isPlayOnAwake)
            Play();
    }
}
