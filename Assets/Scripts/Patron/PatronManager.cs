using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatronManager : MonoBehaviour
{
  private PatronSpawner spawner;

  public static PatronManager Instance;

  void Awake()
  {
    Instance = this;
    spawner = GetComponent<PatronSpawner>();
  }
}
