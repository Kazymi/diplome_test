using UnityEngine;

public class PetAnimatorController
{
    private readonly Animator animator;
    private static readonly int Idle = Animator.StringToHash("Idle");
    private static readonly int Walk = Animator.StringToHash("Walk");
    private static readonly int Attack = Animator.StringToHash("Attack");
    private static readonly int Spawn = Animator.StringToHash("Spawn");

    public PetAnimatorController(Animator animator)
    {
        this.animator = animator;
    }

    public void SetBool(PetAnimationType animationType, bool value)
    {
        switch (animationType)
        {
            case PetAnimationType.Idle:
                animator.SetBool(Idle, value);
                break;
            case PetAnimationType.Walk:
                animator.SetBool(Walk, value);
                break;
        }
    }

    public void SetTrigger(PetAnimationType animationType)
    {
        switch (animationType)
        {
            case PetAnimationType.Attack:
                animator.SetTrigger(Attack);
                break;
            case PetAnimationType.Spawn:
                animator.SetTrigger(Spawn);
                break;
        }
    }

    public float GetAnimationDuration(PetAnimationType animationType)
    {
        var clips = animator.runtimeAnimatorController.animationClips;
        foreach (var clip in clips)
        {
            if (clip.name.ToLower().Contains(animationType.ToString().ToLower()))
                return clip.length;
        }
        return 1f; // fallback
    }
}
