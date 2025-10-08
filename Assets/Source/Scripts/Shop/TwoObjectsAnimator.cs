using DG.Tweening;
using UnityEngine;

public class TwoObjectsAnimator : MonoBehaviour
{
    [Header("Objects")]
    [SerializeField] private Transform object1;
    [SerializeField] private Transform object2;

    [Header("Show Timings")]
    [SerializeField] private float object1AppearTime = 0.4f;
    [SerializeField] private float object2AppearTime = 0.25f;

    [Header("Pulse Settings")]
    [SerializeField] private float pulseMultiplier = 1.05f;
    [SerializeField] private float pulseDuration = 0.5f;

    [Header("Hide Timings")]
    [SerializeField] private float explodeDuration = 0.35f;
    [SerializeField] private float hideDuration = 0.25f;
    [SerializeField] private float explodeScaleMultiplier = 2f;

    private Vector3 object1StartScale;
    private Vector3 object2StartScale;
    private Tween pulseTween;

    private void Start()
    {
        PlayAnimation();
    }

    public void PlayAnimation()
    {
        object1StartScale = object1.localScale;
        object2StartScale = object2.localScale;

        object1.localScale = Vector3.zero;
        object2.localScale = Vector3.zero;

        Sequence seq = DOTween.Sequence();
        seq.Append(object1.DOScale(object1StartScale, object1AppearTime)
                          .SetEase(Ease.OutBack));
        
        seq.Append(object2.DOScale(object2StartScale, object2AppearTime)
                          .SetEase(Ease.OutBounce));
        seq.OnComplete(() =>
        {
            pulseTween = object2.DOScale(object2StartScale * pulseMultiplier, pulseDuration)
                                .SetLoops(-1, LoopType.Yoyo)
                                .SetEase(Ease.InOutSine);
        });
    }
    
    public void HideWithExplosion()
    {
        pulseTween?.Kill();

        ExplodeObject(object1, object1StartScale);
        ExplodeObject(object2, object2StartScale);
    }

    private void ExplodeObject(Transform target, Vector3 baseScale)
    {
        target.DOScale(baseScale * explodeScaleMultiplier, explodeDuration * 0.7f)
              .SetEase(Ease.OutQuad)
              .OnComplete(() =>
              {
                  target.DOScale(0f, explodeDuration * 0.3f)
                        .SetEase(Ease.InQuad);
              });
    }
    
    public void HideSimply()
    {
        pulseTween?.Kill();

        object1.DOScale(0f, hideDuration).SetEase(Ease.InSine);
        object2.DOScale(0f, hideDuration).SetEase(Ease.InSine);
    }
}