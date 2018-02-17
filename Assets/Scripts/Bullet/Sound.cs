using System;

namespace Bullet {
  public class Sound : SoundBlock {
    public Sound() { enabled = true; }
    public override Type Lib { get { return typeof(AmmoBall.SFX); } }

    public void PlayBounce() {
      Game.SFX.Play("BallBounce" + Variation(4), 0.2f);
    }
  }
}
