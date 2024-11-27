using UnityEngine;

namespace CubeBouncer
{
    public class HandMenuController : MonoBehaviour
    {
        private CubeManager cubeManager;
        private void Start()
        {
            cubeManager = UnityEngine.Object.FindFirstObjectByType<CubeManager>();
        }
        
        public void CreateGrid()
        {
            cubeManager.CreateGrid();
        }
        
        public void DropAll()
        {
            cubeManager.DropAll();
        }
        
        public void RevertAll()
        {
            cubeManager.RevertAll();
        }
    }
}