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

        // 누르고 있을 때
        public void CalculateCharging()
        {
            _myCharingTime += Time.deltaTime;

            if (_isMachineGun)
            {
                // 발사 간격 곱 계수 계산
                _myAttackInterval *= _data.AttackIntervalMultiplier;
                _myAttackInterval = math.clamp(_myAttackInterval, _data.MinAttackInterval, _data.AttackInterval);
            }
        }

        public void ResetCharging()
        {
            _myCharingTime = 0;

            if (_isMachineGun)
            {
                // 발사 간격 초기화
                _myAttackInterval = _data.AttackInterval;

                // 데미지 계수 초기화
                _myDamageMultiplier = _data.DamageMultiplier;

                // 집탄율 초기화
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
                    // 발사 간격 곱 계수 계산
                    _myAttackInterval *= _data.AttackIntervalMultiplier;
                    _myAttackInterval = math.clamp(_myAttackInterval, _data.MinAttackInterval, _data.AttackInterval);
                }
            }

            // 뗐을 때
            else if (Managers.Input.GetButton(Define.GameButton.Shoot, Define.ButtonState.IsUp))
            {
                _myCharingTime = 0;

                if (_isMachineGun)
                {
                    // 발사 간격 초기화
                    _myAttackInterval = _data.AttackInterval;

                    // 데미지 계수 초기화
                    _myDamageMultiplier = _data.DamageMultiplier;

                    // 집탄율 초기화
                    _myAimingPrecision = _data.AimingPrecision;
                }
            }
        }

    }
}