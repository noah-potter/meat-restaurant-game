using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleAvoidanceBehaviour : SteeringBehaviour
{
  [SerializeField]
  private float radius = 2f, agentColliderSize = 0.6f;

  [SerializeField]
  private bool showGizmo = true;

  //gizmo parameters
  float[] dangersResultTemp = null;

  public override (float[] danger, float[] interest) GetSteering(float[] danger, float[] interest, AIData aiData)
  {
    for (int i = 0; i < Directions.All.Count; i++)
    {
      var ray = Physics2D.Raycast(transform.position, Directions.All[i], radius, LayerMask.GetMask("Obstacle"));

      if (ray.collider == null)
      {
        // danger[i] = 0;
        continue;
      }

      var x = ray.distance - agentColliderSize;
      var y = radius - agentColliderSize;

      float weight = ray.distance <= agentColliderSize
        ? 1
        : (y - x) / y;

      //Add obstacle parameters to the danger array
      for (int j = 0; j < Directions.All.Count; j++)
      {
        var magnitude = Vector2.Dot(Directions.All[i], Directions.All[j]);
        float result = magnitude;

        float valueToPutIn = result * weight;

        //override value only if it is higher than the current one stored in the danger array
        // Debug.LogFormat("ObstacleAvoidanceBehaviour: {0}", valueToPutIn);
        if (valueToPutIn > danger[j])
        {
          danger[j] = valueToPutIn;
        }
      }

      // danger[i] = weight;
    }

    // foreach (Collider2D obstacleCollider in aiData.obstacles)
    // {
    //   Vector2 directionToObstacle
    //       = obstacleCollider.ClosestPoint(transform.position) - (Vector2)transform.position;
    //   float distanceToObstacle = directionToObstacle.magnitude;

    //   //calculate weight based on the distance Enemy<--->Obstacle
    //   float weight
    //       = distanceToObstacle <= agentColliderSize
    //       ? 1
    //       : (radius - distanceToObstacle) / radius;

    //   Vector2 directionToObstacleNormalized = directionToObstacle.normalized;

    //   //Add obstacle parameters to the danger array
    //   for (int i = 0; i < Directions.All.Count; i++)
    //   {
    //     float result = Vector2.Dot(directionToObstacleNormalized, Directions.All[i]);

    //     float valueToPutIn = result * weight;

    //     //override value only if it is higher than the current one stored in the danger array
    //     Debug.LogFormat("ObstacleAvoidanceBehaviour: {0}", valueToPutIn);
    //     if (valueToPutIn > danger[i])
    //     {
    //       danger[i] = valueToPutIn;
    //     }
    //   }
    // }

    dangersResultTemp = danger;
    return (danger, interest);
  }

  private void OnDrawGizmos()
  {
    if (showGizmo == false)
      return;

    Gizmos.DrawWireSphere(transform.position, radius);

    if (Application.isPlaying && dangersResultTemp != null)
    {
      if (dangersResultTemp != null)
      {
        Gizmos.color = Color.red;
        for (int i = 0; i < dangersResultTemp.Length; i++)
        {
          Gizmos.DrawRay(
              transform.position + new Vector3(0, 0.02f),
              Directions.All[i] * dangersResultTemp[i]);
        }
      }
    }
  }
}

public static class Directions
{
  public static int numDirections = 16;
  private static List<Vector2> directions = null;
  public static List<Vector2> All
  {
    get
    {
      if (directions == null)
      {
        directions = GetDirections();
      }
      return directions;
    }
  }

  public static List<Vector2> GetDirections()
  {
    List<Vector2> directions = new List<Vector2>();

    for (int i = 0; i < numDirections; i++)
    {
      float angle = i * Mathf.PI * 2 / numDirections;
      directions.Add(new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)));
    }

    return directions;
  }

}

