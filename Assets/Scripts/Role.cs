using System.Collections.Generic;
using UnityEngine;

public class Role : MonoBehaviour
{
	public int m_maxHp = 100;
	public int m_atk = 5;

	float m_hp = 0;

	Team m_team = Team.Red;

	public Team team
	{
		get { return m_team; }
		set { m_team = value; }
	}

	Animator m_animator;
	CharacterController m_cc;
	AnimatorStateInfo m_currentState;
	AnimatorStateInfo m_nextState;

	void Start()
	{
		m_animator = GetComponent<Animator>();
		m_cc = GetComponent<CharacterController>();
		m_hp = m_maxHp;
	}

	void Update()
	{
		m_currentState = m_animator.GetCurrentAnimatorStateInfo(0);
		m_nextState = m_animator.GetNextAnimatorStateInfo(0);
	}

	void ResetTriggers()
	{
		m_animator.ResetTrigger("Stand");
		m_animator.ResetTrigger("Walk");
		m_animator.ResetTrigger("Run");
		m_animator.ResetTrigger("Attack");
		m_animator.ResetTrigger("Hit");
		m_animator.ResetTrigger("Dead");
		m_animator.ResetTrigger("Revive");
		m_animator.ResetTrigger("Block");
	}

	public void Stand()
	{
		ResetTriggers();
		m_animator.SetTrigger("Stand");
	}

	public void Walk()
	{
		ResetTriggers();
		m_animator.SetTrigger("Walk");
	}

	public void Run()
	{
		ResetTriggers();
		m_animator.SetTrigger("Run");
	}

	public void Attack()
	{
		ResetTriggers();
		m_animator.SetTrigger("Attack");
	}

	public void Block()
	{
		ResetTriggers();
		m_animator.SetBool("Blocking", true);
		m_animator.SetTrigger("Block");
	}

	public void UnBlock()
	{
		ResetTriggers();
		m_animator.SetBool("Blocking", false);
		m_animator.SetTrigger("Stand");
	}

	public void Hit(Role from)
	{
		m_hp -= from.m_atk;
		if (m_hp <= 0)
		{
			Dead();
			return;
		}

		ResetTriggers();
		m_animator.SetTrigger("Hit");
	}

	public void Dead()
	{
		m_hp = 0;
		ResetTriggers();
		m_animator.SetTrigger("Dead");
	}

	public void Revive()
	{
		ResetTriggers();
		m_animator.SetTrigger("Revive");
	}

	public void AddHp(int hp)
	{
		m_hp = hp;
	}

	public bool IsDuring(string action)
	{
		return m_currentState.IsTag(action);
	}

	public void Move(Vector3 motion)
	{
		m_cc.Move(motion);
	}

	public bool IsWalking()
	{
		return (m_currentState.IsTag("Walk") && !m_nextState.IsTag("Stand")) || m_nextState.IsTag("Walk");
	}

	public bool IsRunning()
	{
		return (m_currentState.IsTag("Run") && !m_nextState.IsTag("Stand")) || m_nextState.IsTag("Run");
	}

	public bool IsAttacking()
	{
		return m_currentState.IsTag("Attack") || m_nextState.IsTag("Attack");
	}

	public bool IsAlive()
	{
		return m_hp > 0;
	}

	public void Lookat(Vector3 dir, float lerp)
	{
		dir.y = 0;
		Quaternion lookAtRotation = Quaternion.LookRotation(dir, Vector3.up);
		transform.rotation = Quaternion.Slerp(transform.rotation, lookAtRotation, lerp);
	}

	void Hit(float range, float angle)
	{
		List<Role> units = World.FindNearbyEnemy(this, range);
		Vector3 forward = transform.forward;
		forward.y = 0;
		foreach (var unit in units)
		{
			Vector3 delta = unit.transform.position - transform.position;
			delta.y = 0;
			if (Vector3.Angle(forward, delta) < angle)
			{
				unit.Hit(this);
			}
		}
	}

	void OnAttackHit(int id)
	{
		switch (id)
		{
			case 1:
				{
					Hit(2.5f, 90);
				}
				break;
			case 2:
				{
					Hit(2.5f, 90);
				}
				break;
			case 3:
				{
					Hit(4, 90);
				}
				break;
			default:
				{
					Debug.Log(string.Format("{0}OnAttackHit({1})", name, id));
				}
				break;
		}
	}

	void OnGUI()
	{
		Vector3 offset = new Vector3(0, 2.5f, 0);
		Vector3 pos = Camera.main.WorldToScreenPoint(transform.position + offset);
		float x = pos.x;
		float y = Screen.height - pos.y;
		float w = 25;
		float h = 20;
		Rect rect = new Rect(x - w / 2, y - h / 2, w, h);
		GUI.Label(rect, m_hp.ToString());
	}
}
