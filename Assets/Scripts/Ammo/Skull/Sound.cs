using System;

namespace AmmoSkull {
  public class Sound : SoundBlock {
    public Sound() { enabled = true; }
    public override Type Lib { get { return typeof(Minion.SFX); } }

    public void PlayLand() {
      if (Game.Camera.InView(Base))
        Game.SFX.Play("MinionStep" + Variation(3), 0.5f);
    }
  }
}
