using UnityEngine;

public struct DamageInfo
{
    public float amount;
    public GameObject source;      
    public GameObject instigator;  //хто player / AI
    public Vector3 hitPoint;
    public Vector3 hitDirection;
}

