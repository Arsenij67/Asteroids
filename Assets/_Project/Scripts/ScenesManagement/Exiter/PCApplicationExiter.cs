using UnityEngine;

namespace Asteroid.Exit
{
    public class PCApplicationQuitter : IApplicationQuitter
    {
        public void Quit()
        {
            Application.Quit();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }
    }
}
