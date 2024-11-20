using Unity.Mathematics;
using UnityEngine;
using Utils;

namespace CombatSystem
{
    public abstract partial class Shoot
    {
        
        protected virtual void SetTimer()
        {
            _currTime += Time.deltaTime;
        }

        // ������ ���� ��
        public void CalculateCharging()
        {
            _myCharingTime += Time.deltaTime;

            if (_isMachineGun)
            {
                // �߻� ���� �� ��� ���
                _myAttackInterval *= _data.AttackIntervalMultiplier;
                _myAttackInterval = math.clamp(_myAttackInterval, _data.MinAttackInterval, _data.AttackInterval);
            }
        }

        public void ResetCharging()
        {
            _myCharingTime = 0;

            if (_isMachineGun)
            {
                // �߻� ���� �ʱ�ȭ
                _myAttackInterval = _data.AttackInterval;

                // ������ ��� �ʱ�ȭ
                _myDamageMultiplier = _data.DamageMultiplier;

                // ��ź�� �ʱ�ȭ
                _myAimingPrecision = _data.AimingPrecision;
            }
        }

        public void SetChargingTime()
        {
            if (Managers.Input.GetButton(Define.GameButton.Shoot, Define.ButtonState.IsPressed))
            {
                _myCharingTime += Time.deltaTime;

                if (_isMachineGun)
                {
                    // �߻� ���� �� ��� ���
                    _myAttackInterval *= _data.AttackIntervalMultiplier;
                    _myAttackInterval = math.clamp(_myAttackInterval, _data.MinAttackInterval, _data.AttackInterval);
                }
            }

            // ���� ��
            else if (Managers.Input.GetButton(Define.GameButton.Shoot, Define.ButtonState.IsUp))
            {
                _myCharingTime = 0;

                if (_isMachineGun)
                {
                    // �߻� ���� �ʱ�ȭ
                    _myAttackInterval = _data.AttackInterval;

                    // ������ ��� �ʱ�ȭ
                    _myDamageMultiplier = _data.DamageMultiplier;

                    // ��ź�� �ʱ�ȭ
                    _myAimingPrecision = _data.AimingPrecision;
                }
            }
        }

    }
}