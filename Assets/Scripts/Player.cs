using UnityEngine;

public class Player : MonoBehaviour
{
	public float m_moveSpeed = 7;

	Role m_role;

	enum State
	{
		Normal,
		Deading,
		Dead,
		Revive,
	}

	State m_state = State.Normal;

	float m_elapsedTime = 0;

	void Start()
	{
		m_role = GetComponent<Role>();
	}

	void Update()
	{
		switch (m_state)
		{
			case State.Normal:
				{
					if (!m_role.IsAlive())
					{
						m_elapsedTime = 0;
						m_state = State.Deading;
					}
					else
					{
						UpdateNormal();
					}
				}
				break;
			case State.Deading:
				{
					m_elapsedTime += Time.deltaTime;
					if (m_elapsedTime > 2.5f)
					{
						m_state = State.Dead;
					}
				}
				break;
			case State.Dead:
				{

				}
				break;
			case State.Revive:
				{
					m_elapsedTime += Time.deltaTime;
					if (m_elapsedTime > 2.0f)
					{
						m_role.AddHp(m_role.m_maxHp);
						m_state = State.Normal;
					}
				}
				break;
			default:
				break;
		}
	}

	void UpdateNormal()
	{
		if (Input.GetMouseButtonUp(0))
		{
			m_role.Attack();
			return;
		}

		if (Input.GetKeyUp(KeyCode.LeftShift))
		{
			m_role.UnBlock();
		}

		float x = Input.GetAxisRaw("Horizontal");
		float y = Input.GetAxisRaw("Vertical");

		if (m_role.IsAttacking())
		{
			// do nothing
		}
		else
		{
			if (x != 0 || y != 0)
			{
				m_role.Run();

				if (m_role.IsRunning())
				{
					float moveSpeed = m_moveSpeed;
					float rotateSpeed = moveSpeed * 3;

					Vector3 dir = Camera.main.transform.TransformDirection(new Vector3(x, 0, y));
					dir = dir.normalized;

					Vector3 motion = dir * moveSpeed * Time.deltaTime;
					motion.y = -5;

					m_role.Move(motion);
					m_role.Lookat(dir, rotateSpeed * Time.deltaTime);
				}
			}
			else
			{
				if (m_role.IsWalking() || m_role.IsRunning())
				{
					m_role.Stand();
				}
			}
		}

		if (Input.GetKeyDown(KeyCode.LeftShift))
		{
			m_role.Block();
		}
	}

	void OnGUI()
	{
		switch (m_state)
		{
			case State.Normal:
				break;
			case State.Deading:
				break;
			case State.Dead:
				{
					int w = 80;
					int h = 20;
					Rect rect = new Rect((Screen.width - w) / 2, (Screen.height - h) / 2, w, h);
					if (GUI.Button(rect, "复活"))
					{
						Revive();
					}
				}
				break;
			case State.Revive:
				break;
			default:
				break;
		}
	}

	void Revive()
	{
		m_elapsedTime = 0;
		m_state = State.Revive;
		m_role.Revive();
	}
}
