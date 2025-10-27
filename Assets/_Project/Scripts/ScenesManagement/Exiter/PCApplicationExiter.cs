using UnityEngine;
namespace Asteroid.Exit
{
    public class PCApplicationQuitter : IApplicationQuitter
    {
        public void Quit()
        {
            Debug.Log("�����");
            Application.Quit();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }
    }
}
