using UnityEngine;
using System.Collections;
using TMPro;

public class ButtonInteraction : MonoBehaviour
{
    private Vector3 _originalPosition;
    private int _amount = 0;
    private MeshRenderer _meshRenderer;
    private Color _originalColor;

    public TMP_Text ButtonText;
    public float PressDepth = 0.15f;
    public float AnimationSpeed = 6f;

    public AudioClip ClickSound;

    private void Awake()
    {
        _originalPosition = transform.position;
        _meshRenderer = GetComponent<MeshRenderer>();
        _originalColor = _meshRenderer.material.color;
    }
    private void Start()
    {
    }

    public void PressButton()
    {
        TriggerEffects();

        _amount++;
        ButtonText.text = _amount.ToString();

        ChangeToGreen();

        StartCoroutine(AnimatePress());
    }

    private void TriggerEffects()
    {
        if (ClickSound != null)
        {
            EffectManager.Instance.PlaySound(ClickSound);
        }

        EffectManager.Instance.PlayButtonEffect(transform.position);
    }

    private IEnumerator AnimatePress()
    {
        Vector3 pressedPosition = _originalPosition - new Vector3(0, PressDepth, 0);
        while (Vector3.Distance(transform.position, pressedPosition) > 0.01f)
        {
            transform.position = Vector3.Lerp(transform.position, pressedPosition, AnimationSpeed * Time.deltaTime);
            yield return null;
        }

        yield return new WaitForSeconds(0.1f);

        while (Vector3.Distance(transform.position, _originalPosition) > 0.01f)
        {
            transform.position = Vector3.Lerp(transform.position, _originalPosition, AnimationSpeed * Time.deltaTime);
            yield return null;
        }
    }

    public void UpdateOriginalPosition(Vector3 newPosition)
    {
        _originalPosition = newPosition;
        transform.position = newPosition;
    }

    public void ResetText()
    {
        _amount = 0;
        ButtonText.text = "";
    }

    public void ChangeToGreen()
    {
        if (_meshRenderer != null)
        {
            _meshRenderer.material.color = Color.green;
        }
    }

    public void ResetColor()
    {
        if (_meshRenderer != null)
        {
            _meshRenderer.material.color = _originalColor;
        }
    }
}
