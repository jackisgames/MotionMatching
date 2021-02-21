
using System.Collections.Generic;
using UnityEngine;

namespace MotionMatching
{
    public class MMService : MonoBehaviour
    {
	    [SerializeField]
	    private MMAssetLibrary[] m_assets;

	    private MMQuery[] m_queries;

	    private readonly List<MMRequest> m_pendingRequests = new List<MMRequest>();

	    private void Start()
	    {
			m_queries = new MMQuery[ m_assets.Length ];

		    for (int i = 0; i < m_assets.Length; ++i)
		    {
				m_queries[i] = new MMQuery( m_assets[i] );
		    }
	    }

	    private void Update()
	    {
		    for (int i = 0; i < m_pendingRequests.Count; ++i)
		    {
			    MMRequest request = m_pendingRequests[i];
			    request.Agent.OnResult( m_queries[request.AnimDataIndex].Query(request) );
		    }

			m_pendingRequests.Clear();
	    }

	    public void Request(MMRequest request)
	    {
			m_pendingRequests.Add(request);
	    }
    }
}
