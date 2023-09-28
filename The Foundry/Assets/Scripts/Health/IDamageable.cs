using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPSGame
{
    public interface IDamageable
    {
        public void OnTakeDamage(float amount, Actor instigator);
    }
}
