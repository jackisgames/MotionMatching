using MotionMatching;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

public class MMAgent : MonoBehaviour
{
	public AnimationClip CurrentAnimClip
	{
		get { return m_currentClip.IsNull()? null : m_currentClip.GetAnimationClip();  }
	}

	public double CurrentTime
	{
		get { return m_currentClip.IsNull() ? 0 : m_currentClip.GetTime(); }
	}

	[SerializeField]
	private float m_blendTime = .5f;

	private AnimationClipPlayable m_currentClip;

    private AnimationMixerPlayable m_mixer;
	private AnimationPlayableOutput m_output;
    private PlayableGraph m_graph;
	private float m_weight = 1.0f;

    private void Start()
    {
		m_graph = PlayableGraph.Create();
	    m_graph.SetTimeUpdateMode(DirectorUpdateMode.GameTime);
        
	    m_mixer = AnimationMixerPlayable.Create(m_graph, 2);
        m_output = AnimationPlayableOutput.Create(m_graph, "Animation", GetComponent<Animator>());
	    m_output.SetSourcePlayable( m_mixer );
        m_graph.Play();
    }

    private void Update()
    {
        // simple blend only 2 pose
	    if (m_weight < 1.0f)
	    {
		    m_weight = Mathf.MoveTowards( m_weight, 1.0f, Time.deltaTime / m_blendTime );

		    m_mixer.SetInputWeight(0, m_weight);
		    m_mixer.SetInputWeight(1, 1 - m_weight);
        }
    }

	private void OnDestroy()
	{
		m_currentClip.Destroy();
		m_mixer.Destroy();
		m_graph.Destroy();
	}

	public bool IsBlending
	{
		get { return m_weight < 1.0f; }
	}

    public void OnResult( MMResult result )
	{

		if (result.Clip == null )
		{
			return;
		}
		bool needBlend = m_currentClip.IsNull() == false ;

        if (needBlend)
		{

			if (result.Clip == m_currentClip.GetAnimationClip())
			{
				return;
			}

            AnimationClipPlayable blendOutClip = m_currentClip;
			m_mixer.DisconnectInput(0);
			m_mixer.DisconnectInput(1);
            m_mixer.ConnectInput(1, blendOutClip, 0, 1.0f);
			m_weight = 0.0f;
			m_mixer.SetInputWeight(0, 0.0f);
			m_mixer.SetInputWeight(1, 1.0f);
        }
		else
		{

            m_mixer.SetInputWeight(0, 1.0f);
			m_mixer.SetInputWeight(1, 0.0f);
        }

		m_currentClip = AnimationClipPlayable.Create( m_graph, result.Clip );
        m_currentClip.SetTime( result.Time );

		m_mixer.DisconnectInput(0);
        m_mixer.ConnectInput(0, m_currentClip, 0, needBlend? 0.0f : 1.0f);

    }
}
