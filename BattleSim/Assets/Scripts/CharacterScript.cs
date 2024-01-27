using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterScript : MonoBehaviour
{
    public CharacterSO charSO;

    public bool startsLocked, isLocked;

    [Header("Stats")]
    public int hp, atk, def, spd;

    public int currentHP;

    // Number of stages each stat is boosted by
    public int atkBoost = 0, defBoost = 0, spdBoost = 0;

    public int level;

    public MoveSO[] moveset = new MoveSO[3];

    void Start()
    {
        InitializeCharacter();
    }

    public void InitializeCharacter()
    {
        level = charSO.startingLevel;
        hp = charSO.baseHealth;
        atk = charSO.baseAttack;
        def = charSO.baseDefense;
        spd = charSO.baseSpeed;

        currentHP = hp;

        for(int i = 0; i < charSO.startingMoves.Count; i++)
        {
            moveset[i] = charSO.startingMoves[i];
        }
    }

    public void LevelUp()
    {
        level++;
        hp += charSO.healthGrowth;
        atk += charSO.attackGrowth;
        def += charSO.defenseGrowth;
        spd += charSO.speedGrowth;

        //TODO: Add menu for adding move from learnset
    }

    // Applies boosts, ensures they don't go over the cap of 6 boosts
    public void BoostStat(Stat stat, int boost)
    {
        if(stat == Stat.Attack)
        {
            atkBoost = Mathf.Min(atkBoost + 1, 6);
            return;
        }

        if (stat == Stat.Defense)
        {
            defBoost = Mathf.Min(defBoost + 1, 6);
            return;
        }

        if (stat == Stat.Speed)
        {
            spdBoost = Mathf.Min(spdBoost + 1, 6);
            return;
        }
    }
}
