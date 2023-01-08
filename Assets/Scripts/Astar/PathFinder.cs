using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PathFinder : MonoBehaviour
{
  [SerializeField]
  private float clearRadius;
  private List<Node> map = new List<Node>();
  [SerializeField]
  private int numWidthSegments;
  [SerializeField]
  private int numHeightSegments;
  [SerializeField]
  private Vector2 lowerLeftStart;
  [SerializeField]
  private Vector2 upperRightEnd;

  public static PathFinder Instance { get; private set; }

  // Scan through the area and ray cast at each point for collision with an obstacle
  void Awake()
  {
    Instance = this;

    var width = upperRightEnd.x - lowerLeftStart.x;
    var height = upperRightEnd.y - lowerLeftStart.y;
    var widthStep = width / numWidthSegments;
    var heightStep = height / numHeightSegments;
    for (int i = 0; i < numWidthSegments; i++)
    {
      for (int j = 0; j < numHeightSegments; j++)
      {
        var x = lowerLeftStart.x + i * widthStep;
        var y = lowerLeftStart.y + j * heightStep;
        var position = new Vector2(x, y);
        var collider = Physics2D.OverlapCircle(position, clearRadius, LayerMask.GetMask("Obstacle"));
        var isObstacle = collider != null;

        if (isObstacle)
        {
          Debug.DrawRay(position, Vector2.up * 0.1f, Color.red, 1000f);
        }
        else
        {
          Debug.DrawRay(position, Vector2.up * 0.1f, Color.green, 1000f);
        }

        var node = new Node(i, j, x, y, !isObstacle);
        map.Add(node);
      }
    }
  }

  public List<Node> GetPath(Vector2 start, Vector2 end)
  {
    var startNode = GetClosestNode(start);
    var endNode = GetClosestNode(end);
    var startTile = new Tile();
    startTile.X = startNode.XIndex;
    startTile.Y = startNode.YIndex;

    var finishTile = new Tile();
    finishTile.X = endNode.XIndex;
    finishTile.Y = endNode.YIndex;

    startTile.SetDistance(finishTile.X, finishTile.Y);

    var activeTiles = new List<Tile>();
    activeTiles.Add(startTile);
    var visitedTiles = new List<Tile>();

    while (activeTiles.Any())
    {
      var checkTile = activeTiles.OrderBy(x => x.CostDistance).First();

      Debug.Log($"Checking {checkTile.X}, {checkTile.Y}");

      if (checkTile.X == finishTile.X && checkTile.Y == finishTile.Y)
      {
        var path = new List<Node>();
        path.Add(GetNode(checkTile.X, checkTile.Y));
        //We found the destination and we can be sure (Because the the OrderBy above)
        //That it's the most low cost option. 
        var tile = checkTile;
        Debug.Log("Retracing steps backwards...");
        while (true)
        {
          Debug.Log($"{tile.X} : {tile.Y}");
          tile = tile.Parent;
          if (tile == null)
          {
            Debug.Log("Done!");
            DrawPath(path);
            path.Reverse();
            return path;
          }
          else
          {
            path.Add(GetNode(tile.X, tile.Y));
          }
        }
      }

      visitedTiles.Add(checkTile);
      activeTiles.Remove(checkTile);

      var walkableTiles = GetWalkableTiles(checkTile, finishTile);

      Debug.Log($"Found {walkableTiles.Count} walkable tiles");

      foreach (var walkableTile in walkableTiles)
      {
        //We have already visited this tile so we don't need to do so again!
        if (visitedTiles.Any(x => x.X == walkableTile.X && x.Y == walkableTile.Y))
          continue;

        //It's already in the active list, but that's OK, maybe this new tile has a better value (e.g. We might zigzag earlier but this is now straighter). 
        if (activeTiles.Any(x => x.X == walkableTile.X && x.Y == walkableTile.Y))
        {
          var existingTile = activeTiles.First(x => x.X == walkableTile.X && x.Y == walkableTile.Y);
          if (existingTile.CostDistance > checkTile.CostDistance)
          {
            activeTiles.Remove(existingTile);
            activeTiles.Add(walkableTile);
          }
        }
        else
        {
          //We've never seen this tile before so add it to the list. 
          activeTiles.Add(walkableTile);
        }
      }
    }

    return null;
  }

  private Node GetNode(int x, int y)
  {
    return map.First(node => node.XIndex == x && node.YIndex == y);
  }

  private List<Tile> GetWalkableTiles(Tile currentTile, Tile targetTile)
  {
    var possibleTiles = new List<Tile>()
  {
    new Tile { X = currentTile.X, Y = currentTile.Y - 1, Parent = currentTile, Cost = currentTile.Cost + 1 },
    new Tile { X = currentTile.X, Y = currentTile.Y + 1, Parent = currentTile, Cost = currentTile.Cost + 1},
    new Tile { X = currentTile.X - 1, Y = currentTile.Y, Parent = currentTile, Cost = currentTile.Cost + 1 },
    new Tile { X = currentTile.X + 1, Y = currentTile.Y, Parent = currentTile, Cost = currentTile.Cost + 1 },

    new Tile { X = currentTile.X + 1, Y = currentTile.Y + 1, Parent = currentTile, Cost = currentTile.Cost + 0.8f },
    new Tile { X = currentTile.X + 1, Y = currentTile.Y - 1, Parent = currentTile, Cost = currentTile.Cost + 0.8f},
    new Tile { X = currentTile.X - 1, Y = currentTile.Y + 1, Parent = currentTile, Cost = currentTile.Cost + 0.8f },
    new Tile { X = currentTile.X - 1, Y = currentTile.Y - 1, Parent = currentTile, Cost = currentTile.Cost + 0.8f },
  };

    possibleTiles.ForEach(tile => tile.SetDistance(targetTile.X, targetTile.Y));

    // Filter out tiles that are in bounds
    possibleTiles = possibleTiles
      .Where(tile =>
      {
        var node = GetNode(tile.X, tile.Y);
        return node != null && node.IsOpen;
      })
      .ToList();

    return possibleTiles;
  }

  private Node GetClosestNode(Vector2 position)
  {
    var width = upperRightEnd.x - lowerLeftStart.x;
    var height = upperRightEnd.y - lowerLeftStart.y;
    var widthStep = width / numWidthSegments;
    var heightStep = height / numHeightSegments;
    var x = (int)((position.x - lowerLeftStart.x) / widthStep);
    var y = (int)((position.y - lowerLeftStart.y) / heightStep);

    return map.Find(n => n.XIndex == x && n.YIndex == y);
  }

  public void DrawPath(List<Node> path)
  {
    for (var i = 1; i < path.Count; i++)
    {
      var start = path[i - 1];
      var end = path[i];
      Debug.DrawLine(new Vector2(start.X, start.Y), new Vector2(end.X, end.Y), Color.red);
      Debug.Log($"{start.X}, {start.Y} -> {end.X}, {end.Y}");
    }
  }

  private void OnDrawGizmos()
  {
    // draw four lines to make a box
    Gizmos.DrawLine(lowerLeftStart, lowerLeftStart + new Vector2(0, upperRightEnd.y - lowerLeftStart.y));
    Gizmos.DrawLine(lowerLeftStart, lowerLeftStart + new Vector2(upperRightEnd.x - lowerLeftStart.x, 0));
    Gizmos.DrawLine(upperRightEnd, upperRightEnd - new Vector2(0, upperRightEnd.y - lowerLeftStart.y));
    Gizmos.DrawLine(upperRightEnd, upperRightEnd - new Vector2(upperRightEnd.x - lowerLeftStart.x, 0));
  }
}