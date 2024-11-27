using RealityCollective.ServiceFramework.Definitions;
using MixedReality.Toolkit;
using UnityEngine;

namespace MRTKExtensions.Manipulation
{
    [CreateAssetMenu(menuName = nameof(HandSmashingServiceProfile), fileName = nameof(HandSmashingServiceProfile),
        order = (int)CreateProfileMenuItemIndices.ServiceConfig)]
	public class HandSmashingServiceProfile :  BaseServiceProfile<IHandSmashingService>
    {
        [SerializeField]
        private float forceMultiplier = 100;

        public float ForceMultiplier => forceMultiplier;

        [SerializeField]
        private float smashAreaSize = 0.02f;

        public float SmashAreaSize => smashAreaSize;

        [SerializeField]
        private float projectionDistanceMultiplier = 1.1f;

        public float ProjectionDistanceMultiplier => projectionDistanceMultiplier;

        [SerializeField] 
        private Handedness trackedHands = Handedness.Both;

        public Handedness TrackedHands => trackedHands;
    }
}