using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Untitled(7)", menuName = "ScriptableObjects/Type", order = 1)]
public class TypeSO : ScriptableObject
{
    public string typeName = "Basic";

    [Header("Offensive Matchups")]
    public List<TypeSO> strongAgainst = new List<TypeSO>();
    public List<TypeSO> weakAgainst = new List<TypeSO>();
}
