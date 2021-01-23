using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Characters.Enemy;
using Assets.Scripts.Physics;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[Serializable]
public struct Difficult
{
    public float jasonSpeed;
    public float jasonBrokeDoorPercentage;
    public float jasonWalkSoundSpeed;
    public float jasonTimeToGiveUp;
}

public class DificultManager : MonoBehaviour
{
    [SerializeField] Difficult level0;
    [SerializeField] Difficult level1;
    [SerializeField] Difficult level2;
    [SerializeField] Difficult level3;
    [SerializeField] Difficult level4;
    [SerializeField] Difficult level5;
    [SerializeField] Difficult level6;

    [SerializeField] GameObject player;
    [SerializeField] AudioSource jsonWalkAudio;

    [SerializeField] AudioSource musicNormal;
    [SerializeField] AudioSource hardMusic;
    Mover mover;
    EnemyFsm fsm;
    CharacterInventary inventary;
    Difficult[] difficulties;
    Animator animator;
    ShadowsMidtonesHighlights shadowsMidtonesHighlights;

    void Awake()
    {
        var volume = FindObjectOfType<Volume>();
        volume.profile.TryGet(out shadowsMidtonesHighlights);
        shadowsMidtonesHighlights.active = false;
        mover = GetComponent<Mover>();
        fsm = GetComponent<EnemyFsm>();
        inventary = player.GetComponent<CharacterInventary>();
        animator = GetComponentInChildren<Animator>();
    }

    void Start()
    {
        difficulties = new[] {level0, level1, level2, level3, level4, level5, level6};
    }

    void Update()
    {
        var newSpeed = difficulties[inventary.NumberOfKeys].jasonSpeed;
        mover.SetMoveSeed(newSpeed);

        var newBrokeDoorPercentage = difficulties[inventary.NumberOfKeys].jasonBrokeDoorPercentage;
        fsm.SetBrokeDoorPercentage(newBrokeDoorPercentage);
        fsm.SetTimeToGiveUp(difficulties[inventary.NumberOfKeys].jasonTimeToGiveUp);

        jsonWalkAudio.pitch = animator.speed = difficulties[inventary.NumberOfKeys].jasonWalkSoundSpeed;

        if (inventary.NumberOfKeys == difficulties.Length-1 && !shadowsMidtonesHighlights.active)
        {
            shadowsMidtonesHighlights.active = true;
            fsm.MoveAndSetState<FollowingTarget>(GameObject.Find("LastApearEvent").transform.position);
            musicNormal.Pause();
            hardMusic.Play();
        }
    }
}