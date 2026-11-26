using LanternCourier.Core;
using LanternCourier.Save;
using LanternCourier.World;
using UnityEngine;
using UnityEngine.UI;

namespace LanternCourier.UI
{
    public sealed class TitleMenu : MonoBehaviour
    {
        [SerializeField] Button continueButton;

        void Start()
        {
            continueButton.interactable = SaveSerializer.Exists();
        }

        public void NewGame()
        {
            GameSession.Instance.NewGame();
            SceneFlow.LoadGame();
        }

        public void Continue()
        {
            if (GameSession.Instance.Continue())
            {
                SceneFlow.LoadGame();
            }
            else
            {
                continueButton.interactable = false;
            }
        }
    }
}
