using UnityEngine;

public class HandMover : MonoBehaviour
{
    public HandInteraction HandInteraction;

    [Header("Movement Settings")]
    public float ForwardSpeed = 4f;
    public float LateralSpeed = 5f;
    public float TouchSensitivity = 0.001f;
    public float MinX = -3.5f;
    public float MaxX = 3.5f;

    [Header("Vertical Oscillation Settings")]
    public float OscillationDuration = 1f; //
    public float MinY = 0f;
    public float MaxY = -0.75f;

    public AnimationCurve OscillationCurve;

    [Header("Dynamic Attributes")]
    public int Width = 1;
    public int Height = 1;

    private const float MinOscillationDuration = 0.02f;
    private const int MaxWidth = 5;
    private const int MaxHeight = 5;

    private float _oscillationTimer = 0f;
    private Vector2 _touchStartPosition;
    private bool _isMovingDown = true;

    private void Update()
    {
        HandleMovement();
        ApplyVerticalOscillation();
    }

    private void HandleMovement()
    {
        Vector3 forwardMovement = Vector3.forward * ForwardSpeed * Time.deltaTime;

        float lateralInput = Application.isMobilePlatform ? GetTouchInput() : GetKeyboardInput();
        Vector3 lateralMovement = Vector3.right * lateralInput * LateralSpeed * Time.deltaTime;

        Vector3 combinedMovement = forwardMovement + lateralMovement;
        if (combinedMovement.magnitude > 0)
        {
            combinedMovement = combinedMovement.normalized * ForwardSpeed * Time.deltaTime;
        }

        transform.Translate(combinedMovement);

        Vector3 clampedPosition = transform.position;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, MinX, MaxX);
        transform.position = clampedPosition;
    }

    private float GetTouchInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
            {
                float normalizedDeltaX = (touch.position.x - _touchStartPosition.x) / Screen.width;
                return normalizedDeltaX * TouchSensitivity;
            }
        }
        return 0f;
    }

    private float GetKeyboardInput()
    {
        return Input.GetAxis("Horizontal");
    }

    private void ApplyVerticalOscillation()
    {
        _oscillationTimer += Time.deltaTime / OscillationDuration;
        if (_oscillationTimer > 1f)
        {
            _oscillationTimer = 0f;
        }

        float curveValue = OscillationCurve.Evaluate(_oscillationTimer);
        float newY = Mathf.Lerp(MinY, MaxY, curveValue);

        Vector3 position = transform.position;
        position.y = newY;
        transform.position = position;

        if (curveValue > 0.5f)
        {
            if (!_isMovingDown)
            {
                _isMovingDown = true;
                HandInteraction.AllowPress();
            }
        }
        else
        {
            _isMovingDown = false;
        }
    }

    // Yeni özellik artýrma/azaltma metodlarý
    public void ModifyPushRate(int value)
    {
        OscillationDuration = Mathf.Max(MinOscillationDuration, OscillationDuration - value * 0.1f); // Basma süresini dinamik olarak düzenle
        Debug.Log($"Oscillation Duration modified to: {OscillationDuration}");
    }

    public void ModifyWidth(int value)
    {
        Width = Mathf.Clamp(Width + value, 1, MaxWidth); // El geniþliðini artýr/azalt, sýnýrla
        Debug.Log($"Width modified to: {Width}");
        UpdateWidth();
    }

    public void ModifyHeight(int value)
    {
        Height = Mathf.Clamp(Height + value, 1, MaxHeight); // El yüksekliðini artýr/azalt, sýnýrla
        Debug.Log($"Height modified to: {Height}");
        UpdateHeight();
    }

    private void UpdateWidth()
    {
        Debug.Log("Width updated!");
    }

    private void UpdateHeight()
    {
        Debug.Log("Height updated!");
    }
}
