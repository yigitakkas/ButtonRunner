using UnityEngine;

public class HandInteraction : MonoBehaviour
{
    private bool _canPress = true; 
    private GateInteraction _currentGate;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Button"))
        {
            if (!_canPress) return;

            ButtonInteraction button = other.GetComponent<ButtonInteraction>();
            if (button != null)
            {
                button.PressButton();
                _canPress = false;
            }
        }

        if (other.CompareTag("Gate"))
        {
            if (_currentGate != null) return;

            GateInteraction gate = other.GetComponent<GateInteraction>();
            if (gate != null)
            {
                _currentGate = gate;
            }
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Gate") && _currentGate != null)
        {
            HandMover handMover = GetComponentInParent<HandMover>();
            if (handMover != null)
            {
                _currentGate.ApplyGateEffect(handMover);
                _currentGate = null;
            }
        }

        if (other.CompareTag("FinishLine"))
        {
            HandleFinish();
        }
    }

    public void AllowPress()
    {
        _canPress = true;
    }

    private void HandleFinish()
    {
        Debug.Log("Finish line reached!");

        GameManager.Instance?.OnLevelComplete();
    }
}
