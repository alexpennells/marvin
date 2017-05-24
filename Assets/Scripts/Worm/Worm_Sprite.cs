using UnityEngine;
using System;
using System.Timers;

public class Worm_Sprite : SpriteObj {
  public override void Step() {
    if (IsPlaying("body") || Base.Is("Hurt"))
      return;

    FacingLeft = Base.Physics.hspeed < 0;
    FaceAngle((Base as Worm_Base).RotationAngle);

    if (spriteRenderer.color.g < 0.94f)
      spriteRenderer.color = new Color(1f, spriteRenderer.color.g + 0.05f, spriteRenderer.color.b + 0.05f, 1f);
    else
      spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
  }

  public void PlayRed() {
    spriteRenderer.color = new Color(1f, 0f, 0f, 1f);
  }

  /***********************************
   * ANIMATION DEFINITIONS
   **********************************/

  public void PlayHead() {
    Animate("head", 1f);
  }

  public void PlayBody() {
    Animate("body", 1f);
  }
}
