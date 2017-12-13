using System;

namespace Soul {
  public class Sound : SoundObj {
    public override Type Lib { get { return typeof(Soul.SFX); } }

    public void PlayCollect() {
      Game.SFX.Play("CollectSoul" + Variation(3), 0.2f);
    }
  }
}
