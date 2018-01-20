using System;

namespace Soul {
  public class Sound : SoundBlock {
    public Sound() { enabled = true; }
    public override Type Lib { get { return typeof(Soul.SFX); } }

    public void PlayCollect() {
      Game.SFX.Play("CollectSoul" + Variation(3), 0.5f);
    }
  }
}
