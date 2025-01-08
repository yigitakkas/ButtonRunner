using UnityEngine;
using System.Collections.Generic;

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
    public float MinY = 0f;
    public float MaxY = -0.75f;
    public AnimationCurve OscillationCurve;

    [Header("Dynamic Attributes")]
    public GameObject HandPrefab;
    public Transform HandsContainer;
    public int Width = 1;
    public int Height = 1;

    [Header("Hand Position Settings")]
    public float HorizontalSpacing = 0.25f;
    public float VerticalSpacing = 0.4f;

    private const int MaxWidth = 5;
    private const int MaxHeight = 5;
    private const float MinOscillationDuration = 0.02f;

    private List<GameObject> _hands = new List<GameObject>();
    private Queue<GameObject> _handPool = new Queue<GameObject>();
    private float _oscillationTimer = 0f;
    private bool _isMovingDown = true;

    private void Start()
    {
        UpdateHands();
    }

    private void Update()
    {
        HandleMovement();
        ApplyVerticalOscillation();
    }

    private void HandleMovement()
    {
        Vector3 forwardMovement = Vector3.forward * ForwardSpeed * Time.deltaTime;
        float lateralInput = GetInput();
        Vector3 lateralMovement = Vector3.right * lateralInput * LateralSpeed * Time.deltaTime;

        transform.Translate(forwardMovement + lateralMovement);

        Vector3 clampedPosition = transform.position;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, MinX, MaxX);
        transform.position = clampedPosition;
    }

    private float GetInput()
    {
        if (Application.isMobilePlatform && Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            return (touch.position.x - Screen.width / 2f) / Screen.width * TouchSensitivity;
        }
        return Input.GetAxis("Horizontal");
    }

    private void ApplyVerticalOscillation()
    {
        _oscillationTimer = (_oscillationTimer + Time.deltaTime / OscillationDuration) % 1f;
        float curveValue = OscillationCurve.Evaluate(_oscillationTimer);
        float newY = Mathf.Lerp(MinY, MaxY, curveValue);

        transform.position = new Vector3(transform.position.x, newY, transform.position.z);

        if (curveValue > 0.5f && !_isMovingDown)
        {
            _isMovingDown = true;
            AllowHandPress();
        }
        else if (curveValue <= 0.5f)
        {
            _isMovingDown = false;
        }
    }

    private void AllowHandPress()
    {
        foreach (Transform hand in HandsContainer)
        {
            HandInteraction interaction = hand.GetComponentInChildren<HandInteraction>();
            if (interaction != null)
            {
                interaction.AllowPress();
            }
        }
    }

    public void ModifyPushRate(int value)
    {
        OscillationDuration = Mathf.Max(MinOscillationDuration, OscillationDuration - value * 0.1f);
        Debug.Log($"Push Rate modified. Oscillation Duration: {OscillationDuration}");
    }

    public void ModifyWidth(int value)
    {
        int newWidth = Mathf.Clamp(Width + value, 1, MaxWidth);

        if (newWidth != Width)
        {
            Width = newWidth;
            UpdateHands();
        }

        Debug.Log($"Width modified to: {Width}");
    }

    public void ModifyHeight(int value)
    {
        int newHeight = Mathf.Clamp(Height + value, 1, MaxHeight);

        if (newHeight != Height)
        {
            Height = newHeight;
            UpdateHands();
        }

        Debug.Log($"Height modified to: {Height}");
    }

    private void UpdateHands()
    {
        int requiredHands = Width * Height;

        while (_hands.Count > requiredHands)
        {
            GameObject hand = _hands[^1];
            _hands.RemoveAt(_hands.Count - 1);
            hand.SetActive(false);
            _handPool.Enqueue(hand);
        }

        for (int i = _hands.Count; i < requiredHands; i++)
        {
            GameObject hand = _handPool.Count > 0 ? _handPool.Dequeue() : Instantiate(HandPrefab, HandsContainer);
            hand.SetActive(true);
            _hands.Add(hand);
        }

        for (int i = 0; i < _hands.Count; i++)
        {
            int column = i % Width;
            int row = i / Width;
            _hands[i].transform.localPosition = CalculateHandPosition(column, row);
        }
    }

    private Vector3 CalculateHandPosition(int column, int row)
    {
        float xOffset = (column - (Width - 1) / 2f) * HorizontalSpacing;
        float zOffset = row * VerticalSpacing;
        return new Vector3(xOffset, 0, zOffset);
    }
}
