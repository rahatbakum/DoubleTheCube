using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;
    private int _score = 0;
    public int Score => _score;
    private GameState _currentGameState = GameState.Playing;
    public GameState CurrentGameState => _currentGameState;
    [SerializeField] private UnityEvent<int> _scoreChanged;
    public event UnityAction<int> ScoreChanged
    {
        add => _scoreChanged.AddListener(value);
        remove => _scoreChanged.RemoveListener(value);
    }
    [SerializeField] private UnityEvent _won;
    public event UnityAction Won
    {
        add => _won.AddListener(value);
        remove => _won.RemoveListener(value);
    }
    [SerializeField] private UnityEvent _lost;
    public event UnityAction Lost
    {
        add => _lost.AddListener(value);
        remove => _lost.RemoveListener(value);
    }
    [SerializeField] private UnityEvent _paused;
    public event UnityAction Paused
    {
        add => _paused.AddListener(value);
        remove => _paused.RemoveListener(value);
    }
    [SerializeField] private UnityEvent _unpaused;
    public event UnityAction Unpaused
    {
        add => _unpaused.AddListener(value);
        remove => _unpaused.RemoveListener(value);
    }
    private float _defaultTimeScale;
    

    public void Win()
    {
        if(_currentGameState == GameState.Playing)
        {
            _currentGameState = GameState.Won;
            _won.Invoke();
        }
    }

    public void Lose()
    {
        if(_currentGameState == GameState.Playing)
        {
            _currentGameState = GameState.Lost;
            _lost.Invoke();
        }
    }

    public void Pause()
    {
        if(_currentGameState != GameState.Playing)
            return;
        _defaultTimeScale = Time.timeScale;
        Time.timeScale = 0f;
        _currentGameState = GameState.Paused;
        _paused.Invoke();
    }

    public void Unpause()
    {
        if(_currentGameState != GameState.Paused)
            return;
        Time.timeScale = _defaultTimeScale;
        _currentGameState = GameState.Playing;
        _unpaused.Invoke();
    }

    public void AddScore(int value)
    {
        ChangeScore(_score + Mathf.Abs(value));  
    }
    

    private void ChangeScore(int newScore)
    {
        _score = newScore;
        _scoreChanged.Invoke(_score);
    }

    private void Awake()
    {
        if(Instance != null)
            throw new System.Exception("Here is more than one GameManager in the scene");
        else
            Instance = this;
    }
}

public enum GameState
{
    Playing,
    Won,
    Lost,
    Paused
}
