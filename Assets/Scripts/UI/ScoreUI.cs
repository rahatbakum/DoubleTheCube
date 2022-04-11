using UnityEngine;
using UnityEngine.UI;

public class ScoreUI : MonoBehaviour
{
    [SerializeField] private Text _text;
    public void OnScoreChanged(int score)
    {
        _text.text = score.ToString();
    }
}
