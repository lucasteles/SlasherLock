using System.Collections;
using Assets.Interactables.Physics;
using Assets.Scripts.Ai.FiniteStateMachine;
using UnityEngine;

public class FollowingTarget : State
{
    readonly AudioClip tryingToOpenDoorSound;
    Door onDoor;
    bool waiting = false;
    float brokeDoorPercentage;

    public FollowingTarget(Fsm fsm, AudioClip tryingToOpenDoorSound, float brokeDoorPercentage) : base(fsm)
    {
        this.tryingToOpenDoorSound = tryingToOpenDoorSound;
        this.brokeDoorPercentage = brokeDoorPercentage;
    }

    public override void Execute()
    {
    }

    public override void OnEnter()
        => fsm.PathFinder.FollowTarget(fsm.Awareness.LastTargetFound);

    public override void OnExit() => fsm.PathFinder.StopFollowing();

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

                    if (Random.value <= brokeDoorPercentage)
                        door.ForceOpen();

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