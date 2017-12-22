using UnityEngine;
using System;

namespace Willy {
  public class Base : InputObj {
    private BaseObj follow = null;
    private Player.Base player;

    private bool grow = false;
    public float maxDist = 0.1f;

    protected override void Init() {
      this.player = GameObject.Find("Player").GetComponent<Player.Base>();
      this.follow = this.player;
      JumpToDesiredPosition();
      Sprite.Play("Idle");
    }

    protected override void Step () {
      base.Step();

      if (!Is("Punching")) {
        Sprite.FacingLeft = player.Sprite.FacingLeft;
        MoveToDesiredPosition();
      }

      AdjustScale();
    }

    protected override void DosPressed () {
      State("Punch");
    }

    public void CreateFist() {
      WillyFist.Base fist = Game.Create("WillyFist", Position) as WillyFist.Base;
      fist.Sprite.FacingLeft = Sprite.FacingLeft;
      fist.Sprite.Renderer.sortingLayerID = Sprite.Renderer.sortingLayerID;
      fist.Sprite.SetLayer(Sprite.GetLayer() + 1);

      grow = false;
      if (Sprite.FacingLeft) {
        Physics.hspeed = Physics.hspeedMax;
        fist.Physics.hspeed = -5f;
      } else {
        Physics.hspeed = -Physics.hspeedMax;
        fist.Physics.hspeed = 5f;
      }
    }

    // Instantly jump to the desired position.
    public void JumpToDesiredPosition() {
      Position = DesiredPosition();
    }

    private void AdjustScale() {
      if (grow) {
        float newSize = Math.Min(Math.Abs(transform.localScale.x) + 0.05f, 1.25f);
        transform.localScale = new Vector3(Sprite.FacingLeft ? -newSize : newSize, newSize, 1f);
      } else {
        float newSize = Math.Max(Math.Abs(transform.localScale.x) - 0.01f, 0.75f);
        transform.localScale = new Vector3(Sprite.FacingLeft ? -newSize : newSize, newSize, 1f);
      }
    }

    // Gradually move to the desired position.
    private void MoveToDesiredPosition() {
      Vector3 des = DesiredPosition();

      if (x < des.x)
        Physics.hspeed += 0.015f;
      else
        Physics.hspeed -= 0.015f;

      if (y < des.y)
        Physics.vspeed += 0.01f;
      else
        Physics.vspeed -= 0.01f;

      // Snap X if out of bounds
      if (x < MinimumDesiredX())
        x += (MinimumDesiredX() - x) / 24;
      else if (x > MaximumDesiredX())
        x -= (x - MaximumDesiredX()) / 24;

      // Snap Y if out of bounds
      if (y < MinimumDesiredY())
        y += (MinimumDesiredY() - y) / 16;
      else if (y > MaximumDesiredY())
        y -= (y - MaximumDesiredY()) / 16;
    }

    private Vector3 DesiredPosition () {
      if (this.follow == null)
        return Position;

      float yOffset = 0.25f;
      float xOffset = 0.25f;
      if (this.follow.Sprite.FacingLeft)
        xOffset *= -1;

      return new Vector3(this.follow.Position.x + xOffset, this.follow.Position.y + yOffset, this.follow.Position.z);
    }

    private float MinimumDesiredX() {
      return DesiredPosition().x - this.maxDist;
    }

    private float MaximumDesiredX() {
      return DesiredPosition().x + this.maxDist;
    }

    private float MinimumDesiredY() {
      return DesiredPosition().y - this.maxDist;
    }

    private float MaximumDesiredY() {
      return DesiredPosition().y + this.maxDist;
    }

    /***********************************
     * STATE CHANGE FUNCTIONS
     **********************************/

    public void StatePunch() {
      if (Is("Punching"))
        return;

      Sprite.Play("Punch");
      grow = true;

      if (Sprite.FacingLeft)
        Physics.hspeed = -Physics.hspeedMax;
      else
        Physics.hspeed = Physics.hspeedMax;
    }

    /***********************************
     * STATE CHECKERS
     **********************************/

    public bool IsPunching() { return Sprite.IsPlaying("punch"); }

    /***********************************
     * EDITOR NONSENSE
     **********************************/

    protected override bool DrawGizmos() {
      // Desired Position
      Debug.DrawLine(new Vector3(DesiredPosition().x - Game.UNIT / 16, DesiredPosition().y, z), new Vector3(DesiredPosition().x + Game.UNIT / 16, DesiredPosition().y, z), Color.magenta, 0, false);
      Debug.DrawLine(new Vector3(DesiredPosition().x, DesiredPosition().y - Game.UNIT / 16, z), new Vector3(DesiredPosition().x, DesiredPosition().y + Game.UNIT / 16, z), Color.magenta, 0, false);

      Debug.DrawLine(new Vector3(MinimumDesiredX(), MaximumDesiredY(), z), new Vector3(MaximumDesiredX(), MaximumDesiredY(), z), Color.magenta, 0, false);
      Debug.DrawLine(new Vector3(MinimumDesiredX(), MinimumDesiredY(), z), new Vector3(MaximumDesiredX(), MinimumDesiredY(), z), Color.magenta, 0, false);
      Debug.DrawLine(new Vector3(MinimumDesiredX(), MinimumDesiredY(), z), new Vector3(MinimumDesiredX(), MaximumDesiredY(), z), Color.magenta, 0, false);
      Debug.DrawLine(new Vector3(MaximumDesiredX(), MinimumDesiredY(), z), new Vector3(MaximumDesiredX(), MaximumDesiredY(), z), Color.magenta, 0, false);

      return false;
    }
  }
}
