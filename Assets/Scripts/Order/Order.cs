using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Order
{
  public RecipeMapping Recipe { get; private set; }

  public Order(RecipeMapping recipe)
  {
    Recipe = recipe;
  }
}
