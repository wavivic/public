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

            // ���� ��Ÿ��� ���� ������ ����
            // ...

            Debug.Log("��� ������ : " + damage);
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

            // �ּ� ���� �ð� �ؼ����� ���ϸ� ������ 0
            return 0;
        }
    }
}