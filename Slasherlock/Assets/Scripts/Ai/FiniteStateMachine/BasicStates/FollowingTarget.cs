using System;
using System.Collections;
using Assets.Interactables.Physics;
using Assets.Scripts.Ai.FiniteStateMachine;
using Assets.Scripts.Ai.FiniteStateMachine.BasicStates;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using Random = UnityEngine.Random;

public class FollowingTarget : State
{
    readonly AudioClip tryingToOpenDoorSound;
    Door onDoor;
    bool waiting = false;
    MotionBlur blur;
    Func<float> brokeDoorPercentage;
    readonly float waitWhenWaking;

    public FollowingTarget(Fsm fsm, AudioClip tryingToOpenDoorSound, Func<float> brokeDoorPercentage,
        float waitWhenWaking) : base(fsm)
    {
        this.tryingToOpenDoorSound = tryingToOpenDoorSound;
        this.brokeDoorPercentage = brokeDoorPercentage;
        this.waitWhenWaking = waitWhenWaking;
    }

    public override void UpdateState()
    {
    }

    public override void OnEnter()
    {
        var volume = GameObject.FindObjectOfType<Volume>();
        volume.profile.TryGet(out blur);
        blur.active = true;

        if (string.IsNullOrEmpty(fsm.LastState) || !fsm.LastState.Contains(nameof(WalkAroundState)))
            fsm.PathFinder.FollowTarget(fsm.Awareness.LastTargetFound);
        else
        {
            IEnumerator wait()
            {
                fsm.Mover.PreventMovement();
                fsm.PathFinder.StopFollowing();
                yield return new WaitForSeconds(waitWhenWaking);
                fsm.Mover.AllowMovement();
                fsm.PathFinder.FollowTarget(fsm.Awareness.LastTargetFound);
            }

            fsm.StartCoroutine(wait());
        }
    }

    public override void OnExit()
    {
        blur.active = false;
        fsm.PathFinder.StopFollowing();
    }

    public override string ToString()
        => typeof(FollowingTarget).Name;

    public override void OnTriggerEnter(Collider2D other)
    {
        if (other.gameObject.GetComponent<Door>() is { } door)
            onDoor = door;
    }

    public override void OnTriggerStay(Collider2D other)
    {
        if (waiting) return;
        if (onDoor)
        {
            if (onDoor.GetState() == Door.State.ConfirmedLock)
            {
                var audio = fsm.gameObject.GetComponent<AudioSource>();

                IEnumerator waitSound(Door door)
                {
                    yield return new WaitForSeconds(tryingToOpenDoorSound.length);
                    waiting = false;

                    if (Random.value <= brokeDoorPercentage())
                        door.ForceOpen();
                    else
                        // nao deixa o jason preso fora da grid da porta
                        fsm.transform.position = AstarPath.active.GetNearest(fsm.transform.position).position;

                    OnEnter();
                }

                waiting = true;
                audio.PlayOneShot(tryingToOpenDoorSound);
                onDoor.Shake();
                fsm.PathFinder.StopFollowing();
                fsm.StartCoroutine(waitSound(onDoor));
                onDoor = null;
            }
        }
    }
}