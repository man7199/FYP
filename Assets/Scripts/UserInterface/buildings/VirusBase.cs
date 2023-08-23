using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirusBase : Building
{
    public Pathogen[] units;

    public Unit SpawnRandom()
    {
        return Runner.Spawn(RandomUnit(), RandomLocation(), Quaternion.identity);
    }

    private Pathogen RandomUnit()
    {
        return units[Random.Range(0, units.Length - 1)];
    }

    private Vector3 RandomLocation()
    {
        Vector3 randomTop = transform.position + new Vector3(Random.Range(21, 25), 0, Random.Range(-5, 25));
        Vector3 randomBottom = transform.position + new Vector3(Random.Range(-5, -1), 0, Random.Range(-5, 25));
        Vector3 randomLeft = transform.position + new Vector3(Random.Range(-5, 25), 0, Random.Range(-5, -1));
        Vector3 randomRight = transform.position + new Vector3(Random.Range(-5, 25), 0, Random.Range(21, 25));
        Vector3[] location = { randomTop, randomBottom, randomLeft, randomRight };
        return location[Random.Range(0, 3)];
    }
}
