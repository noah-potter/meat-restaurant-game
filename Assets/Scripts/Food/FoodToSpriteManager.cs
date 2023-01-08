using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodToSpriteManager : MonoBehaviour
{
  public static FoodToSpriteManager Instance;

  public UDictionary<FoodType, Sprite> FoodToSpriteMap;

  void Awake()
  {
    Instance = this;
  }
}
