using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour //TODO Delete. Full rework with state manager
{
    [SerializeField] private LevelCompleteUI levelCompleteUI;
    
    private bool _levelCompleted;

    private void Update()
    {
        if (_levelCompleted) return;

        if (BulletTarget.AliveCount <= 0)
        {
            CompleteLevel();
        }
    }

    private void CompleteLevel()
    {
        FindAnyObjectByType<InputSystem.InputsManager>()?.gameObject.SetActive(false);//BAD 
        
        levelCompleteUI.Show();
        
        _levelCompleted = true;

        Time.timeScale = 0f;

        Debug.Log("LEVEL COMPLETE");
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);//BAD
    }
}