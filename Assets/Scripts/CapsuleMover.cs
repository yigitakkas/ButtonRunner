using UnityEngine;

public class CapsuleMover : MonoBehaviour
{
    public float MoveSpeed = 5f;

    private void Update()
    {
        transform.Translate(Vector3.forward * MoveSpeed * Time.deltaTime);
    }
}
