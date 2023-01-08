using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SeatStatus
{
  Available,
  Occupied
}

public class Seat : MonoBehaviour
{
  public SeatStatus Status { get; private set; }

  void Awake()
  {
    Status = SeatStatus.Available;
  }

  public void Occupy()
  {
    Status = SeatStatus.Occupied;
  }

  public void LeaveSeat()
  {
    Status = SeatStatus.Available;
  }
}
