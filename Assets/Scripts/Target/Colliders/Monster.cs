using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Target {
  namespace Colliders {

    public class Monster : CollisionHandler {
      public override void Step() {
        if (Base.Sprite.IsPlaying("idle")) {
          Collider.size = new Vector2(0, 0);
        } else if (Base.Sprite.IsPlaying("inhale")) {
          Collider.size = new Vector2(0.6661708f, 0.3369771f);
          Collider.offset = new Vector2(-0.1839186f, 0.003845274f);
        } else if (Base.Sprite.IsPlaying("stretch")) {
          StretchResize();
        } else if (Base.Sprite.IsPlaying("shrink")) {
          ShrinkResize();
        }
      }

      protected override void HandleCollision(eObjectType otherType, BaseObj other) {
        switch (otherType) {
          case eObjectType.WILLY_FIST:
            WillyFistCollision(other as WillyFist.Base);
            break;
        }
      }

      /***********************************
       * HANDLERS
       **********************************/

      private void WillyFistCollision(WillyFist.Base other) {
        other.State("Impact");

        if (Base.Is("Inhaling") || Base.Is("Stretching")) {
          Base.State("Hurt");
        } else {
          Base.State("Break");
        }
      }

      /***********************************
       * COLLIDER RESIZING
       **********************************/

      private void StretchResize() {
        Vector2 newSize = Vector2.zero;
        Vector2 newOffset = Vector2.zero;
        float curTime = Base.Sprite.GetAnimationTime();

        if (curTime >= 0.2f && curTime < 0.4f) {
          newOffset = new Vector2(0.04998338f, -0.01335365f);
          newSize = new Vector2(0.1983669f, -0.2200264f);
        } else if (curTime >= 0.4f && curTime < 0.6f) {
          newOffset = new Vector2(0.02593565f, -0.006482661f);
          newSize = new Vector2(0.2464623f, -0.2749923f);
        } else if (curTime >= 0.6f && curTime < 0.8f) {
          newOffset = new Vector2(-0.05307829f, -0.003047407f);
          newSize = new Vector2(0.4044902f, 0.3368298f);
        } else if (curTime >= 0.8f && curTime < 1f) {
          newOffset = new Vector2(-0.1458338f, -0.01335329f);
          newSize = new Vector2(0.5900013f, 0.3986665f);
        }

        Collider.size = newSize;
        Collider.offset = newOffset;
      }

      private void ShrinkResize() {
        Vector2 newSize = Vector2.zero;
        Vector2 newOffset = Vector2.zero;
        float curTime = Base.Sprite.GetAnimationTime();

        if (curTime >= 0.8f && curTime < 1f) {
          newOffset = new Vector2(0.04998338f, -0.01335365f);
          newSize = new Vector2(0.1983669f, -0.2200264f);
        } else if (curTime >= 0.6f && curTime < 0.8f) {
          newOffset = new Vector2(0.02593565f, -0.006482661f);
          newSize = new Vector2(0.2464623f, -0.2749923f);
        } else if (curTime >= 0.4f && curTime < 0.6f) {
          newOffset = new Vector2(-0.05307829f, -0.003047407f);
          newSize = new Vector2(0.4044902f, 0.3368298f);
        } else if (curTime >= 0.2f && curTime < 0.4f) {
          newOffset = new Vector2(-0.1458338f, -0.01335329f);
          newSize = new Vector2(0.5900013f, 0.3986665f);
        }

        Collider.size = newSize;
        Collider.offset = newOffset;
      }
    }

  }
}
