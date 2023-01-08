using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RecipesData
{
  public static List<RecipeMapping> Recipes = new List<RecipeMapping>()
  {
    new RecipeMapping()
    {
      RecipeRequirements = new List<RecipeRequirement>()
      {
        new RecipeRequirement()
        {
          Amount = 1,
          FoodType = FoodType.Leg,
        },
      },
      Result = new RecipeRequirement()
      {
        Amount = 1,
        FoodType = FoodType.Steak,
      },
    },
    new RecipeMapping()
    {
      RecipeRequirements = new List<RecipeRequirement>()
      {
        new RecipeRequirement()
        {
          Amount = 1,
          FoodType = FoodType.Arm,
        },
      },
      Result = new RecipeRequirement()
      {
        Amount = 1,
        FoodType = FoodType.Sausage,
      },
    },
  };

  public static RecipeMapping GetRandomRecipe()
  {
    return Recipes[Random.Range(0, Recipes.Count)];
  }
}
