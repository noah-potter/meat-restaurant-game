using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderUI : MonoBehaviour
{
  public GameObject ResultImagePrefab;
  public GameObject RequirementImagePrefab;
  public GameObject RequirementsRow;
  public GameObject ResultsRow;

  public void Render(Order order)
  {
    order.Recipe.RecipeRequirements.ForEach(requirement =>
    {
      var image = Instantiate(RequirementImagePrefab, RequirementsRow.transform).transform.GetComponent<UnityEngine.UI.Image>();
      image.sprite = FoodToSpriteManager.Instance.FoodToSpriteMap[requirement.FoodType];
    });

    var image = Instantiate(ResultImagePrefab, ResultsRow.transform).transform.GetComponent<UnityEngine.UI.Image>();
    image.sprite = FoodToSpriteManager.Instance.FoodToSpriteMap[order.Recipe.Result.FoodType];
  }
}
