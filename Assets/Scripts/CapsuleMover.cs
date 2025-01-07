using UnityEngine;

public class CapsuleMover : MonoBehaviour
{
    [Header("Movement Settings")]
    public float ForwardSpeed = 4f;
    public float LateralSpeed = 5f;
    public float TouchSensitivity = 0.001f;
    public float MinX = -3.7f;
    public float MaxX = 3.7f;

    private Vector2 _touchStartPosition;

    private void Update()
    {
        MoveForward();
        HandleInput();
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
                    // Hareket miktarýný ekran geniþliðine göre normalize et
                    float normalizedDeltaX = (touch.position.x - _touchStartPosition.x) / Screen.width;

                    // Hareketi hassasiyete ve hýz katsayýsýna göre uygula
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
}
