using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Patron : MonoBehaviour
{
  private Order _order;
  private float _timeOrdered;
  private float _timeToWait;
  [SerializeField]
  private GameObject orderCanvasPrefab;
  private GameObject orderCanvas;

  private System.Action<Patron> _onOrderComplete;

  public void Initialize(float timeToWait, System.Action<Patron> onOrderComplete)
  {
    _timeToWait = timeToWait;
    _onOrderComplete = onOrderComplete;
  }

  void Start()
  {
    // Find a target seat to sit at
    var seats = GameObject.FindObjectsOfType<Seat>();

    ShuffleSeats(seats);

    // Loop through seats looking for an empty one that no one has reserved
    foreach (var seat in seats)
    {
      if (seat.Status == SeatStatus.Available)
      {
        // Reserve the seat
        seat.Occupy();

        // Move to the seat
        transform.position = seat.transform.position;

        // Place an order
        _order = OrderManager.Instance.PlaceRandomOrder();
        _timeOrdered = Time.time;

        orderCanvas = Instantiate(orderCanvasPrefab, transform);
        var orderUI = orderCanvas.GetComponent<OrderUI>();
        orderUI.Render(_order);

        orderCanvas.transform.position = transform.position + new Vector3(0, 1);

        break;
      }
    }
  }

  void Update()
  {
    if (_order != null && Time.time - _timeOrdered > _timeToWait)
    {
      // Remove the order
      _order = null;
      _onOrderComplete(this);
    }
  }

  void ShuffleSeats(Seat[] seats)
  {
    for (int i = 0; i < seats.Length; i++)
    {
      Seat temp = seats[i];
      int randomIndex = Random.Range(i, seats.Length);
      seats[i] = seats[randomIndex];
      seats[randomIndex] = temp;
    }
  }

  void OnDestroy()
  {
    Destroy(orderCanvas);
  }
}
