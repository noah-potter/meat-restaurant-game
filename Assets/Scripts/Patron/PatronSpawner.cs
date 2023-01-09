using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatronSpawner : MonoBehaviour
{
  [SerializeField]
  private int numPatronsToSpawn = 0;
  [SerializeField]
  private float timeToWait = 30f;
  [SerializeField]
  private GameObject patronPrefab;
  private List<Patron> patrons = new List<Patron>();
  [SerializeField]
  private int patronSpawnDelay;
  private float _lastPatronSpawnTime = -999999f;

  void Update()
  {
    var canSpawn = Time.time - _lastPatronSpawnTime > patronSpawnDelay;
    if (numPatronsToSpawn > 0 && canSpawn)
    {
      Spawn();
    }
  }

  public Patron Spawn()
  {
    numPatronsToSpawn -= 1;
    _lastPatronSpawnTime = Time.time;
    GameObject patronObject = Instantiate(patronPrefab, Vector3.zero, Quaternion.identity);
    Patron patron = patronObject.GetComponent<Patron>();
    patron.Initialize(timeToWait, RemovePatron);
    patrons.Add(patron);
    return patron;
  }

  public void RemovePatron(Patron patron)
  {
    patrons.Remove(patron);
    Destroy(patron.gameObject);
    numPatronsToSpawn += 1;
  }
}
