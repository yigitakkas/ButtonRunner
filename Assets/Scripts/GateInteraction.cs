using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GateInteraction : MonoBehaviour
{
    public enum GateType { PushRate, Width, Height }
    public GateType gateType;
    public int Value = 1;
    public Image Headline;
    public SpriteRenderer Effector;
    public TMP_Text ValueText;

    private bool _isTriggered = false;

    public AudioClip GateSound;
    public GameObject GateEffectPrefab;

    private void Awake()
    {
        if (Value > 0)
        {
            Headline.color = Color.green;
            Effector.color = Color.green;
            ValueText.text = "+" + Value.ToString();
        }
        else
        {
            Headline.color = Color.red;
            Effector.color = Color.red;
            ValueText.text = Value.ToString();
        }
    }

    public void ApplyGateEffect(HandMover handMover)
    {
        if (_isTriggered) return;

        ApplyGateFX();

        _isTriggered = true;
        Headline.color = Color.gray;
        Effector.color = Color.gray;

        switch (gateType)
        {
            case GateType.PushRate:
                handMover.ModifyPushRate(Value);
                break;

            case GateType.Width:
                handMover.ModifyWidth(Value);
                break;

            case GateType.Height:
                handMover.ModifyHeight(Value);
                break;
        }

        Debug.Log($"{gateType} Gate triggered with value: {Value}");
    }

    private void ApplyGateFX()
    {
        if (GateSound != null)
        {
            EffectManager.Instance.PlaySound(GateSound);
        }

        if (GateEffectPrefab != null)
        {
            EffectManager.Instance.PlayGateEffect(transform.position);
        }
    }
}
