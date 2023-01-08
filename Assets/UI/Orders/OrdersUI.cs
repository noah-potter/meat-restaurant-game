using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class OrdersUI : MonoBehaviour
{
  public OrderUI OrderPrefab;

  public Dictionary<Order, OrderUI> RenderedOrderUIs = new Dictionary<Order, OrderUI>();

  public void AddOrder(Order order)
  {
    var orderUI = Instantiate(OrderPrefab, transform).GetComponent<OrderUI>();
    orderUI.Render(order);
    RenderedOrderUIs.Add(order, orderUI);
  }

  public void RemoveOrder(Order order)
  {
    var orderUI = RenderedOrderUIs[order];
    RenderedOrderUIs.Remove(order);
    Destroy(orderUI.gameObject);
  }
}
