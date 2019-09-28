using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
	[SerializeField] List<WaveConfig> waveConfigs;
    [SerializeField] bool looping = false;
	int startingWave = 0;
    // Start is called before the first frame update
    IEnumerator Start()
    {
        do
        {
            yield return StartCoroutine(SpawnAllWaves());
        }
        while (looping);
    }
    private IEnumerator SpawnAllWaves()
    {
        for (int i = 0; i < waveConfigs.Count; i++)
        {
            yield return StartCoroutine(SpawnAllEnemiesInWave(waveConfigs[i]));
        }
    }
	private IEnumerator SpawnAllEnemiesInWave(WaveConfig waveConfig)
	{
        var waypoints = waveConfig.GetWaypoints();
        for (int i = 0; i < waveConfig.GetEnemyCount(); i++)
        {
            // vertical flip enemies
            var rotationVector = new Vector3(0, 0, 180);
            var enemy = Instantiate(
                waveConfig.GetEnemyPrefab(),
                waveConfig.GetWaypoints()[0].transform.position,
                Quaternion.Euler(rotationVector)
            );
            
            enemy.GetComponent<EnemyPathing>().SetWaveConfig(waveConfig);
            yield return new WaitForSeconds(waveConfig.GetTimeBetweenSpawns());
        }
	}
}
