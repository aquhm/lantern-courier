using UnityEngine;

namespace LanternCourier.World
{
    public sealed class VillageSchedule : MonoBehaviour
    {
        [SerializeField] DayNightCycle cycle;
        [SerializeField] GameObject recipient;

        bool night;

        void Start()
        {
            Apply(cycle.IsNight);
        }

        void Update()
        {
            bool isNight = cycle.IsNight;
            if (isNight != night)
            {
                Apply(isNight);
            }
        }

        void Apply(bool isNight)
        {
            night = isNight;
            recipient.SetActive(!isNight);
        }
    }
}
