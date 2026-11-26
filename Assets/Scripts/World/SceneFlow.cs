using UnityEngine.SceneManagement;

namespace LanternCourier.World
{
    public static class SceneFlow
    {
        public const string TitleScene = "Title";
        public const string GameScene = "Game";

        public static void LoadTitle()
        {
            SceneManager.LoadScene(TitleScene);
        }

        public static void LoadGame()
        {
            SceneManager.LoadScene(GameScene);
        }
    }
}
