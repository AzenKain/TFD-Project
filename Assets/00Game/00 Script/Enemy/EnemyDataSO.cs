using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "enemyData", menuName = "Enemy")]
public class EnemyDataSO : ScriptableObject
{
    public int _ID;
    public string _name;
    public float _HP, _speed, _armor, _baseDmg, _forceKnockBack, _detectTargetRadius, _rangeAttack;
    public int _cost;
}
