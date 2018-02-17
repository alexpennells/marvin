using UnityEngine;
using System;

namespace Player {
  public class Input : InputBlock {
    public Input() { enabled = true; }

    protected override void LeftHeld (float val) {
      if (Base.Is("Throwing"))
        Base.Sprite.FacingLeft = true;

      if (Base.Is("Dead") || Base.Is("Ziplining") || Base.Is("Throwing") || Base.Is("Ducking") || Base.Is("Pounding"))
        return;

      if (Base.Physics.Ground.WallJump.IsJumping())
        return;

      // Don't increase speed if walk speed is maxed out.
      if (!Base.Is("Running") && Base.Physics.hspeed <= -(Base as Player.Base).maxWalkingHspeed)
        return;

      Base.Physics.hspeed -= (Base as Player.Base).TrueAccelerationSpeed;
    }

    protected override void RightHeld (float val) {
      if (Base.Is("Throwing"))
        Base.Sprite.FacingRight = true;

      if (Base.Is("Dead") || Base.Is("Ziplining") || Base.Is("Throwing") || Base.Is("Ducking") || Base.Is("Pounding"))
        return;

      if (Base.Physics.Ground.WallJump.IsJumping())
        return;

      // Don't increase speed if walk speed is maxed out.
      if (!Base.Is("Running") && Base.Physics.hspeed >= (Base as Player.Base).maxWalkingHspeed)
        return;

      Base.Physics.hspeed += (Base as Player.Base).TrueAccelerationSpeed;
    }

    protected override void DownPressed () {
      if (Base.Is("Throwing"))
        Base.State("CancelThrow");
      else if (Base.Is("Ziplining"))
        Base.Physics.Rail.StopGrinding();
      else if (!Base.HasFooting)
        Base.State("Pound");
    }

    protected override void UnoPressed () {
      Base.RestartCoroutine("JumpPress");

      if (Base.Is("Dead") || Base.Is("Pounding"))
        return;

      if (Base.Is("Ziplining")) {
        Base.Physics.Rail.StopGrinding();
        Base.Physics.vspeed = (Base as Player.Base).jumpSpeed;
      } else if (Base.Is("WallSliding")) {
        Base.Sound.Play("Jump");
        Base.Physics.Ground.WallJump.ActuallyWallJump();
      } else if (Base.HasFooting) {
        if (Base.Is("Throwing"))
          (Base as Player.Base).EndThrow();

        Base.Physics.vspeed = (Base as Player.Base).jumpSpeed;
        Base.Sound.Play("Jump");
        Base.Physics.Ground.Collider.ClearFooting();
      }
    }

    protected override void UnoReleased () {
      if (Base.Is("Dead"))
        return;

      if (!Base.Physics.Ground.WallJump.IsJumping() && Base.Physics.vspeed > 2)
        Base.Physics.vspeed = 2;
    }

    protected override void DosPressed () {
      Base.State("Throw");
    }

    protected override void TresPressed () {
      Base.State("Hurt", true);
    }

    protected override void DosReleased () {
      if (Base.Is("Throwing"))
        Base.Sprite.SetSpeed(1.5f);
    }
  }
}
