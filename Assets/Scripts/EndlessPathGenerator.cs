using UnityEngine;
using System.Collections.Generic;

public class OptimizedButtonGrid : MonoBehaviour
{
    [Header("Button Grid Settings")]
    public GameObject ButtonPrefab;
    public Transform CameraTransform;
    [Range(1, 20)] public int Columns = 7; 
    [Range(0, 30)] public int ExtraRows = 30; 
    public float HorizontalSpacing = 1.1f; 
    public float VerticalSpacing = 1.1f; 

    private Queue<GameObject[]> _rows; 
    private int _rowsOnScreen;
    private float _spawnZ;

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

    /// Ýlk satýrlarý oluþturur.
    private void SpawnInitialRows()
    {
        for (int i = 0; i < _rowsOnScreen; i++)
        {
            SpawnRow();
        }
    }

    /// Kamera pozisyonuna göre satýr geri dönüþümü yapar.
    private void CheckAndRecycleRows()
    {
        if (CameraTransform.position.z > _spawnZ - (_rowsOnScreen * VerticalSpacing))
        {
            RecycleRow();
        }
    }

    /// Yeni bir satýr oluþturur.
    private void SpawnRow()
    {
        GameObject[] newRow = new GameObject[Columns];

        for (int x = 0; x < Columns; x++)
        {
            // Satýrdaki butonun pozisyonunu hesapla
            float xPosition = x * HorizontalSpacing - (Columns - 1) * HorizontalSpacing / 2;
            Vector3 spawnPosition = new Vector3(xPosition, -1, _spawnZ);

            // Yeni butonu oluþtur
            GameObject newButton = Instantiate(ButtonPrefab, spawnPosition, ButtonPrefab.transform.rotation);
            newButton.transform.SetParent(transform);
            newRow[x] = newButton;
        }

        _rows.Enqueue(newRow);
        _spawnZ += VerticalSpacing;
    }

    /// En eski satýrý geri dönüþtürerek sona taþýr.
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
            }
        }

        _rows.Enqueue(oldRow);
        _spawnZ += VerticalSpacing;
    }
}
