#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;

namespace MotionMatching
{
	[System.Serializable]
	class MMAnimData
    {
		public AnimationCurve[] Motions = new AnimationCurve[ 7 ];

	    public Vector3 GetMotionTranslationAtTime( float time )
	    {
			return new Vector3(
				Motions[0].Evaluate( time ),
				Motions[1].Evaluate( time ),
				Motions[2].Evaluate( time )
                );
	    }

	    public Quaternion GetMotionOrientationAtTime(float time)
	    {
		    return new Quaternion(
			    Motions[3].Evaluate(time),
			    Motions[4].Evaluate(time),
			    Motions[5].Evaluate(time),
			    Motions[6].Evaluate(time)
		    );
        }
	}
	[CreateAssetMenu(menuName = "MotionMatching/AssetLibrary")]
	class MMAssetLibrary : ScriptableObject
    {
	    [SerializeField]
	    public AnimationClip[] Animations;

	    [SerializeField]
	    public MMAnimData[] AnimDatas;

#if UNITY_EDITOR
        private void OnValidate()
	    {
			AnimDatas = new MMAnimData[Animations.Length];

		    for (int i = 0; i < AnimDatas.Length; ++i)
		    {
			    AnimationClip clip = Animations[i];
			    if (clip != null)
			    {
				    MMAnimData MMdata = new MMAnimData();
				    AnimDatas[i] = MMdata;

				    EditorCurveBinding[] bindings = AnimationUtility.GetCurveBindings(clip);

				    for (int j = 0; j < 7; ++j)
				    {
					    MMdata.Motions[j] = AnimationUtility.GetEditorCurve(clip, bindings[j]);
				    }
                }
		    }
        }
#endif
    }
}
