using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Gem")]
public class GemSO : ScriptableObject
{
    public enum Gemtype
    {
        AttackBuff,
        SpeedBuff
    };

    public Gemtype gemtype;
    public bool isActive;
    public int buffRate;
    public string title;
    public string description;
}
