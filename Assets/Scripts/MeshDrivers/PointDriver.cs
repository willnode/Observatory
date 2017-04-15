using UnityEngine;
using UnityEngine.UI;

// Driver for Texts at Xplainer
public class PointDriver : MonoBehaviour
{
  public Text head;
  public Text body;
  public Toggle toggle;
  
  public void SetUp (string h, string b)
  {
    head.text = h;
    body.text = b;
  }
}