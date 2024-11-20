using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameButton = Utils.Define.GameButton;
using ButtonState = Utils.Define.ButtonState;
using Interface;
using JetBrains.Annotations;
using Unity.Mathematics;
using Utils;
using Random = UnityEngine.Random;
using InGame;

namespace CombatSystem
{
    public abstract partial class Shoot : MonoBehaviour, IWeapon
    {
        protected Agent _owner;
        protected Camera _myCam;
        protected WeaponSheetData _data;
        public CoverUp _coverUp;

        protected bool _isTargetingType;
        protected bool _isChargingType;
        protected bool _isWarmupType;
        protected bool _isRangeType;
        protected bool _isSprayType;
        protected bool _isMachineGun = false;

        protected int _currAmmo;
        protected float _currTime;
        [SerializeField] protected float _myCharingTime;
        [SerializeField] protected float _myAttackInterval;
        [SerializeField] protected float _myAimingPrecision;
        [SerializeField] protected float _myDamageMultiplier;
        protected LayerMask _layer;
        protected bool _needReload;

        #region Property

        public virtual int CurrAmmo
        {
            get => _currAmmo;
            protected set
            {
                _currAmmo = value;
                _needReload = _currAmmo < _data.Magazine;
            }
        }
        public bool NeedReload => _needReload;
        public bool IsChargingType => _isChargingType;
        public bool IsMachineGun => _isMachineGun;
        public Define.WeaponType WeaponType { get; protected set; }

        public CoverUp Cover
        {
            get => _coverUp;
            set => _coverUp = value;
        }
        #endregion

        public virtual void Initialize(AgentData data)
        {
            _data = data.WeaponSheet;
            CurrAmmo = _data.Magazine;
            _myAttackInterval = _data.AttackInterval;
            _currTime = _myAttackInterval;
            _myAimingPrecision = _data.AimingPrecision;

            _owner = GetComponentInParent<Agent>();
            _myCam = _owner.transform.parent.GetComponentInChildren<Camera>();
            _layer = 1 << LayerMask.NameToLayer("Monster");

            Define.WeaponType type =
                (Define.WeaponType)Enum.Parse(typeof(Define.WeaponType),
                    data.AgentSheet.WeaponType);

            SetTypeSettings(type);
        }

        public abstract void ShootProcess();
        
        public virtual void Shooting()
        {
            RaycastHit[] list = _isSprayType ? GetTargetListBySprayType() : GetTargetLisBySingle();

            if (list != null)
            {
                foreach (var target in list)
                {
                    float damage = CalculateDamage(target.point);
                    target.collider.gameObject.GetComponent<IDamageable>()?.GetDamage(damage, _owner.transform);
                }
            }
            else print($"<b><color=red>Nobody . . .</color></b>");
            
            CurrAmmo--;
            _currTime = 0;
        }

        public void Reload()
        {
            CurrAmmo = _data.Magazine;
            _myCharingTime = 0;

            if (_isWarmupType)
            {
                _myAttackInterval = _data.AttackInterval;
                _myDamageMultiplier = _data.DamageMultiplier;
                _myAimingPrecision = _data.AimingPrecision;
            }
        }

        public void Activate() => gameObject.SetActive(true);
        public void Deactivate() => gameObject.SetActive(false);

        #region --- Initial Setting ---

        private void SetTypeSettings(Define.WeaponType weaponeType)
        {
            WeaponType = weaponeType;
            _isMachineGun = false;
            switch (weaponeType)
            {
                case Define.WeaponType.None:
                    break;
                case Define.WeaponType.SMG:
                    _isRangeType = false;
                    _isTargetingType = true;
                    _isChargingType = false;
                    _isWarmupType = false;
                    _isSprayType = false;
                    break;
                case Define.WeaponType.SG:
                    _isRangeType = true;
                    _isTargetingType = true;
                    _isChargingType = false;
                    _isWarmupType = false;
                    _isSprayType = true;
                    break;
                case Define.WeaponType.AR:
                    _isRangeType = false;
                    _isTargetingType = true;
                    _isChargingType = false;
                    _isWarmupType = false;
                    _isSprayType = false;
                    break;
                case Define.WeaponType.MG:
                    _isRangeType = false;
                    _isTargetingType = false;
                    _isChargingType = false;
                    _isWarmupType = true;
                    _isSprayType = true;
                    _isMachineGun = true;
                    break;
                case Define.WeaponType.SR:
                    _isRangeType = false;
                    _isTargetingType = false;
                    _isChargingType = true;
                    _isWarmupType = false;
                    _isSprayType = false;
                    break;
                case Define.WeaponType.RL:
                    _isRangeType = true;
                    _isTargetingType = false;
                    _isChargingType = true;
                    _isWarmupType = false;
                    _isSprayType = false;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        #endregion
    }
}