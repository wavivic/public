using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.Mathematics;
using UnityEngine;
using Utils;
using Random = UnityEngine.Random;

namespace CombatSystem
{
    public abstract partial class Shoot
    {
        
        [CanBeNull]
        private RaycastHit[] GetTargetLisBySingle(bool isBeforeShoot = false)
        {
            RaycastHit[] hits = null;

            if (!SetOriginPosition(out Vector3 origin)) return null;

            Ray directRay = _myCam.ScreenPointToRay(_myCam.WorldToScreenPoint(origin));
            Debug.DrawRay(directRay.origin, directRay.direction * 1000, Color.cyan, 0.5f);

            // Ray에 감지된 몬스터가 있는 경우
            if (Physics.Raycast(directRay, out RaycastHit hit,
                    Define.MAX_REACH + _myCam.nearClipPlane, _layer))
            {
                hits = new RaycastHit[] { hit };

                // 마우스 커서 있는 곳에 바로 타겟이 있냐, 없냐만 검사
                if (isBeforeShoot)
                    goto jump;

                if (_isRangeType) // 범위형
                {
                    hits = new RaycastHit[Define.MAX_HIT_PER_RANGE_ATTACK];
                    Ray ray = new Ray(hit.transform.position, directRay.direction);

                    int hitCount = Physics.SphereCastNonAlloc(ray, _data.AttackRange, hits, 10, _layer);

                    if (hitCount < hits.Length)
                    {
                        Array.Resize(ref hits, hitCount);
                    }
                }
            }

            jump:
            return hits;
        }

        [CanBeNull]
        protected RaycastHit[] GetTargetListBySprayType(bool isMachineGun = false)
        {
            List<RaycastHit> hits = new List<RaycastHit>();

            int howRepeat = isMachineGun ? 1 : Define.SPRAY_NUM_PER_ONE_SHOT;
            for (int i = 0; i < howRepeat; i++)
            {
                float radian = 1;

                if (isMachineGun)
                {
                    _myAimingPrecision *= _data.AimingPrecisionChangePerShot;
                    _myAimingPrecision = math.clamp(_myAimingPrecision, Define.AIMING_PRECISION_MACHINEGUN, _data.AimingPrecision);
                    
                    radian *= _myAimingPrecision;
                }
                
                // mouse click position
                if (!SetOriginPosition(out Vector3 origin)) return null;

                // mousePos convert to World
                Vector3 targetPos = origin;

                Vector2 randomDir = Random.insideUnitSphere;
                Vector3 randomPoint = targetPos + new Vector3(randomDir.x, randomDir.y, 0) * (radian);

                Ray ray = new Ray(_myCam.transform.position, (randomPoint - _myCam.transform.position).normalized);

                //DrawDebugPoint(randomPoint, 0.2f, Color.magenta);
                Debug.DrawRay(ray.origin, ray.direction * 100, Color.white, 0.5f);

                if (Physics.Raycast(ray.origin, ray.direction, out RaycastHit hit, Define.MAX_REACH, _layer))
                {
                    hits.Add(hit);
                }
            }

            return hits.ToArray();
        }

        protected bool SetOriginPosition(out Vector3 origin)
        {
            if (IsAutoAttack())
            {
                if (_owner.Detector.GetNearestTarget(out Vector3 temp))
                {
                    origin = temp;
                    return true;
                }
                else
                {
                    origin = Vector3.zero;
                    return false;
                }
            }
            else
            {
                Ray directRay = _myCam.ScreenPointToRay(Input.mousePosition);
                if(Physics.Raycast(directRay, out RaycastHit hit, Define.MAX_REACH + _myCam.nearClipPlane, _layer)){
                    origin = hit.collider.transform.position;
                    return true;
                }
                origin = Vector3.zero;
                return false;
            }
        }
        void DrawDebugPoint(Vector3 position, float pointSize, Color pointColor)
        {
            Debug.DrawLine(position - Vector3.up * pointSize, position + Vector3.up * pointSize, pointColor, 0.5f);
            Debug.DrawLine(position - Vector3.left * pointSize, position + Vector3.left * pointSize, pointColor, 0.5f);
            Debug.DrawLine(position - Vector3.forward * pointSize, position + Vector3.forward * pointSize, pointColor, 0.5f);
        }
    }
}