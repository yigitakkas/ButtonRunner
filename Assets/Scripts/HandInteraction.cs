using UnityEngine;

public class HandInteraction : MonoBehaviour
{
    private bool _canPress = true; // Tekrar tekrar basmayý engellemek için

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
            GateInteraction gate = other.GetComponent<GateInteraction>();
            if (gate != null)
            {
                HandMover handMover = GetComponentInParent<HandMover>();
                if (handMover != null)
                {
                    gate.ApplyGateEffect(handMover); 
                }
            }
        }
    }

    public void AllowPress()
    {
        _canPress = true;
    }
}
