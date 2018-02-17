using UnityEngine;
using System;
using System.Collections.Generic;

namespace Bullet {
  public class SolidCollider : SolidColliderBlock {
    public SolidCollider() { enabled = true; }

    protected override void RoofCollision(SolidObj roof) {
      if ((Base as Bullet.Base).ammoType == eAmmoType.BALL) {
        BaseObj ball = Game.Create("Ammo/Ball", Base.Position);
        ball.Physics.hspeed = Base.Physics.hspeed / 3;
        ball.Physics.vspeed = Math.Abs(Base.Physics.vspeed) / -2;
        Base.Sound.Play("Bounce");
      }

      Base.DestroySelf();
    }

    protected override void WallCollision(SolidObj wall) {
      if ((Base as Bullet.Base).ammoType == eAmmoType.BALL) {
        BaseObj ball = Game.Create("Ammo/Ball", Base.Position);
        ball.Physics.hspeed = -Base.Physics.hspeed / 2;
        ball.Physics.vspeed = 3f;
        Base.Sound.Play("Bounce");
      }

      Base.DestroySelf();
    }

    protected override void FootingCollision(SolidObj footing) {
      if ((Base as Bullet.Base).ammoType == eAmmoType.BALL) {
        BaseObj ball = Game.Create("Ammo/Ball", Base.Position);
        ball.Physics.hspeed = Base.Physics.hspeed / 3;
        ball.Physics.vspeed = Math.Max(Math.Abs(Base.Physics.vspeed) / 2, 2);
        Base.Sound.Play("Bounce");
      }

      Base.DestroySelf();
    }
  }
}
