using MixedReality.Toolkit;
using MixedReality.Toolkit.Subsystems;
using RealityCollective.ServiceFramework.Services;
using UnityEngine;
using UnityEngine.XR;

namespace MRTKExtensions.Manipulation
{
    [System.Runtime.InteropServices.Guid("e072c09b-2295-413b-9908-61ae7ba52d90")]
	public class HandSmashingService : BaseServiceWithConstructor, IHandSmashingService
	{
		private readonly HandSmashingServiceProfile handSmashingServiceProfile;

        public HandSmashingService(string name,  uint priority,  HandSmashingServiceProfile profile) : base(name, priority) 
		{
			handSmashingServiceProfile = profile;
            handsAggregatorSubsystem = XRSubsystemHelpers.GetFirstRunningSubsystem<HandsAggregatorSubsystem>();
        }

        private Vector3? lastRightPosition;
        private Vector3? lastLeftPosition;
        private readonly HandsAggregatorSubsystem handsAggregatorSubsystem;


        public override void Update()
        {
            base.Update();
            lastRightPosition = ApplySmashMovement( Handedness.Right, lastRightPosition);
            lastLeftPosition = ApplySmashMovement( Handedness.Left, lastLeftPosition);
        }

        private Vector3? ApplySmashMovement(Handedness handedness, Vector3? previousHandLocation)
        {
            Vector3? currentHandPosition = null;
            if ((handSmashingServiceProfile.TrackedHands & handedness) == handedness)
            {
                var handPosition = TryGetHandPosition(handedness, 
                    TrackedHandJoint.Palm, TrackedHandJoint.Wrist, 
                    TrackedHandJoint.ThumbTip, TrackedHandJoint.MiddleTip, TrackedHandJoint.LittleTip);
                if (handPosition != null)
                {
                    currentHandPosition = handPosition;
                    TryApplyForceFromVectors(previousHandLocation, currentHandPosition);
                }
            }

            return currentHandPosition;
        }
        
        private Vector3? TryGetHandPosition(Handedness handedness, params TrackedHandJoint[] joints)
        {
            foreach (var joint in joints)
            {
                if (handsAggregatorSubsystem.TryGetJoint(joint,
                    handedness == Handedness.Right ? XRNode.RightHand : XRNode.LeftHand,
                    out var handJointPose))
                {
                    return handJointPose.Position;
                }
            }

            return null;
        }

        private void TryApplyForceFromVectors(Vector3? previousHandLocation, Vector3? currentHandPosition)
        {
            if (previousHandLocation != null && currentHandPosition != null)
            {
                var handVector = currentHandPosition.Value - previousHandLocation.Value;
                var distanceMoved = Mathf.Abs(handVector.magnitude);
                if (Physics.SphereCast(currentHandPosition.Value, handSmashingServiceProfile.SmashAreaSize, 
                                     handVector, out var hitInfo, 
                                     distanceMoved * handSmashingServiceProfile.ProjectionDistanceMultiplier))                
                {
                    if (hitInfo.rigidbody != null)
                    {
                        hitInfo.rigidbody.AddForceAtPosition(
                            handVector * handSmashingServiceProfile.ForceMultiplier,
                                 hitInfo.transform.position);
                    }
                }
            }
        }
	}
}
