using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Untitled(7)", menuName = "ScriptableObjects/Character", order = 1)]
public class CharacterSO : ScriptableObject
{
    public string characterName = "John";
    public string characterDescription = "Indescribable";

    public Sprite characterIcon;
    public Sprite characterSprite;

    public TypeSO characterType;

    public int startingLevel = 1;

    [Space(10)]
    [Header("Base Stats")]
    public int baseHealth = 10;
    public int baseAttack = 1;
    public int baseDefense = 1;
    public int baseSpeed = 1;

    [Space(10)]
    [Header("Stat Growths")]
    [Description("How much stats go up by on level up")]
    public int healthGrowth = 0;
    public int attackGrowth = 0;
    public int defenseGrowth = 0;
    public int speedGrowth = 0;

    [Space(10)]
    [Header("Stat Caps")]
    public int healthCap = 25;
    public int attackCap = 25;
    public int defenseCap = 25;
    public int speedCap = 25;

    [Header("Moveset")]
    public List<MoveSO> startingMoves = new List<MoveSO>();
    [Space(10)]
    public List<MoveSO> learnableMoves = new List<MoveSO>(); 
}
