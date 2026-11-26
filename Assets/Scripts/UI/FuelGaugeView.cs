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

        void Start()
        {
            GameSession.Instance.FuelRatio
                .Subscribe(Render)
                .AddTo(this);
        }

        void Render(float ratio)
        {
            var scale = fill.localScale;
            scale.x = Mathf.Clamp01(ratio);
            fill.localScale = scale;
            label.SetText("Fuel {0:0}%", ratio * 100f);
        }
    }
}
