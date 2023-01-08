public class Node
{
  public int XIndex { get; set; }
  public int YIndex { get; set; }
  public float X { get; set; }
  public float Y { get; set; }
  public bool IsOpen { get; set; }

  public Node(int xIndex, int yIndex, float x, float y, bool isOpen)
  {
    XIndex = xIndex;
    YIndex = yIndex;
    X = x;
    Y = y;
    IsOpen = isOpen;
  }
}