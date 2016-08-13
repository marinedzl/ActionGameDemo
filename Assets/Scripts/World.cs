using System.Collections.Generic;
using UnityEngine;

public enum Team
{
	Red,
	Blue,
	Count,
}

public class World : MonoBehaviour
{
	static World s_instance;

	List<Role>[] m_units;
	List<Role> m_erase;

	void Awake()
	{
		s_instance = this;
	}

	void Update()
	{
		for (int teamIndex = 0; teamIndex < (int)Team.Count; teamIndex++)
		{
			List<Role> units = m_units[teamIndex];
			foreach (var unit in units)
			{
				if (unit.CompareTag("Player")) // 跳过主角
					continue;
				if (!unit.IsAlive())
				{
					Destroy(unit.gameObject, 4);
					m_erase.Add(unit);
				}
			}
			foreach (var unit in m_erase)
				units.Remove(unit);
			m_erase.Clear();
		}
	}

	void Start()
	{
		m_erase = new List<Role>();
		m_units = new List<Role>[(int)Team.Count];

		for (int teamIndex = 0; teamIndex < (int)Team.Count; teamIndex++)
		{
			m_units[teamIndex] = new List<Role>();
			Team team = (Team)teamIndex;
			Transform units = transform.FindChild(team.ToString());
			int count = units.childCount;
			for (int i = 0; i < count; i++)
			{
				Role role = units.GetChild(i).gameObject.GetComponent<Role>();
				if (role != null)
				{
					role.team = team;
					m_units[teamIndex].Add(role);
				}
			}
		}
	}

	public Role FindNearsetEnemyInternal(Role self, float range)
	{
		Role target = null;
		float min = float.MaxValue;

		for (int i = 0; i < (int)Team.Count; i++)
		{
			Team team = (Team)i;
			if (team == self.team) // 阵营相同，跳过
				continue;

			List<Role> units = m_units[i];

			foreach (var unit in units)
			{
				if (!unit.IsAlive())
					continue;

				Vector3 delta = unit.transform.position - self.transform.position;
				delta.y = 0;
				float distance = delta.magnitude;

				if (distance > range) // 超出距离，跳过
					continue;

				if (distance < min)
				{
					target = unit;
					min = distance;
				}
			}
		}

		return target;
	}

	public List<Role> FindNearbyEnemyInternal(Role self, float range)
	{
		List<Role> targets = new List<Role>();

		for (int i = 0; i < (int)Team.Count; i++)
		{
			Team team = (Team)i;
			if (team == self.team) // 阵营相同，跳过
				continue;

			List<Role> units = m_units[i];

			foreach (var unit in units)
			{
				if (!unit.IsAlive())
					continue;

				Vector3 delta = unit.transform.position - self.transform.position;
				delta.y = 0;
				float distance = delta.magnitude;

				if (distance > range) // 超出距离，跳过
					continue;

				targets.Add(unit);
			}
		}

		return targets;
	}

	public static Role FindNearsetEnemy(Role self, float range)
	{
		return s_instance.FindNearsetEnemyInternal(self, range);
	}

	public static List<Role> FindNearbyEnemy(Role self, float range)
	{
		return s_instance.FindNearbyEnemyInternal(self, range);
	}
}
