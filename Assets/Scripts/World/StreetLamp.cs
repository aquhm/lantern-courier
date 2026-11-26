using UnityEngine;

namespace LanternCourier.World
{
    public sealed class StreetLamp : MonoBehaviour
    {
        [SerializeField] Light lamp;

        public bool IsLit => lamp.enabled;

        void Awake()
        {
            SetLit(true);
        }

        public void SetLit(bool lit)
        {
            lamp.enabled = lit;
        }
    }
}
