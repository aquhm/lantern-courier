using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace LanternCourier.Player
{
    public sealed class FlareThrower : MonoBehaviour
    {
        [SerializeField] InputActionReference throwFlare;
        [SerializeField] Flare flarePrefab;
        [SerializeField] DroneController drone;
        [SerializeField] int poolSize = 4;
        [SerializeField] float cooldown = 0.6f;
        [SerializeField] float launchHeight = 0.6f;

        readonly Stack<Flare> idle = new();
        float nextThrowTime;

        public int IdleCount => idle.Count;

        void Awake()
        {
            for (int i = 0; i < poolSize; i++)
            {
                var flare = Instantiate(flarePrefab);
                flare.Bind(this);
                flare.gameObject.SetActive(false);
                idle.Push(flare);
            }
        }

        void OnEnable()
        {
            throwFlare.action.Enable();
            throwFlare.action.performed += OnThrowPerformed;
        }

        void OnDisable()
        {
            throwFlare.action.performed -= OnThrowPerformed;
        }

        void OnThrowPerformed(InputAction.CallbackContext context)
        {
            TryThrow();
        }

        public bool TryThrow()
        {
            if (Time.timeScale <= 0f || Time.time < nextThrowTime || idle.Count == 0)
            {
                return false;
            }

            nextThrowTime = Time.time + cooldown;
            var flare = idle.Pop();
            flare.Launch(transform.position + Vector3.up * launchHeight, drone.Facing);
            return true;
        }

        public void Return(Flare flare)
        {
            flare.gameObject.SetActive(false);
            idle.Push(flare);
        }
    }
}
