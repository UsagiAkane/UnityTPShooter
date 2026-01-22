using System;
using UnityEngine;

namespace Guns
{
    public class Rifle : ProjectileGun
    {
        private void Update()
        {
            if (!HasAimDebug())
                return;

            Vector3 dir = (GetLastAimPoint() - firePoint.position).normalized;

            Debug.DrawRay(
                firePoint.position,
                dir * 10f,
                Color.magenta
            );
        }
    }
}