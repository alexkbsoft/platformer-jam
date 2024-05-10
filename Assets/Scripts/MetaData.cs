using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Meta Data")]

public class MetaData : ScriptableObject
{
    public int SoulShards = 0;
    public List<Vector3> RelicPositions = new ();
    public int Relics = 0;
    public int RunLevel = 1;
    public int JumpLevel = 1;
    public int BonusJumps = 0;
    public int DashLevel = 0;
    public int ClimbLevel = 0;
    public int RangeAttackLevel = 0;
    public int AttackLevel = 1;
}
