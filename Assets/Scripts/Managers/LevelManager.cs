using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private string _nextLevelName = "Main";
    
    public void NextLevel()
    {
        SceneManager.LoadScene(_nextLevelName, LoadSceneMode.Single);
    } 

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
    }
}
