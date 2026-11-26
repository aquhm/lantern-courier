using LanternCourier.Player;
using LanternCourier.World;
using UnityEngine;

namespace LanternCourier.Enemy
{
    public sealed class MothSenses : MonoBehaviour
    {
        [SerializeField] LayerMask lightMask;
        [SerializeField] float range = 9f;
        [SerializeField] float scanInterval = 0.25f;

        readonly Collider[] hits = new Collider[16];
        float nextScan;

        public Transform NearestLight { get; private set; }
        public LanternFuel Lantern { get; private set; }

        void Update()
        {
            if (Time.time < nextScan)
            {
                return;
            }

            nextScan = Time.time + scanInterval;
            Scan();
        }

        void Scan()
        {
            NearestLight = null;
            Lantern = null;

            var origin = transform.position;
            int count = Physics.OverlapSphereNonAlloc(origin, range, hits, lightMask, QueryTriggerInteraction.Collide);
            float best = float.MaxValue;

            for (int i = 0; i < count; i++)
            {
                var hit = hits[i];
                bool isLantern = hit.TryGetComponent(out LanternFuel fuel);
                if (isLantern ? fuel.IsEmpty : !IsLitLamp(hit))
                {
                    continue;
                }

                float sqrDistance = (hit.transform.position - origin).sqrMagnitude;
                if (sqrDistance < best)
                {
                    best = sqrDistance;
                    NearestLight = hit.transform;
                    Lantern = isLantern ? fuel : null;
                }
            }
        }

        static bool IsLitLamp(Collider hit)
        {
            return hit.TryGetComponent(out StreetLamp lamp) && lamp.IsLit;
        }
    }
}
