using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Untitled(7)", menuName = "ScriptableObjects/Move", order = 1)]
public class MoveSO : ScriptableObject
{
    public string moveName = "Splash";
    public string moveDescription = "Does nothing";

    public Type type = Type.Basic;
    public List<MoveEffect> effects = new List<MoveEffect>();

    [Header("Damage Effects")]
    public int damagePower;

    /*[Space(10)]
    [Header("Debuff Effects")]*/
    
}

public enum MoveEffect
{
    Damage,
    Status
}

public enum Stat
{
    Attack,
    Defense,
    Speed
}

public enum Target
{
    Self,
    Opponent
}
