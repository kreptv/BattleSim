using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Untitled(7)", menuName = "ScriptableObjects/Move", order = 1)]
public class MoveSO : ScriptableObject
{
    public string moveName = "Splash";
    public string moveDescription = "Does nothing";

    public TypeSO type;
    public List<MoveEffect> effects = new List<MoveEffect>();

    [Space(10)]
    [Header("Animations")]
    //public Sprite animSprite;
    public float animTime = 0.1f;
    public int animationIndex = 0;

    [Space(10)]
    [Header("Damage Effects")]
    public int damagePower;

    [Space(10)]
    [Header("Status Effects")]
    public Target statusTarget;
    public Stat statChanged;
    public int stagesChanged;
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
