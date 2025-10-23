using UnityEngine;

public class CurvedProjectile : MonoBehaviour
{
    [Header("Runtime target (устанавливается из спавнера)")]
    public Transform target;

    private float speed => paramSystem.ProjectileSpeed;

    public float amplitude = 0.5f;
    public float frequency = 2f;
    public float hitRadius = 0.5f;

    private Vector3 startPos;
    private Vector3 targetPosAtStart;
    private float flightDuration;
    private float elapsed = 0f;
    private Vector3 lateral;
    private float phase;
    private CharacterParamSystem paramSystem;

    public void Initialize(CharacterParamSystem characterParamSystem,Transform target, float amplitude, float frequency)
    {
        paramSystem = characterParamSystem;
        this.target = target;
        this.amplitude = amplitude;
        this.frequency = Mathf.Max(0.01f, frequency);

        startPos = transform.position;
        targetPosAtStart = target != null ? target.position : startPos + transform.forward * 10f;

        float distance = Vector3.Distance(startPos, targetPosAtStart);
        flightDuration = Mathf.Max(0.05f, distance / this.speed);
        Vector3 forward = (targetPosAtStart - startPos).normalized;
        Vector3 up = Vector3.up;
        if (Mathf.Abs(Vector3.Dot(forward, up)) > 0.99f) up = Vector3.right;
        lateral = Vector3.Cross(forward, up).normalized;

        phase = Random.Range(0f, Mathf.PI * 2f);
        transform.forward = forward;
    }

    void Update()
    {
        if (target == null)
        {
            MoveToStaticTarget();
            return;
        }

        elapsed += Time.deltaTime;
        var t = Mathf.Clamp01(elapsed / flightDuration);
        var currentTargetPos = target.position;
        var linePos = Vector3.Lerp(startPos, currentTargetPos, t);
        var easedAmp = amplitude * (1f - t);
        var curve = Mathf.Sin(t * Mathf.PI * frequency + phase);
        var offset = lateral * curve * easedAmp;
        transform.position = linePos + offset;
        var lookAheadT = Mathf.Clamp01((elapsed + 0.02f) / flightDuration);
        var futureLinePos = Vector3.Lerp(startPos, currentTargetPos, lookAheadT);
        var futureOffset = lateral * Mathf.Sin(lookAheadT * Mathf.PI * frequency + phase) * amplitude *
                           (1f - lookAheadT);
        var lookDir = (futureLinePos + futureOffset) - transform.position;
        if (lookDir.sqrMagnitude > 0.0001f) transform.forward = lookDir.normalized;
        if (Vector3.Distance(transform.position, currentTargetPos) <= hitRadius || t >= 1f)
        {
            OnHit();
        }

        transform.rotation = Quaternion.identity;
    }

    void MoveToStaticTarget()
    {
        elapsed += Time.deltaTime;
        float t = Mathf.Clamp01(elapsed / flightDuration);
        Vector3 linePos = Vector3.Lerp(startPos, targetPosAtStart, t);
        float easedAmp = amplitude * (1f - t);
        float curve = Mathf.Sin(t * Mathf.PI * frequency + phase);
        Vector3 offset = lateral * curve * easedAmp;
        transform.position = linePos + offset;

        if (t >= 1f)
        {
            OnHit();
        }
    }

    void OnHit()
    {
        Destroy(gameObject);
        target?.GetComponent<EnemyStateMachine>().TakeDamage(paramSystem.AttachDamage);
    }
}