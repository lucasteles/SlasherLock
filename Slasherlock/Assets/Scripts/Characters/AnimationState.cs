using System;
using Assets.Scripts.Physics;
using UnityEditor.Animations;
using UnityEngine;

public class AnimationState : MonoBehaviour
{
    Animator anim;
    Mover mover;
    CheckWalking check;
    WalkSide side = WalkSide.Down;
    bool isWalking;

    string animName;
    void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        check = new CheckWalking(transform);
        mover = GetComponent<Mover>();
    }

    string GetAnimName() => $"{(isWalking ? "walk" : "idle")}-{side.ToString().ToLower()}";

    void Update()
    {
        isWalking = !check.IsStopped();
        side = mover.Side;

        var newName = GetAnimName();
        if (string.IsNullOrEmpty(animName) || newName != animName)
        {
            animName = newName;
            anim.Play(animName);
        }

        check.Update(Time.deltaTime);
    }
}
