using UnityEngine;

public class FollowCamera : MonoBehaviour
{
	public Transform m_target;
	public float m_smooth = 6;

	Vector3 m_delta;

	void Start()
	{
		m_delta = transform.position - m_target.position;
	}
	
	void Update()
	{
		transform.position = Vector3.Lerp(transform.position, m_target.position + m_delta, Time.deltaTime * m_smooth);
	}
}
