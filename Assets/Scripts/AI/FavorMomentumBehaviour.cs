using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FavorMomentumBehaviour : SteeringBehaviour
{
  [SerializeField]
  private float scaleReduction;
  private Rigidbody2D _rigidbody;
  [SerializeField]
  private bool showGizmo = true;

  float[] dangersResultTemp = null;

  void Awake()
  {
    _rigidbody = GetComponent<Rigidbody2D>();
  }

  public override (float[] danger, float[] interest) GetSteering(float[] danger, float[] interest, AIData aiData)
  {
    for (int i = 0; i < Directions.All.Count; i++)
    {
      var amount = Vector2.Dot(_rigidbody.velocity.normalized, Directions.All[i] * scaleReduction);

      interest[i] = Mathf.Clamp01(interest[i] + amount);
    }
    return (danger, interest);
  }
}
