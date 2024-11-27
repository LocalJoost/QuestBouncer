using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace CubeBouncer
{
    public class RoomMeshDetector : MonoBehaviour
    {
        [SerializeField]
        private GameObject roomMeshBuildingBlock;
        
        private GameObject roomMesh;
        private float lastTimeCreated;
        private bool isRoomMeshActive = false;
        
        public UnityEvent OnRoomMeshCreated;

        private void Start()
        {
            lastTimeCreated = Time.time -5;
#if UNITY_EDITOR
            isRoomMeshActive = true;
            OnRoomMeshCreated?.Invoke();
#endif
        }

        private async void Update()
        {
            if(isRoomMeshActive)
            {
                return;
            }

            if (!(lastTimeCreated + 5 < Time.time)) return;
            if(roomMesh != null)
            {
                Destroy(roomMesh);
            }
            roomMesh = Instantiate(roomMeshBuildingBlock);
            await Task.Delay(100);
            var roomMeshAnchor = FindAnyObjectByType<RoomMeshAnchor>();
            if(roomMeshAnchor != null)
            {
                isRoomMeshActive = true;
                OnRoomMeshCreated?.Invoke();
                return;
            }
            lastTimeCreated = Time.time;
        }
    }
}