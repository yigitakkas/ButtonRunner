using UnityEngine;
using System.Collections;
using TMPro;

public class ButtonInteraction : MonoBehaviour
{
    private bool _isPressed = false;
    private Vector3 _originalPosition;
    private int _amount = 0;
    private MeshRenderer _meshRenderer; // MeshRenderer referansý
    private Color _originalColor;

    public TMP_Text ButtonText;
    public float PressDepth = 0.2f;
    public float AnimationSpeed = 5f;


    private void Start()
    {
        _originalPosition = transform.position;
        _meshRenderer = GetComponent<MeshRenderer>();
        _originalColor = _meshRenderer.material.color;
    }

    public void PressButton()
    {
        //if (_isPressed) return;

        //_isPressed = true;

        _amount++;
        ButtonText.text = _amount.ToString();

        ChangeToGreen();

        StopAllCoroutines();
        StartCoroutine(AnimatePress());
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

        _isPressed = false;
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
        _meshRenderer.material.color = _originalColor;
    }
}
