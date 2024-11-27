using UnityEngine;
using UnityEngine.Serialization;

namespace CubeBouncer
{
    public class CubeManipulator : MonoBehaviour
    {
        public int Id { get;set; }

        [SerializeField]
        private AudioClip bounceTogetherClip;

        [SerializeField]
        private AudioClip bounceOtherClip;

        private Rigidbody _rigidBody;
        private AudioSource _audioSource;


        private Vector3 _orginalPosition;
        private Vector3 _originalRotation;

        private void Start()
        {
            _rigidBody = GetComponent<Rigidbody>();
            _audioSource = GetComponent<AudioSource>();

            _orginalPosition = transform.position;
            _originalRotation = transform.rotation.eulerAngles;
        }

        public void Drop()
        {
            _rigidBody.useGravity = true;
        }


        public void Revert()
        {
            if(_rigidBody.isKinematic)
            {
                return; // already returning
            }

            _rigidBody.isKinematic = true;

            _rigidBody.useGravity = false;
            LeanTween.move(gameObject, _orginalPosition, 1f);
            LeanTween.rotate(gameObject, _originalRotation, 1f).setOnComplete(() => 
                _rigidBody.isKinematic = false);
        }

        private void OnCollisionEnter(Collision coll)
        {
            // Ignore returning bodies
            if (_rigidBody.isKinematic) return;

            // Play a click on hitting another cube, but only if the cube has a higher Id
            // to prevent the same sound being played twice
            var otherCube = coll.gameObject.GetComponent<CubeManipulator>();
            if (otherCube != null && otherCube.Id < Id)
            {
                _audioSource.PlayOneShot(bounceTogetherClip);
            }

            if (otherCube == null)
            {
                if (coll.relativeVelocity.magnitude > 0.1)
                {
                    _audioSource.PlayOneShot(bounceOtherClip);
                }
            }
        }
    }
}
