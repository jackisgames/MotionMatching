using MotionMatching;
using UnityEngine;

class SampleMotionMatching : MonoBehaviour
{
	private const int PRECISION = 10;

	[SerializeField]
	private Camera m_camera;

	[SerializeField]
	private LineRenderer m_lineRenderer;

	[SerializeField]
	private MMService m_service;

	[SerializeField]
	private MMAgent m_agent;

	[SerializeField]
	private AnimationCurve m_curve;

	private MMRequest m_request = new MMRequest();
	private Quaternion m_targetRotation = Quaternion.identity;

    private void Start()
    {
	    m_request.Agent = m_agent;
	    m_request.TargetDuration = .5f;
		m_request.TargetTransforms = new CircularBuffer<MMTransform>( PRECISION );
	    m_lineRenderer.positionCount = PRECISION + 1;

	    m_targetRotation = transform.rotation;
    }

    private void Update()
	{

		float inputX = Input.GetAxis("Horizontal");
		float inputY = Input.GetAxis("Vertical");
		Vector3 targetDirectionScreenSpace = new Vector3( inputX, 0, inputY );
		float inputMag = targetDirectionScreenSpace.magnitude;
		
		if (inputMag < .05f)
		{
			inputMag = 0;
			m_targetRotation = transform.rotation;
        }
		else
		{
			Vector3 targetDirectionWorldSpace = Vector3.ProjectOnPlane(m_camera.transform.rotation * targetDirectionScreenSpace, Vector3.up);
            m_targetRotation = Quaternion.LookRotation(targetDirectionWorldSpace);


			if (inputMag < .2f)
			{
				//to simulate turn in place, not really necessary
				inputMag = 0;
			}

        }

        Vector3 prevPosition = transform.position;
		SetLine( 0 , transform.position );
		for (int i = 0; i < PRECISION; ++i)
		{
			float amount = m_curve.Evaluate( (float)(i + 1) / PRECISION );
			Quaternion rot = Quaternion.Lerp( transform.rotation, m_targetRotation, amount );
			Vector3 pos = transform.position + (rot * Vector3.forward) * inputMag * 3 * amount;

			RaycastHit hit;
			if (Physics.Raycast(pos + new Vector3(0, 10, 0), Vector3.down, out hit))
			{
				pos = hit.point;
			}

			Vector3 translationMS = transform.InverseTransformPoint(pos);
			Quaternion rotationMS = rot * Quaternion.Inverse(transform.rotation);

			m_request.TargetTransforms.Push(new MMTransform(translationMS, rotationMS));

			SetLine( i + 1, pos );
			prevPosition = pos;
		}

		if (m_agent.IsBlending == false)
		{
			m_service.Request(m_request);
        }

		m_camera.transform.LookAt(transform.position);

	}

	private void SetLine(int index, Vector3 pos)
	{
		pos.y = pos.y + .3f;
		m_lineRenderer.SetPosition( index, pos );
	}
}
