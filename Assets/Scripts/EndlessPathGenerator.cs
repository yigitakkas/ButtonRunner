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

    private Queue<GameObject[]> _rows;
    private int _rowsOnScreen;
    private float _spawnZ;
    private int _currentRowCount = 0; 
    private bool _finishPlaced = false; 

    private void Start()
    {
        InitializeGrid();
        SpawnInitialRows();
    }

    private void Update()
    {
        CheckAndRecycleRows();
    }

    private void InitializeGrid()
    {
        _rows = new Queue<GameObject[]>();

        float cameraViewDistance = CameraTransform.GetComponent<Camera>().fieldOfView / 2f;
        _rowsOnScreen = Mathf.CeilToInt(cameraViewDistance / VerticalSpacing) + ExtraRows;

        _spawnZ = CameraTransform.position.z;
    }

    /// �lk sat�rlar� olu�turur.
    private void SpawnInitialRows()
    {
        for (int i = 0; i < _rowsOnScreen; i++)
        {
            SpawnRow();
        }
    }

    /// Kamera pozisyonuna g�re sat�r geri d�n���m� yapar.
    private void CheckAndRecycleRows()
    {
        if (_finishPlaced) return; // Finish �izgisi yerle�tirildiyse devam etmeyin

        if (CameraTransform.position.z > _spawnZ - (_rowsOnScreen * VerticalSpacing))
        {
            if (_currentRowCount < MaxRows)
            {
                RecycleRow();
            }
            else if (!_finishPlaced)
            {
                PlaceFinishLine(); // Finish �izgisini yerle�tir
                _finishPlaced = true;
            }
        }
    }

    /// Yeni bir sat�r olu�turur.
    private void SpawnRow()
    {
        if (_currentRowCount >= MaxRows) return;

        GameObject[] newRow = new GameObject[Columns];

        for (int x = 0; x < Columns; x++)
        {
            float xPosition = x * HorizontalSpacing - (Columns - 1) * HorizontalSpacing / 2;
            Vector3 spawnPosition = new Vector3(xPosition, -1, _spawnZ);

            GameObject newButton = Instantiate(ButtonPrefab, spawnPosition, ButtonPrefab.transform.rotation);
            newButton.transform.SetParent(transform);
            newRow[x] = newButton;
        }

        _rows.Enqueue(newRow);
        _spawnZ += VerticalSpacing;
        _currentRowCount++;
    }

    /// En eski sat�r� geri d�n��t�rerek sona ta��r.
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

    /// Finish �izgisini yerle�tirir.
    private void PlaceFinishLine()
    {
        Vector3 finishPosition = new Vector3(0, -0.99f, _spawnZ);
        Instantiate(FinishPrefab, finishPosition, FinishPrefab.transform.rotation, transform);
    }
}
