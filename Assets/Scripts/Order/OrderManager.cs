using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderManager : MonoBehaviour
{
  public static OrderManager Instance;

  void Awake()
  {
    Instance = this;
  }

  public Order PlaceRandomOrder()
  {
    var recipe = RecipesData.GetRandomRecipe();
    var order = new Order(recipe);
    return order;
  }
}
