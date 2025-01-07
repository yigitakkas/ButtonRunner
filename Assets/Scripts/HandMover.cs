using UnityEngine;

public class HandMover : MonoBehaviour
{
    [Header("Movement Settings")]
    public float ForwardSpeed = 4f;
    public float LateralSpeed = 5f;
    public float TouchSensitivity = 0.001f;
    public float MinX = -3.5f;
    public float MaxX = 3.5f;

    [Header("Vertical Oscillation Settings")]
    public float OscillationDuration = 1f;
    public float MinY = 0.5f;
    public float MaxY = 1f;  

    public AnimationCurve OscillationCurve;

    private float _oscillationTimer = 0f; 
    private Vector2 _touchStartPosition;

    private void Update()
    {
        MoveForward();
        HandleInput();
        ApplyVerticalOscillation();
    }

    private void MoveForward()
    {
        transform.Translate(Vector3.forward * ForwardSpeed * Time.deltaTime);
    }

    private void HandleInput()
    {
        if (Application.isMobilePlatform)
        {
            HandleTouchInput();
        }
        else
        {
            HandleKeyboardInput();
        }
    }

    private void HandleTouchInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    _touchStartPosition = touch.position;
                    break;

                case TouchPhase.Moved:
                case TouchPhase.Stationary:
                    float normalizedDeltaX = (touch.position.x - _touchStartPosition.x) / Screen.width;
                    float moveX = normalizedDeltaX * LateralSpeed * TouchSensitivity;
                    MoveLateral(moveX);
                    break;
            }
        }
    }

    private void HandleKeyboardInput()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        MoveLateral(horizontalInput * LateralSpeed * Time.deltaTime);
    }

    private void MoveLateral(float amount)
    {
        transform.Translate(Vector3.right * amount);

        Vector3 clampedPosition = transform.position;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, MinX, MaxX);
        transform.position = clampedPosition;
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
    }
}
