using UnityEngine;
using System.Collections.Generic;

public class EndlessPathGenerator : MonoBehaviour
{
    [Header("Button Grid Settings")]
    public GameObject ButtonPrefab;
    public GameObject FinishPrefab;
    public Transform CameraTransform;
    [Range(1, 20)] public int Columns = 7;
    [Range(0, 30)] public int ExtraRows = 30;
    public float HorizontalSpacing = 1.1f;
    public float VerticalSpacing = 1.1f;

    [Header("Level Settings")]
    public int MaxRows = 100;

    private Queue<GameObject[]> _rows = new Queue<GameObject[]>();
    private int _rowsOnScreen;
    private float _spawnZ;
    private int _currentRowCount = 0;
    private bool _finishPlaced = false;

    private const float ButtonHeight = -0.99f;

    private void Start()
    {
        InitializeGrid();
        SpawnInitialRows();
    }

    private void Update()
    {
        if (!_finishPlaced)
        {
            CheckAndRecycleRows();
        }
    }

    private void InitializeGrid()
    {
        float cameraViewDistance = CameraTransform.GetComponent<Camera>().fieldOfView / 2f;
        _rowsOnScreen = Mathf.CeilToInt(cameraViewDistance / VerticalSpacing) + ExtraRows;
        _spawnZ = CameraTransform.position.z;
    }

    private void SpawnInitialRows()
    {
        for (int i = 0; i < _rowsOnScreen; i++)
        {
            SpawnRow();
        }
    }

    private void CheckAndRecycleRows()
    {
        if (CameraTransform.position.z > _spawnZ - (_rowsOnScreen * VerticalSpacing))
        {
            if (_currentRowCount < MaxRows)
            {
                RecycleRow();
            }
            else
            {
                PlaceFinishLine();
                _finishPlaced = true;
            }
        }
    }

    private void SpawnRow()
    {
        if (_currentRowCount >= MaxRows) return;

        GameObject[] newRow = new GameObject[Columns];
        for (int x = 0; x < Columns; x++)
        {
            float xPosition = x * HorizontalSpacing - (Columns - 1) * HorizontalSpacing / 2;
            Vector3 spawnPosition = new Vector3(xPosition, ButtonHeight, _spawnZ);

            GameObject newButton = Instantiate(ButtonPrefab, spawnPosition, ButtonPrefab.transform.rotation, transform);
            newRow[x] = newButton;
        }

        _rows.Enqueue(newRow);
        _spawnZ += VerticalSpacing;
        _currentRowCount++;
    }

    private void RecycleRow()
    {
        if (_rows.Count == 0) return;

        GameObject[] oldRow = _rows.Dequeue();
        foreach (GameObject button in oldRow)
        {
            if (button != null)
            {
                Vector3 newPosition = button.transform.position;
                newPosition.z = _spawnZ;

                button.transform.position = newPosition;

                ButtonInteraction buttonInteraction = button.GetComponent<ButtonInteraction>();
                if (buttonInteraction != null)
                {
                    buttonInteraction.StopAllCoroutines();
                    buttonInteraction.UpdateOriginalPosition(newPosition);
                    buttonInteraction.ResetText();
                    buttonInteraction.ResetColor();
                }
            }
        }

        _rows.Enqueue(oldRow);
        _spawnZ += VerticalSpacing;
        _currentRowCount++;
    }

    private void PlaceFinishLine()
    {
        Vector3 finishPosition = new Vector3(0, ButtonHeight, _spawnZ);
        Instantiate(FinishPrefab, finishPosition, FinishPrefab.transform.rotation, transform);
    }
}
