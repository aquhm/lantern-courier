using LanternCourier.Core;
using LanternCourier.Economy;
using UnityEngine;

namespace LanternCourier.Player
{
    public sealed class LanternFuel : MonoBehaviour
    {
        [SerializeField] UpgradeCatalog catalog;
        [SerializeField] Light lantern;
        [SerializeField] float lanternIntensity = 4f;
        [SerializeField] float burnPerSecond = 0.6f;
        [SerializeField] Transform refuelPoint;
        [SerializeField] float refuelRadius = 3.5f;
        [SerializeField] float refuelPerSecond = 20f;

        GameSession session;
        float capacity;
        float current;

        public float Capacity => capacity;
        public float Current => current;
        public bool IsEmpty => current <= 0f;

        void Start()
        {
            session = GameSession.Instance;
            ApplyTankTier(session.Data.tankTier);
            current = capacity;
            Publish();
        }

        void Update()
        {
            float delta = -burnPerSecond * Time.deltaTime;
            if (refuelPoint != null)
            {
                float sqrDistance = (transform.position - refuelPoint.position).sqrMagnitude;
                if (sqrDistance <= refuelRadius * refuelRadius)
                {
                    delta += refuelPerSecond * Time.deltaTime;
                }
            }

            Change(delta);
        }

        public void ApplyTankTier(int tier)
        {
            capacity = catalog.Get(UpgradeCatalog.FuelTank, tier).capacity;
            current = Mathf.Min(current, capacity);
            Publish();
        }

        public float Drain(float amount)
        {
            float drained = Mathf.Min(amount, current);
            Change(-drained);
            return drained;
        }

        void Change(float delta)
        {
            current = Mathf.Clamp(current + delta, 0f, capacity);
            Publish();
        }

        void Publish()
        {
            float ratio = capacity > 0f ? current / capacity : 0f;
            session.FuelRatio.Value = ratio;
            lantern.enabled = current > 0f;
            lantern.intensity = lanternIntensity * Mathf.Lerp(0.3f, 1f, ratio);
        }
    }
}
