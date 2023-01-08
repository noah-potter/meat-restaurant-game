using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatronSpawner : MonoBehaviour
{
  [SerializeField]
  private GameObject patronPrefab;
  private List<Patron> patrons = new List<Patron>();

  void Start()
  {

  }

  void Update()
  {

  }

  public Patron Spawn()
  {
    GameObject patronObject = Instantiate(patronPrefab, Vector3.zero, Quaternion.identity);
    Patron patron = patronObject.GetComponent<Patron>();
    patrons.Add(patron);
    return patron;
  }
}
