using UnityEngine;

public class HandInteraction : MonoBehaviour
{
    private bool _canPress = true; 
    private GateInteraction _currentGate;

    private void OnTriggerEnter(Collider other)
    {
        HandleCollision(other, false);
    }

    private void OnTriggerExit(Collider other)
    {
        HandleCollision(other, true);
    }

    private void HandleCollision(Collider other, bool isExiting)
    {
        switch (other.tag)
        {
            case "Button":
                if (!isExiting) HandleButtonInteraction(other);
                break;

            case "Gate":
                if (isExiting) HandleGateExit(other);
                else HandleGateEnter(other);
                break;

            case "FinishLine":
                if (isExiting) HandleFinish();
                break;
        }
    }

    private void HandleButtonInteraction(Collider other)
    {
        if (!_canPress) return;

        ButtonInteraction button = other.GetComponent<ButtonInteraction>();
        if (button != null)
        {
            button.PressButton();
            _canPress = false;
        }
    }

    private void HandleGateEnter(Collider other)
    {
        if (_currentGate != null) return;

        GateInteraction gate = other.GetComponent<GateInteraction>();
        if (gate != null)
        {
            _currentGate = gate;
        }
    }

    private void HandleGateExit(Collider other)
    {
        if (_currentGate != null)
        {
            HandMover handMover = GetComponentInParent<HandMover>();
            if (handMover != null)
            {
                _currentGate.ApplyGateEffect(handMover);
                _currentGate = null;
            }
        }
    }
    private void HandleFinish()
    {
        Debug.Log("Finish line reached!");

        GameManager.Instance?.OnLevelComplete();
    }

    public void AllowPress()
    {
        _canPress = true;
    }

}
