using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyAnimatorController
{
    private readonly Animator animator;
    private readonly Dictionary<EnemyAnimationType, int> _animationHashes = new Dictionary<EnemyAnimationType, int>();

    public EnemyAnimatorController(Animator animator)
    {
        this.animator = animator;
        foreach (EnemyAnimationType type in Enum.GetValues(typeof(EnemyAnimationType)))
        {
            _animationHashes.Add(type, Animator.StringToHash(type.ToString()));
        }
    }

    public void SetBool(EnemyAnimationType state, bool value)
    {
        animator.SetBool(_animationHashes[state], value);
    }

    public void SetTrigger(EnemyAnimationType state)
    {
        animator.SetTrigger(_animationHashes[state]);
    }

    public float GetAnimationDuration(EnemyAnimationType type)
    {
        var clips = animator.runtimeAnimatorController.animationClips;
        var clip = clips.FirstOrDefault(c => c.name == type.ToString());
        return clip != null ? clip.length : 0.5f;
    }
}