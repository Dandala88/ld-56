using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public float radius;
    public int volume;
    public List<EnemySpawn> enemies = new List<EnemySpawn>();

    public void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    private void Start()
    {
        var totalRarity = 0f;
        foreach (var enemy in enemies)
            totalRarity += enemy.rarity;

        for (int i = 0; i < volume; i++)
        {
            Enemy selectedEnemy = enemies.First().prefab;
            float rarityRoll = UnityEngine.Random.value * totalRarity;
            foreach (var enemy in enemies)
            {
                if (rarityRoll < enemy.rarity)
                {
                    selectedEnemy = enemy.prefab;
                    break;
                }
                rarityRoll -= enemy.rarity;
            }
            Vector3 randomDirection = UnityEngine.Random.onUnitSphere;
            float randomDistance = Mathf.Pow(UnityEngine.Random.value, 1f / 3f) * radius;
            var clone = Instantiate(selectedEnemy);
            clone.transform.position = transform.position + (randomDirection * randomDistance);
        }
    }
}

[Serializable]
public class EnemySpawn
{
    public Enemy prefab;
    [Tooltip("0 is rare, 1 is common")]
    [Range(0, 1)]
    public float rarity;
}
