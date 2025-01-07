using UnityEngine;

public class PlaneFollower : MonoBehaviour
{
    public Transform CameraTransform;
    public float OffsetZ = -10f; 
    public float FollowSpeed = 5f; 

    private void Update()
    {
        Vector3 targetPosition = new Vector3(transform.position.x, transform.position.y, CameraTransform.position.z + OffsetZ);

        transform.position = Vector3.Lerp(transform.position, targetPosition, FollowSpeed * Time.deltaTime);
    }
}
