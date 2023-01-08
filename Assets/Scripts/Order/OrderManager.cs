using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderManager : MonoBehaviour
{
  public static OrderManager Instance;

  [SerializeField]
  private OrdersUI OrdersUI;

  void Awake()
  {
    Instance = this;
  }

  public Order PlaceRandomOrder()
  {
    var recipe = RecipesData.GetRandomRecipe();
    var order = new Order(recipe);
    OrdersUI.AddOrder(order);
    return order;
  }

  public void RemoveOrder(Order order)
  {
    OrdersUI.RemoveOrder(order);
  }
}
