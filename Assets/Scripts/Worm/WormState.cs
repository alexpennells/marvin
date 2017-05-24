using UnityEngine;

public class WormState {
  public Vector3 position;
  public bool facingLeft;
  public float rotation;

  public WormState(Vector3 pos, bool faceLeft, float rot) {
    position = pos;
    facingLeft = faceLeft;
    rotation = rot;
  }
}
