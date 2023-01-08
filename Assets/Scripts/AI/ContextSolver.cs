using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContextSolver : MonoBehaviour
{
  [SerializeField]
  private bool showGizmos = true;

  //gozmo parameters
  float[] interestGizmo = new float[0];
  Vector2 resultDirection = Vector2.zero;
  private float rayLength = 2;

  private void Start()
  {
    interestGizmo = new float[Directions.numDirections];
  }

  public Vector2 GetDirectionToMove(List<SteeringBehaviour> behaviours, AIData aiData)
  {
    float[] danger = new float[Directions.numDirections];
    float[] interest = new float[Directions.numDirections];
    float[] adjustedInterest = new float[Directions.numDirections];

    //Loop through each behaviour
    foreach (SteeringBehaviour behaviour in behaviours)
    {
      (danger, interest) = behaviour.GetSteering(danger, interest, aiData);
    }

    //subtract danger values from interest array
    for (int i = 0; i < Directions.numDirections; i++)
    {
      adjustedInterest[i] = Mathf.Clamp01(interest[i] - danger[i]);
    }

    // // Find the strongest interest direction
    // float maxInterest = 0;
    // int maxInterestIndex = 0;
    // for (int i = 0; i < Directions.numDirections; i++)
    // {
    //   if (adjustedInterest[i] > maxInterest)
    //   {
    //     maxInterest = adjustedInterest[i];
    //     maxInterestIndex = i;
    //   }
    // }

    // var strongestDirection = Directions.All[maxInterestIndex];

    // // Go through the directions finding the dot product against the strongest direction
    // for (int i = 0; i < Directions.numDirections; i++)
    // {

    //   float result = Vector2.Dot(strongestDirection, Directions.All[i]);
    //   Debug.DrawRay(transform.position, Directions.All[i] * result, Color.green, 0.05f);

    //   adjustedInterest[i] = result;
    // }

    // // Reapply the dangers
    // for (int i = 0; i < Directions.numDirections; i++)
    // {
    //   adjustedInterest[i] = Mathf.Clamp01(adjustedInterest[i] - danger[i]);
    // }

    //get the average direction
    Vector2 outputDirection = Vector2.zero;
    for (int i = 0; i < Directions.numDirections; i++)
    {
      outputDirection += Directions.All[i] * adjustedInterest[i];
    }

    outputDirection.Normalize();

    resultDirection = outputDirection;

    interestGizmo = adjustedInterest;

    //return the selected movement direction
    return resultDirection;
  }


  private void OnDrawGizmos()
  {
    if (Application.isPlaying && showGizmos)
    {
      Gizmos.color = Color.yellow;
      Gizmos.DrawRay(transform.position, resultDirection * rayLength);
    }
  }
}