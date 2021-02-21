using UnityEngine;

namespace MotionMatching
{
    public struct MMTransform
    {
	    public Vector3 Position;
	    public Quaternion Rotation;

	    public MMTransform(Vector3 pos, Quaternion rot)
	    {
		    Position = pos;
		    Rotation = rot;
	    }
    }
}
