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
    Lost
}
