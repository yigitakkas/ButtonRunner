using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject EndGameUI;
    private bool _isGameCompleted = false;
    [SerializeField] private HandMover _handMover;

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

        if (EndGameUI != null)
        {
            EndGameUI.SetActive(true);
        }
    }

    private void StopPlayerMovement()
    {
        if (_handMover != null)
        {
            Debug.Log("handmover is not null");
            _handMover.enabled = false;
        }
    }

    public void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex); // Bir sonraki sahneyi yükle
        }
        else
        {
            SceneManager.LoadScene("FirstLevel"); //Ýlk bölüm
        }
    }
}
