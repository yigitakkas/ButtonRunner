using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("UI Elements")]
    public GameObject EndGameUI;

    [Header("Level Settings")]
    [SerializeField] private string firstLevelSceneName = "FirstLevel";

    private bool _isGameCompleted = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void OnLevelComplete()
    {
        if (_isGameCompleted) return;
        _isGameCompleted = true;

        Debug.Log("Level Complete! Stopping player movement and showing end screen.");

        StopPlayerMovement();
        NotifyUI();
    }

    private void StopPlayerMovement()
    {
        HandMover handMover = FindObjectOfType<HandMover>();
        if (handMover != null)
        {
            handMover.StopMovement();
        }
    }

    private void NotifyUI()
    {
        if (EndGameUI != null)
        {
            EndGameUI.SetActive(true);
        }
    }

    public void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            SceneManager.LoadScene(firstLevelSceneName);
        }
    }
}
