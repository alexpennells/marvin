using UnityEngine;

public class WormBody_Base : BaseObj {
  protected override void Init() {
    Sprite.Play("Body");
  }

  public void UpdateState(WormState state) {
    Sprite.FacingLeft = state.facingLeft;
    Sprite.FaceAngle(state.rotation);
    Position = state.position;
  }
}
