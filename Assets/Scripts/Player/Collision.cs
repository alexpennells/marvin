using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Player {
  public class Collision : CollisionHandler {
    protected override void HandleCollision(eObjectType otherType, BaseObj other) {
      switch (otherType) {
        case eObjectType.RAT:
          RatCollision(other as Rat.Base);
          break;
        case eObjectType.MINION:
          MinionCollision(other as Minion.Base);
          break;
        case eObjectType.SKULL:
          SkullCollision(other as Skull.Base);
          break;
        case eObjectType.SOUL:
          SoulCollision(other as Soul.Base);
          break;
        case eObjectType.COIN:
          CoinCollision(other as Coin.Base);
          break;
        case eObjectType.BALLOON:
          BalloonCollision(other as Balloon.Base);
          break;
        case eObjectType.TARGET_MONSTER:
          TargetMonsterCollision(other as Target.Base);
          break;
        case eObjectType.DOOR:
          DoorCollision(other as Door.Base);
          break;
      }
    }

    /***********************************
     * HANDLERS
     **********************************/

    private void MinionCollision(Minion.Base other) {
      if (other.Is("Dead"))
        return;

      if (!Base.HasFooting && Base.y > other.y) {
        Base.State("Bounce");
        Game.CreateParticle("Impact", Base.Position);
        other.State("Die");
      }
      else if (!Base.Is("Dead") && !Base.Is("Invincible")) {
        other.Sound.Play("Giggle");
        Base.State("Hurt", other.Mask.Center.x > Base.x);
      }
    }

    private void SkullCollision(Skull.Base other) {
      if (other.Is("Invincible"))
        return;

      if (!Base.HasFooting && Base.y > other.y) {
        Base.State("Bounce");
        other.State("Die");
      }
      else if (!Base.Is("Dead") && !Base.Is("Invincible")) {
        other.Sound.Play("Giggle");
        Base.State("Hurt", other.Mask.Center.x > Base.x);
      }
    }

    private void RatCollision(Rat.Base other) {
      if (other.Is("Dead"))
        return;

      if (!Base.HasFooting && Base.y > other.y) {
        Base.State("Bounce");
        Game.CreateParticle("Impact", Base.Position);
        other.State("Die");
      }
      else
        Base.State("Hurt", other.Mask.Center.x > Base.x);
    }

    private void SoulCollision(Soul.Base other) {
      if (other.Is("Collectable"))
        other.DestroySelf();
    }

    private void CoinCollision(Coin.Base other) {
      other.DestroySelf();
    }

    private void BalloonCollision(Balloon.Base other) {
      other.DestroySelf();
    }

    private void TargetMonsterCollision(Target.Base other) {
      Base.State("Hurt", true);
    }

    private void DoorCollision(Door.Base other) {
      if (!Game.disableInput && Game.UpHeld && Base.HasFooting)
        Base.State("Exit", other);
    }
  }
}
