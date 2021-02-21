using UnityEngine;

namespace MotionMatching
{
    public struct MMRequest
    {
	    public MMAgent Agent;
        public CircularBuffer<MMTransform> TargetTransforms;
	    public float TargetDuration;
        public int AnimDataIndex;
	    
    }

	public struct MMResult
	{
		public AnimationClip Clip;
		public float Time;
	}
}
