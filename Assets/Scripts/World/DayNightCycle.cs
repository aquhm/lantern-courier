using UnityEngine;

namespace LanternCourier.World
{
    public sealed class DayNightCycle : MonoBehaviour
    {
        [SerializeField] Light sun;
        [SerializeField] float secondsPerDay = 240f;
        [SerializeField, Range(0f, 24f)] float startHour = 8f;
        [SerializeField, Range(0f, 24f)] float duskHour = 19f;
        [SerializeField, Range(0f, 24f)] float dawnHour = 5f;
        [SerializeField, Range(0f, 24f)] float lampsOnHour = 18.5f;
        [SerializeField, Range(0f, 24f)] float lampsOffHour = 6f;
        [SerializeField] float sunYaw = -30f;
        [SerializeField] float dayIntensity = 1.2f;
        [SerializeField] float nightIntensity = 0.05f;
        [SerializeField] float dayAmbient = 1f;
        [SerializeField] float nightAmbient = 0.2f;

        float hour;
        StreetLamp[] lamps = System.Array.Empty<StreetLamp>();

        public float Hour
        {
            get => hour;
            set
            {
                hour = Mathf.Repeat(value, 24f);
                ApplyLighting();
            }
        }

        public bool IsNight => hour >= duskHour || hour < dawnHour;

        void Awake()
        {
            Hour = startHour;
        }

        void Start()
        {
            lamps = FindObjectsByType<StreetLamp>(FindObjectsSortMode.None);
            SetLamps(hour >= lampsOnHour || hour < lampsOffHour);
        }

        void Update()
        {
            Hour = hour + Time.deltaTime * 24f / secondsPerDay;
            UpdateLamps();
        }

        void UpdateLamps()
        {
            float minute = Mathf.Floor(hour * 60f) / 60f;
            if (minute == lampsOnHour)
            {
                SetLamps(true);
            }
            else if (minute == lampsOffHour)
            {
                SetLamps(false);
            }
        }

        void SetLamps(bool lit)
        {
            foreach (var lamp in lamps)
            {
                if (lamp.IsLit != lit)
                {
                    lamp.SetLit(lit);
                }
            }
        }

        void ApplyLighting()
        {
            // 6:00 puts the sun on the horizon, 12:00 straight overhead.
            float pitch = (hour - 6f) / 24f * 360f;
            sun.transform.rotation = Quaternion.Euler(pitch, sunYaw, 0f);

            bool night = IsNight;
            sun.intensity = night ? nightIntensity : dayIntensity;
            RenderSettings.ambientIntensity = night ? nightAmbient : dayAmbient;
        }
    }
}
