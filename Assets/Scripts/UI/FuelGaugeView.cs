using LanternCourier.Core;
using R3;
using TMPro;
using UnityEngine;

namespace LanternCourier.UI
{
    public sealed class FuelGaugeView : MonoBehaviour
    {
        [SerializeField] RectTransform fill;
        [SerializeField] TMP_Text label;
        [SerializeField, Range(0f, 1f)] float lowFuelRatio = 0.25f;
        [SerializeField] float blinkSpeed = 4f;
        [SerializeField] float blinkScale = 0.06f;
        [SerializeField] Color warningColor = new(1f, 0.35f, 0.25f, 1f);

        Color normalColor;

        void Start()
        {
            normalColor = label.color;

            var fuel = GameSession.Instance.FuelRatio;
            fuel.Subscribe(Render)
                .AddTo(this);
            fuel.Subscribe(UpdateWarning)
                .AddTo(this);
        }

        void Render(float ratio)
        {
            var scale = fill.localScale;
            scale.x = Mathf.Clamp01(ratio);
            fill.localScale = scale;
            label.SetText("Fuel {0:0}%", ratio * 100f);
        }

        void UpdateWarning(float ratio)
        {
            bool low = ratio <= lowFuelRatio;
            float blink = low ? Mathf.PingPong(Time.unscaledTime * blinkSpeed, 1f) : 0f;
            transform.localScale = Vector3.one * (1f + blink * blinkScale);
            label.color = low ? Color.Lerp(normalColor, warningColor, blink) : normalColor;
        }
    }
}
