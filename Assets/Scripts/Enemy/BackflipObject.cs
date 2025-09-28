using DG.Tweening;
using UnityEngine;

public class BackflipObject : MonoBehaviour
{
    [Header("Settings")] [SerializeField] private float flipDistance = 5f;
    [SerializeField] private float flipDuration = 2f;
    [SerializeField] private int numberOfFlips = 4;
    [SerializeField] private EnemyStateMachine stateMachine;
    [Header("Easing")] [SerializeField] private Ease movementEase = Ease.OutQuad;
    [SerializeField] private Ease rotationEase = Ease.Linear;

    public float FlipDuration => flipDuration;

    [ContextMenu("Execute Backflip")]
    public void ExecuteBackflip()
    {
        ExecuteBackflip(flipDistance, flipDuration);
    }

    public void ExecuteBackflip(float distance, float duration)
    {
        var backwardDirection = (transform.position - stateMachine.Target.position).normalized;

        var targetPosition = transform.position + backwardDirection * distance;
        var jumpHeight = 2f; // высота дуги
        var totalRotation = 360f * numberOfFlips;

        var flipSequence = DOTween.Sequence();

        // Полёт по дуге
        flipSequence.Join(transform.DOJump(targetPosition, jumpHeight, 1, duration)
            .SetEase(movementEase));

        // Вращение
        flipSequence.Join(transform.DORotate(new Vector3(0, 0, totalRotation), duration, RotateMode.FastBeyond360)
            .SetEase(rotationEase));

        // Берём исходный скейл, чтобы не ломать зеркалку
        Vector3 baseScale = transform.localScale;

        // Squash & Stretch (меняем только Y, X берем из baseScale.x)
        flipSequence.Insert(0f, transform.DOScale(new Vector3(baseScale.x * 1.2f, baseScale.y * 0.8f, baseScale.z), 0.2f)
            .SetLoops(2, LoopType.Yoyo));

        // При приземлении слегка потрясём
        flipSequence.AppendCallback(() =>
        {
            transform.DOShakePosition(0.3f, 0.3f, 10, 90, false, true);
            transform.DOScale(baseScale, 0.1f); // вернем скейл точно в норму
        });

        flipSequence.SetAutoKill(true);
    }

    void OnDestroy()
    {
        if (DOTween.instance != null)
            DOTween.Kill(transform);
    }
}