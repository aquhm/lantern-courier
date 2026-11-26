using LanternCourier.World;
using UnityEngine;
using UnityEngine.InputSystem;

namespace LanternCourier.UI
{
    public sealed class PauseMenu : MonoBehaviour
    {
        [SerializeField] InputActionReference pause;
        [SerializeField] GameObject window;

        public bool IsPaused { get; private set; }

        void Awake()
        {
            window.SetActive(false);
        }

        void OnEnable()
        {
            pause.action.Enable();
            pause.action.performed += OnPausePerformed;
        }

        void OnDisable()
        {
            pause.action.performed -= OnPausePerformed;
        }

        void OnDestroy()
        {
            if (IsPaused)
            {
                Time.timeScale = 1f;
            }
        }

        void OnPausePerformed(InputAction.CallbackContext context)
        {
            if (IsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }

        public void Pause()
        {
            IsPaused = true;
            Time.timeScale = 0f;
            window.SetActive(true);
        }

        public void Resume()
        {
            IsPaused = false;
            Time.timeScale = 1f;
            window.SetActive(false);
        }

        public void QuitToTitle()
        {
            Resume();
            SceneFlow.LoadTitle();
        }
    }
}
