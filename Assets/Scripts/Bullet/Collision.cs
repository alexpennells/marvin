using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Bullet {
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
        case eObjectType.PLAYER_GHOST:
          PlayerGhostCollision(other as PlayerGhost.Base);
          break;
      }
    }

    /***********************************
     * HANDLERS
     **********************************/

    private void MinionCollision(Minion.Base other) {
      if (other.Is("Dead"))
        return;

      Game.CreateParticle("Impact", Base.Position);
      other.State("Die");
      Base.DestroySelf();
    }

    private void SkullCollision(Skull.Base other) {
      if (other.Is("Invincible"))
        return;

      Game.CreateParticle("Impact", Base.Position);
      other.State("Die");
      Base.DestroySelf();
    }

    private void RatCollision(Rat.Base other) {
      if (other.Is("Dead"))
        return;

      Game.CreateParticle("Impact", Base.Position);
      other.State("Die");
      Base.DestroySelf();
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

    private void PlayerGhostCollision(PlayerGhost.Base other) {
      if ((Base as Bullet.Base).ammoType == eAmmoType.HEAD) {
        Base.Physics.hspeed = 0;
        Base.Physics.vspeed = 0;
        Base.StopCoroutine("Boomerang");
        Base.StopCoroutine("ReturnToTig");

        other.transform.Find("Light").parent = Base.transform;
        other.Head = Base as Bullet.Base;
        other.DestroySelf();
      }
    }
  }
}
