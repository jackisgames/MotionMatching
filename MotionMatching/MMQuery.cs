using UnityEngine;

namespace MotionMatching
{
    class MMQuery
    {
	    private readonly MMAssetLibrary m_asset;
	    
	    public MMQuery(MMAssetLibrary asset)
	    {
		    m_asset = asset;
	    }

	    public MMResult Query(MMRequest request)
	    {
		    MMResult result = new MMResult();
			//r
		    float timeSlice = request.TargetDuration / request.TargetTransforms.Length;
		    float bestMatch = 999;
		    for (int i = 0; i < m_asset.AnimDatas.Length; ++i)
		    {
			    AnimationClip clip = m_asset.Animations[i];

			    if (clip == null)
			    {
				    continue;
			    }

			    MMAnimData data = m_asset.AnimDatas[i];
                bool isCurrentAnim = clip == request.Agent.CurrentAnimClip;
			    if (isCurrentAnim)
			    {
				    if (clip.isLooping == false && request.Agent.CurrentTime >= clip.length)
				    {
						continue;
				    }
			    }

				
			    int slices = Mathf.CeilToInt(clip.length / request.TargetDuration);
			    float sliceOffset = clip.length / slices;

                for ( int slice = 0; slice < slices; ++slice )
			    {
				    float currentMatch = 0;
				    float offset = sliceOffset * slice;
				    for (int j = 0; j < request.TargetTransforms.Length; ++j)
				    {
					    MMTransform target = request.TargetTransforms[j];
					    float currentTime = offset + (j + 1) * timeSlice;
					    Vector3 currentVelocity = data.GetMotionTranslationAtTime(currentTime);
					    Quaternion currentRotation = data.GetMotionOrientationAtTime(currentTime);
					    float deltaAngle = Quaternion.Angle(currentRotation, target.Rotation) / 180.0f;
					    Vector3 deltaTranslation = currentVelocity - target.Position;

					    currentMatch += (deltaTranslation.sqrMagnitude + deltaAngle) * (currentTime / request.TargetDuration);

				    }

				    if (currentMatch < bestMatch)
				    {
					    result.Clip = clip;
					    result.Time = offset;
                        bestMatch = currentMatch;
				    }
                }
			    
		    }
		    return result;
	    }
    }
}
