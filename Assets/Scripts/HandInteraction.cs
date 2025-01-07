using UnityEngine;

public class HandInteraction : MonoBehaviour
{
    private bool _canPress = true;

    private void OnTriggerEnter(Collider other)
    {
        if (!_canPress) return;

        if (other.CompareTag("Button"))
        {
            ButtonInteraction button = other.GetComponent<ButtonInteraction>();
            if (button != null)
            {
                button.PressButton();
                _canPress = false;
            }
        }
    }

    public void AllowPress()
    {
        _canPress = true;
    }
}
