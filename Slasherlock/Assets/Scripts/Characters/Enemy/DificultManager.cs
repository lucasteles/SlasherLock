using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Characters.Enemy;
using Assets.Scripts.Physics;
using UnityEngine;

[Serializable]
public struct Difficult
{
    public float jasonSpeed;
    public float jasonBrokeDoorPercentage;
}

public class DificultManager : MonoBehaviour
{
    [SerializeField] Difficult level0;
    [SerializeField] Difficult level1;
    [SerializeField] Difficult level2;
    [SerializeField] Difficult level3;
    [SerializeField] Difficult level4;
    [SerializeField] GameObject player;

    Mover mover;
    EnemyFsm fsm;
    CharacterInventary inventary;
    Difficult[] difficulties;
    void Awake()
    {
        mover = GetComponent<Mover>();
        fsm = GetComponent<EnemyFsm>();
        inventary = player.GetComponent<CharacterInventary>();
    }

    void Start()
    {
        difficulties = new[] {level0, level1, level2, level3, level4};
    }

    void Update()
    {
        var newSpeed = difficulties[inventary.Level].jasonSpeed;
        mover.SetMoveSeed(newSpeed);

        var newBrokeDoorPercentage = difficulties[inventary.Level].jasonBrokeDoorPercentage;
        fsm.SetBrokeDoorPercentage(newBrokeDoorPercentage);

    }
}
