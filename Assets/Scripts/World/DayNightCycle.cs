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
        [SerializeField] float sunYaw = -30f;
        [SerializeField] float dayIntensity = 1.2f;
        [SerializeField] float nightIntensity = 0.05f;
        [SerializeField] float dayAmbient = 1f;
        [SerializeField] float nightAmbient = 0.2f;

        float hour;

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

        void Update()
        {
            Hour = hour + Time.deltaTime * 24f / secondsPerDay;
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
