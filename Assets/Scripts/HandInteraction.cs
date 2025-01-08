using UnityEngine;

public class HandInteraction : MonoBehaviour
{
    private bool _canPress = true; // Tekrar tekrar buton basmay� engellemek i�in
    private GateInteraction _currentGate; // Ge�ilen kap�y� takip eder

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
            if (_currentGate != null) return; // Zaten bir kap�n�n i�indeyse tekrar i�lem yapma

            GateInteraction gate = other.GetComponent<GateInteraction>();
            if (gate != null)
            {
                _currentGate = gate; // Ge�ilen kap�y� kaydet
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
                _currentGate.ApplyGateEffect(handMover); // Kap� etkisini uygula
                _currentGate = null; // Kap� etkisi tamamland�ktan sonra s�f�rla
            }
        }
    }

    public void AllowPress()
    {
        _canPress = true;
    }
}
