using UnityEngine;

public class ReadingBook : MonoBehaviour
{
    public float distance = 0.5f;
    public float heightOffset = -0.1f;

        private void LateUpdate()
    {
        Transform cam = Camera.main.transform;
        transform.position = cam.position + cam.forward * distance + Vector3.up * heightOffset;
        transform.rotation = Quaternion.LookRotation(transform.position - cam.position);
    }
}
