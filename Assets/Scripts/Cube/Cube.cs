using UnityEngine;
using UnityEngine.Events;

public class Cube : MonoBehaviour
{
    public const int NumberBase = 2;
    public const int StartLevel = 1;
    
    [Min (1)]
    [SerializeField] private int _level = StartLevel;
    public int Level
    {
        get 
        {
            if(_isInitialized)
                return _level;
            else
                throw new System.Exception($"{gameObject.name} isn't initialized");
        }
    }

    public int Number => LevelToNumber(Level);
    [SerializeField] private UnityEvent<int> _initialized;
    public event UnityAction<int> Initialized 
    {
        add => _initialized.AddListener(value);
        remove => _initialized.RemoveListener(value);
    }

    [SerializeField] private UnityEvent _activated;
    public event UnityAction Activated 
    {
        add => _activated.AddListener(value);
        remove => _activated.RemoveListener(value);
    }
    [SerializeField] private UnityEvent _spawnedAfterMerge;
    public event UnityAction SpawnedAfterMerge 
    {
        add => _spawnedAfterMerge.AddListener(value);
        remove => _spawnedAfterMerge.RemoveListener(value);
    }

    [SerializeField] private UnityEvent<Cube> _merging;
    public event UnityAction<Cube> Merging
    {
        add => _merging.AddListener(value);
        remove => _merging.RemoveListener(value);
    }

    [SerializeField] private UnityEvent _destroying;
    public event UnityAction Destroying
    {
        add => _destroying.AddListener(value);
        remove => _destroying.RemoveListener(value);
    }

    private bool _isInitialized = false;
    public bool IsInitialized => _isInitialized;
    private bool _isActivated = false;
    public bool IsActivated => _isActivated;

    

    public void Initialize(int level)
    {
        if(_isInitialized)
            throw new System.Exception($"{gameObject.name} is already initialized");
        _level = level;
        _isInitialized = true;
        _initialized?.Invoke(Number);
    }

    public void Activate()
    {
        if(_isActivated)
            throw new System.Exception($"{gameObject.name} is already activated");
        _isActivated = true;
        _activated?.Invoke();
    }

    public void OnSpawnedAfterMerge() => _spawnedAfterMerge.Invoke();
    public void OnMerging(Cube cube) => _merging.Invoke(cube);
    public static int LevelToNumber(int level) => (int) Mathf.Pow(NumberBase, level);
    public static int NumberToLevel(int number) => (int) Mathf.Log(number, NumberBase);

    private void OnDestroy()
    {  
       _destroying.Invoke(); 
    }
}
