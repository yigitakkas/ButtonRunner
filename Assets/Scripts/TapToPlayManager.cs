using UnityEngine;
using UnityEngine.UI;

public class TapToPlayManager : MonoBehaviour
{
    public GameObject TapToPlayUI;
    public HandMover HandMover;

    private bool _isGameStarted = false;

    private void Start()
    {
        HandMover.enabled = false;
    }

    private void Update()
    {
        if (!_isGameStarted && Input.GetMouseButtonDown(0))
        {
            StartGame();
        }
    }

    private void StartGame()
    {
        _isGameStarted = true;

        TapToPlayUI.GetComponent<Animator>().enabled = false;
        TapToPlayUI.SetActive(false);

        HandMover.enabled = true;
    }
}
