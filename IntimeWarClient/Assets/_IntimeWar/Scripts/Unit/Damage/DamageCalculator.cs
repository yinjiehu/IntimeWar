using UnityEngine;
using System.Linq;
using System;
using System.Collections.Generic;

namespace YJH.Unit
{
    public struct DamageInfo
    {
        public float Damage { set; get; }
    }

    public class DamageCalculator
    {
        public static bool RaycastFirst<T>(Vector3 startPosition, Vector3 direction, float range, LayerMask layer,
            out RaycastHit hit, out T receiver, Func<RaycastHit, bool> exCondition = null) where T : Component
        {
            var hits = Physics.RaycastAll(startPosition, direction, range, layer, QueryTriggerInteraction.Collide)
                .Where(h => exCondition == null || exCondition(h))
                .OrderBy(h => Vector3.Distance(startPosition, h.point)).ToList();

            if (hits.Count == 0)
            {
                receiver = null;
                hit = new RaycastHit();
            }
            else
            {
                receiver = hits[0].collider.GetComponent<T>();
                hit = hits[0];
            }

            return hits.Count != 0;
        }

        public static RaycastHit[] RaycastAllOrdered(Vector3 startPosition, Vector3 direction, float range, LayerMask layer, Func<RaycastHit, bool> exCondition = null)
        {
            var hits = Physics.RaycastAll(startPosition, direction, range, layer, QueryTriggerInteraction.Collide)
                .Where(h => exCondition == null || exCondition(h))
                .OrderBy(h => Vector3.Distance(startPosition, h.point)).ToArray();

            return hits;
        }

        public static Collider2D[] RaycastBox2DOrdered(Vector2 startPosition, Vector2 endPosition, LayerMask layer)
        {
            Vector2 point = startPosition + (endPosition - startPosition) / 2;
            float distance = Vector2.Distance(startPosition, endPosition);
            Vector2 size = new Vector2(distance / 4, distance / 2);
            float angle = Vector2.Angle(Vector2.up, endPosition - startPosition);
            var colliders = Physics2D.OverlapBoxAll(point, size, angle, layer)
                .OrderBy(h => Vector2.Distance(startPosition, h.transform.position)).ToArray();
            return colliders;
        }



        public static RaycastHit2D[] RaycastLine2DAll(Vector2 origin, Vector2 direction, float distance, int layer)
        {
            var hits = Physics2D.RaycastAll(origin, direction)
                .OrderBy(h => Vector2.Distance(origin, h.point)).ToArray();
            return hits;
        }

        public static bool CreateRaycastLine2DDamge(UnitInfo attacker, DamageInfo damageInfo,
            Vector2 origin, Vector2 direction, float distance, int layer,
            Action<RaycastHit2D, CollisionEventReceiver> onHit)
        {
            HashSet<int> collidedUnits = new HashSet<int>();
            var hits = RaycastLine2DAll(origin, direction, distance, layer);
            var ev = new DamageEvent()
            {
                Attacker = attacker,
                Damage = damageInfo.Damage,
            };

            for (var i = 0; i < hits.Length; i++)
            {
                var receiver = hits[i].collider.GetComponent<CollisionEventReceiver>();
                if (receiver != null && receiver.Unit.Initialized && !receiver.Unit.IsDead)
                {
                    if (!receiver.IsPartialUnit)
                    {
                        if (collidedUnits.Contains(receiver.Unit.SeqNo))
                            continue;

                        collidedUnits.Add(receiver.Unit.SeqNo);
                    }
                    ev.HitPosition = hits[i].point;
                    ev.HitCollider = receiver.Collider;

                    if (onHit != null)
                    {
                        onHit(hits[i], receiver);
                    }

                    receiver.OnEvent(ev);
                }
            }
            return true;
        }

        public static bool CreateRaycasetBoxDamage(UnitInfo attacker, DamageInfo damageInfo, 
            Vector2 startPosition, Vector2 endPosition, LayerMask layer,
            Action<CollisionEventReceiver> onHit)
        {

            HashSet<int> collidedUnits = new HashSet<int>();
            var colliders = RaycastBox2DOrdered(startPosition, endPosition, layer);
            var ev = new DamageEvent()
            {
                Attacker = attacker,
                Damage = damageInfo.Damage,
            };

            for (var i = 0; i < colliders.Length; i++)
            {
                var receiver = colliders[i].GetComponent<CollisionEventReceiver>();
                if (receiver != null && receiver.Unit.Initialized && !receiver.Unit.IsDead)
                {
                    if (!receiver.IsPartialUnit)
                    {
                        if (collidedUnits.Contains(receiver.Unit.SeqNo))
                            continue;

                        collidedUnits.Add(receiver.Unit.SeqNo);
                    }

                    ev.HitCollider = receiver.Collider;

                    if (onHit != null)
                    {
                        onHit(receiver);
                    }

                    receiver.OnEvent(ev);
                }
            }
            return true;
        }

        public static bool CreateRaycastFirstLinearDamage(UnitInfo attacker, DamageInfo damageInfo, Vector3 startPosition, Vector3 direction, float maxDistance, LayerMask layer,
            Action<RaycastHit, CollisionEventReceiver> onHit, Action<Vector3> onMaxDistance)
        {
            RaycastHit hit; CollisionEventReceiver receiver;
            if (RaycastFirst(startPosition, direction, maxDistance, layer, out hit, out receiver))
            {
                if (receiver != null)
                {
                    var ev = new DamageEvent()
                    {
                        Attacker = attacker,

                        Damage = damageInfo.Damage,
                        HitPosition = hit.point,
                        HitCollider = receiver.Collider,
                    };
                    receiver.OnEvent(ev);

                    onHit(hit, receiver);
                }
                return true;
            }

            onMaxDistance(startPosition + direction.normalized * maxDistance);
            return false;
        }

        public static DamageEvent CreateRaycastAllLinearDamage(UnitInfo attacker, DamageInfo damageInfo,
            Vector3 startPosition, Vector3 direction, float maxDistance, LayerMask layer, Action<Vector3, CollisionEventReceiver> onHit = null, Action<Vector3> onComplete = null)
        {
            HashSet<int> collidedUnits = new HashSet<int>();
            var hits = RaycastAllOrdered(startPosition, direction, maxDistance, layer);
            bool isHitObject = false;
            var ev = new DamageEvent()
            {
                Attacker = attacker,
                Damage = damageInfo.Damage,
            };

            for (var i = 0; i < hits.Length; i++)
            {
                var receiver = hits[i].collider.GetComponent<CollisionEventReceiver>();
                if (receiver != null && receiver.Unit.Initialized && !receiver.Unit.IsDead)
                {
                    if (!receiver.IsPartialUnit)
                    {
                        if (collidedUnits.Contains(receiver.Unit.SeqNo))
                            continue;

                        collidedUnits.Add(receiver.Unit.SeqNo);
                    }

                    ev.HitPosition = hits[i].point;
                    ev.HitCollider = receiver.Collider;

                    if (onHit != null)
                    {
                        onHit(ev.HitPosition, receiver);
                        isHitObject = true;
                    }

                    receiver.OnEvent(ev);
                }
            }
            ev.HitPosition = startPosition + direction.normalized * maxDistance;
            if (!isHitObject)
            {
                if (onComplete != null)
                {
                    onComplete(ev.HitPosition);
                }
            }
            return ev;
        }

        public static void CreateCollisionBallisticDamage(UnitInfo attacker, CollisionEventReceiver receiver, DamageInfo damageInfo,
            Vector3 bulletPosition, Vector3 direction, int penetrationLevel = 0)
        {
            var ev = new DamageEvent()
            {
                Attacker = attacker,
                Damage = damageInfo.Damage,

                HitPosition = receiver.Collider.bounds.ClosestPoint(bulletPosition + direction * -100),
                HitCollider = receiver.Collider,
            };
            receiver.OnEvent(ev);
        }


        public static void CreateRangeDamage(UnitInfo attacker, DamageInfo damageInfo, Vector3 center, float radius, LayerMask collisionLayer, Func<float, float> damageRateByDistance = null)
        {
            var colliders = Physics.OverlapSphere(center, radius, collisionLayer, QueryTriggerInteraction.Collide);
            var collidedUnits = new HashSet<int>();

            var damageEvent = new DamageEvent()
            {
                Attacker = attacker,

                Damage = damageInfo.Damage,
            };

            for (var i = 0; i < colliders.Length; i++)
            {
                var receiver = colliders[i].GetComponent<CollisionEventReceiver>();
                if (receiver != null)
                {
                    if (receiver.Unit == null)
                    {
                        Debug.LogErrorFormat(receiver, "unit is not inialized!");
                        continue;
                    }
                    Debug.LogFormat("grenade hit {0}", receiver.Unit.name);

                    if (receiver.IsPartialUnit || !collidedUnits.Contains(receiver.Unit.SeqNo))
                    {
                        collidedUnits.Add(receiver.Unit.SeqNo);

                        var distance = Vector3.Distance(receiver.Collider.bounds.ClosestPoint(center), center);

                        var rate = damageRateByDistance == null ? (1 - distance / radius * 0.9f) + 0.1f : damageRateByDistance(distance);

                        var direction = receiver.Unit.Model.position - center;
                        direction.y = 0;
                        damageEvent.Damage = damageInfo.Damage * rate;

                        damageEvent.HitCollider = receiver.Collider;
                        receiver.OnEvent(damageEvent);
                    }
                }
            }
        }
    }
}