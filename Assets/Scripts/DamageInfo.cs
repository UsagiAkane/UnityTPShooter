using UnityEngine;

public struct DamageInfo
{
    public float amount;
    public GameObject source;      
    public IDamageInstigator instigator;
    public Vector3 hitPoint;
    public Vector3 hitDirection;
}