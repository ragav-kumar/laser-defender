using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPathing : MonoBehaviour
{
    //Config
    WaveConfig waveConfig;
    //State
    List<Transform> waypoints;
    private int waypointIndex = 0;
	// Start is called before the first frame update
	void Start()
	{
        if (waveConfig != null)
        {
            waypoints = waveConfig.GetWaypoints();
            transform.position = waypoints[waypointIndex].transform.position;
        }
	}
	// Update is called once per frame
	void Update()
	{
        if (waypoints != null)
        {
            Move();
        }
	}
    public void SetWaveConfig(WaveConfig waveConfig)
    {
        this.waveConfig = waveConfig;
        waypoints = waveConfig.GetWaypoints();
        transform.position = waypoints[waypointIndex].transform.position;
    }
	private void Move()
	{
		if (waypointIndex < waypoints.Count)
		{
			var targetPos = waypoints[waypointIndex].transform.position;
			var movementThisFrame = waveConfig.GetMoveSpeed() * Time.deltaTime;
			transform.position = Vector2.MoveTowards(transform.position, targetPos, movementThisFrame);
			if (transform.position == targetPos)
			{
				waypointIndex++;
			}
		}
		else
		{
			Destroy(gameObject);
		}
	}
}
