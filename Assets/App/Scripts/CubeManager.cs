using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Serialization;

namespace CubeBouncer
{
    public class CubeManager : MonoBehaviour
    {
        [SerializeField]
        private GameObject cubeGameObject;
        
        [SerializeField]
        private AudioClip readyClip;
        
        [SerializeField]
        private AudioClip returnAllClip;
        
        private readonly List<CubeManipulator> cubes = new();
        private AudioSource audioSource;

        private void Start()
        {
            audioSource = GetComponent<AudioSource>();
            CreateGrid();
        }

        public void RevertAll()
        {
            audioSource.PlayOneShot(returnAllClip);
            foreach (var c in cubes)
            {
                c.Revert();
            }
        }

        public void DropAll()
        {
            foreach (var c in cubes)
            {
                c.Drop();
            }
        }

        public void CreateGrid()
        {
            foreach (var c in cubes)
            {
                Destroy(c.gameObject);
            }
            cubes.Clear();

            var startPose = new Pose
            {
                position = Camera.main.transform.position +
                           Camera.main.transform.forward * 0.5f,
                rotation = Camera.main.transform.rotation
            };

            const float size = 0.2f;
            const float maxX = 0.35f;
            const float maxY = 0.35f;
            const float maxZ = 1f;

            var id = 0;
            for (var z = 0f; z <= maxZ; z += size)
            {
                for (var x = -maxX; x <= maxX; x += size)
                {
                    for (var y = -maxY; y <= maxY; y += size)
                    {
                        CreateCube(id++, 
                            startPose.position + startPose.forward * z + startPose.right * x + startPose.up * y, 
                            startPose.rotation);
                    }
                }
            }
            audioSource.PlayOneShot(readyClip);
        }

        private void CreateCube(int id, Vector3 location, Quaternion rotation)
        {
            var c = Instantiate(cubeGameObject, location, rotation);
            c.transform.RotateAround(location, c.transform.up, 180f);
            c.transform.parent = transform;
            var m = c.GetComponent<CubeManipulator>();
            m.Id = id;
            cubes.Add(m);
        }
    }
}
