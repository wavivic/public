using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CombatSystem
{
    public abstract partial class Shoot
    {
        
        protected float CalculateDamage(Vector3 hitPos)
        {
            float damage = _owner.AgentData.FinalAttackPower * _data.AttackMultiplier;

            // critical damage
            if (IsCriticalHit())
                damage *= _owner.AgentSheetData.CriticalDamage;

            // charging damage
            if (_isChargingType)
                damage *= CalculateChargingDamage();

            // 적정 사거리에 따른 데미지 감소
            // ...

            Debug.Log("출력 데미지 : " + damage);
            return damage;
        }

        private bool IsCriticalHit()
        {
            return Random.value < _owner.AgentSheetData.Critical;
        }

        private float CalculateChargingDamage()
        {
            if (_myCharingTime >= _data.MinChargeTime)
            {
                float chargingTime = math.clamp(_myCharingTime, 0, _data.ChargeTime);
                float percentage = ((float)chargingTime / _data.ChargeTime);
                return percentage + 1.0f;
            }

            // 최소 차지 시간 준수하지 못하면 데미지 0
            return 0;
        }
    }
}