using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveMonsterType : MonoBehaviour
{
    public MonsterIns MonsterType;
    public Vector3 OrgLocation;
    public Vector3 NewLocation;
    public bool SawTarget;
    public bool Alive;
    public bool Moved;
}