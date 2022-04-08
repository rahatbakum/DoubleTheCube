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

    private bool _isInitialized = false;
    private bool _isActivated = false;

    

    public void Initialize(int level)
    {
        if(_isInitialized)
            throw new System.Exception($"{gameObject.name} is already initialized");
        _level = level;
        _isInitialized = true;
        _initialized?.Invoke(GetNumber());
    }

    public void Activate()
    {
        if(_isActivated)
            throw new System.Exception($"{gameObject.name} is already activated");
        _isActivated = true;
        _activated?.Invoke();
    }

    public int GetNumber() => LevelToNumber(_level);

    public static int LevelToNumber(int level) => (int) Mathf.Pow(NumberBase, level);
    public static int NumberToLevel(int number) => (int) Mathf.Log(number, NumberBase);
}
