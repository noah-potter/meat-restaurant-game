using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatronManager : MonoBehaviour
{
  private PatronSpawner spawner;
  [SerializeField]
  private int numPatronsToSpawn = 0;
  [SerializeField]
  private int patronSpawnDelay;
  private float _lastPatronSpawnTime = -999999f;

  void Awake()
  {
    spawner = GetComponent<PatronSpawner>();
  }

  void Start()
  {

  }

  void Update()
  {
    var canSpawn = Time.time - _lastPatronSpawnTime > patronSpawnDelay;
    if (numPatronsToSpawn > 0 && canSpawn)
    {
      numPatronsToSpawn -= 1;
      spawner.Spawn();
      _lastPatronSpawnTime = Time.time;
    }
  }
}
