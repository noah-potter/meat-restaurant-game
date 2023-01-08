using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TargetDetector : Detector
{
  [SerializeField]
  private bool showGizmos = false;

  //gizmo parameters
  private List<Transform> colliders;

  public void Awake()
  {
    colliders = GameObject.FindObjectsOfType<Seat>().Select(s => s.transform).ToList();
  }

  public override void Detect(AIData aiData)
  {
    if (aiData.currentTarget == null)
      return;

    var path = PathFinder.Instance.GetPath(transform.position, aiData.currentTarget.position);

    if (path.Count > 2)
    {
      var target = path[1];
      aiData.currentTargetPosition = new Vector2(target.X, target.Y);
    }
  }

  private void OnDrawGizmosSelected()
  {
    if (showGizmos == false)
      return;

    if (colliders == null)
      return;
    Gizmos.color = Color.magenta;
    foreach (var item in colliders)
    {
      Gizmos.DrawSphere(item.position, 0.3f);
    }
  }
}