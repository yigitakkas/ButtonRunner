using UnityEngine;

public class HandInteraction : MonoBehaviour
{
    private bool _canPress = true; // Tekrar tekrar buton basmayý engellemek için
    private GateInteraction _currentGate; // Geçilen kapýyý takip eder

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
            if (_currentGate != null) return; // Zaten bir kapýnýn içindeyse tekrar iþlem yapma

            GateInteraction gate = other.GetComponent<GateInteraction>();
            if (gate != null)
            {
                _currentGate = gate; // Geçilen kapýyý kaydet
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
                _currentGate.ApplyGateEffect(handMover); // Kapý etkisini uygula
                _currentGate = null; // Kapý etkisi tamamlandýktan sonra sýfýrla
            }
        }
    }

    public void AllowPress()
    {
        _canPress = true;
    }
}
