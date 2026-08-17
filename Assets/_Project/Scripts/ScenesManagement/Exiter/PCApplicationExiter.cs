using UnityEngine;

namespace Asteroid.Exit
{
    public class PCApplicationQuitter : IApplicationQuitter
    {
        public void Quit()
        {
#if !UNITY_EDITOR
            Application.Quit();
#endif
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }
    }
}
