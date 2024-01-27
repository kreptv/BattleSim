using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Untitled(7)", menuName = "ScriptableObjects/Character", order = 1)]
public class CharacterSO : ScriptableObject
{
    public string characterName = "John";
    public string characterDescription = "Indescribable";

    public Image characterPortrait;

    [Space(10)]
    [Header("Base Stats")]
    public int baseHealth = 10;
    public int baseAttack = 1;
    public int baseDefense = 1;
    public int baseSpeed = 1;

    [Space(10)]
    [Header("Stat Growths")]
    public float healthGrowth = 0f;
    public float attackGrowth = 0f;
    public float defenseGrowth = 0f;
    public float speedGrowth = 0f;

    [Header("Moveset")]
    public List<MoveSO> currentMoveset = new List<MoveSO>();
    [Space(10)]
    public List<MoveSO> learnableMoves = new List<MoveSO>(); 
}
