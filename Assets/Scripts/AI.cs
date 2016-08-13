using UnityEngine;

public class AI : MonoBehaviour
{
	public float m_WalkSpeed = 3.5f;
	public float m_RunSpeed = 7;
	public float m_SearchRange = 10;
	public float m_AttackRange = 1.5f;

	Role m_role;
	Role m_target;

	enum State
	{
		Search,
		Move,
		Attack,
	}

	State m_state = State.Search;
	float m_elapsedTime = 0;

	void Start()
	{
		m_role = GetComponent<Role>();
	}

	float DistanceToTarget()
	{
		Vector3 delta = m_target.transform.position - transform.position;
		delta.y = 0;
		return delta.magnitude;
	}

	void Update()
	{
		if (!m_role.IsAlive())
			return;

		switch (m_state)
		{
			case State.Search:
				{
					m_target = World.FindNearsetEnemy(m_role, m_SearchRange);
					if (m_target != null)
					{
						m_state = State.Move;
					}
				}
				break;
			case State.Move:
				{
					m_target = World.FindNearsetEnemy(m_role, m_SearchRange);
					if (m_target == null)
						break;

					if (!m_target.IsAlive())
					{
						m_target = null;
						m_state = State.Search;
						break;
					}

					float distance = DistanceToTarget();

					if (distance > m_SearchRange)
					{
						m_state = State.Search;
						m_role.Stand();
					}
					else if (distance < m_AttackRange)
					{
						m_state = State.Attack;
						m_elapsedTime = 0;
						m_role.Attack();
					}
					else
					{
						m_role.Walk();

						float moveSpeed = m_WalkSpeed;
						float rotateSpeed = moveSpeed * 3;

						Vector3 dir = m_target.transform.position - transform.position;
						dir = dir.normalized;

						Vector3 motion = dir * m_WalkSpeed * Time.deltaTime;
						motion.y = -5;

						m_role.Move(motion);
						m_role.Lookat(dir, rotateSpeed * Time.deltaTime);
					}
				}
				break;
			case State.Attack:
				{
					if (!m_target.IsAlive())
					{
						m_target = null;
						m_state = State.Search;
						break;
					}

					m_elapsedTime += Time.deltaTime;
					if (m_elapsedTime > 2)
					{
						m_state = State.Move;
					}
				}
				break;
			default:
				break;
		}
	}
}
