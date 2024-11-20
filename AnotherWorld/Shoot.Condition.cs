using Utils;

namespace CombatSystem
{
    public abstract partial class Shoot
    {
          
        public bool CanShoot()
        {
            bool isCanShootTime = _currTime >= _myAttackInterval;
            bool isHasAmmo = CurrAmmo > 0;

            if (!isHasAmmo || !isCanShootTime)
                return false;

            bool isPressed = Managers.Input.GetButton(Define.GameButton.Shoot, Define.ButtonState.IsPressed);
            bool isUp = Managers.Input.GetButton(Define.GameButton.Shoot, Define.ButtonState.IsUp);

            if (_isChargingType)
            {
                if (!isUp)
                    return false;
            }
            else
            {
                if (!isPressed)
                    return false;
            }

            if (_isTargetingType)
            {
                bool hasTarget = GetTargetLisBySingle(true) != null;
                if (!hasTarget)
                    return false;
            }

            return true;
        }

        public bool IsEnoughDelay()
        {
            return _currTime >= _myAttackInterval;
        }

        public bool IsTargetUnderCursor()
        {
            if (!_isTargetingType) return true;
            
            bool hasTarget = GetTargetLisBySingle(true) != null;
            
            if (!hasTarget) return false;
            return true;
        }

        public bool IsEnoughChargingTime()
        {
            if (!_isChargingType) return true;

            return _myCharingTime >= _data.MinChargeTime;
        }

        protected bool IsAutoAttack()
        {
            bool isAutoAttackWhenFocused = !Managers.Input.GetButton(Define.GameButton.Shoot, Define.ButtonState.IsUp) && (_owner.IsFocused &&
                _owner.IsAutoPilot &&
                !Managers.Input.GetButton(Define.GameButton.Shoot, Define.ButtonState.IsPressed));
                                           
            return !_owner.IsFocused || isAutoAttackWhenFocused;
        }
    }
}