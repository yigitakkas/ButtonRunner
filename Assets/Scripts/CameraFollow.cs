using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform Target;
    public Vector3 Offset = new Vector3(0, 10, -10);
    public float SmoothSpeed = 0.125f;
    public float FixedRotationX = 40f;

    private void LateUpdate()
    {
        if (Target == null) return;

        Vector3 desiredPosition = Target.position + Offset;
        desiredPosition.y = 10f;

        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, SmoothSpeed);
        smoothedPosition.y = 10f;

        transform.position = smoothedPosition;

        Vector3 lookDirection = Target.position - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(lookDirection);

        Vector3 fixedRotation = targetRotation.eulerAngles;
        fixedRotation.x = FixedRotationX;

        transform.rotation = Quaternion.Euler(fixedRotation);
    }
}
