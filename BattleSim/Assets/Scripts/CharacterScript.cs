using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterScript : MonoBehaviour
{
    public string charName;
    public string charDescription;
    public string element;
    public Sprite characterImage, characterIcon;
    public bool startsLocked, isLocked;
	public int defaultATK, defaultDEF, defaultSPD;
    public int atk, def, spd;
}
