using UnityEngine;

public class CameraController2D : MonoBehaviour
{
     [Header("Target")]
    [SerializeField] private Transform target;

    [Header("Settings")]
    [SerializeField] private float smoothTime = 0.2f;
    [SerializeField] private Vector2 deadZoneSize = new Vector2(1f, 1f);
    [SerializeField] private float lookAheadDistance = 2f;
    [SerializeField] private float lookAheadSmooth = 0.2f;

    [Header("Level Bounds (optional)")]
    [SerializeField] private bool useBounds = false;
    [SerializeField] private Vector2 minBounds;
    [SerializeField] private Vector2 maxBounds;

    private Vector3 _currentVelocity;
    private Vector3 _lookAhead;
    private Vector3 _lookAheadVelocity;

    private void LateUpdate()
    {
        if (target == null) return;

        Vector3 camPos = transform.position;
        Vector3 targetPos = target.position;
        targetPos.z = camPos.z;

        // --- Dead Zone ---
        Vector2 deadZoneHalf = deadZoneSize / 2f;
        Vector2 offset = target.position - camPos;

        if (Mathf.Abs(offset.x) < deadZoneHalf.x)
            targetPos.x = camPos.x;

        if (Mathf.Abs(offset.y) < deadZoneHalf.y)
            targetPos.y = camPos.y;

        // --- Look Ahead ---
        Vector2 moveDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        Vector3 targetLookAhead = new Vector3(moveDir.x, moveDir.y, 0f) * lookAheadDistance;

        _lookAhead = Vector3.SmoothDamp(_lookAhead, targetLookAhead, ref _lookAheadVelocity, lookAheadSmooth);

        Vector3 desiredPos = targetPos + _lookAhead;

        // --- Smooth follow ---
        Vector3 smoothedPos = Vector3.SmoothDamp(camPos, desiredPos, ref _currentVelocity, smoothTime);

        // --- Bounds ---
        if (useBounds)
        {
            float camHalfHeight = Camera.main.orthographicSize;
            float camHalfWidth = camHalfHeight * Camera.main.aspect;

            smoothedPos.x = Mathf.Clamp(smoothedPos.x, minBounds.x + camHalfWidth, maxBounds.x - camHalfWidth);
            smoothedPos.y = Mathf.Clamp(smoothedPos.y, minBounds.y + camHalfHeight, maxBounds.y - camHalfHeight);
        }

        transform.position = smoothedPos;
    }
}