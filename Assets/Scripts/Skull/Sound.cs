using System;

namespace Skull {
  public class Sound : SoundBlock {
    public Sound() { enabled = true; }
    public override Type Lib { get { return typeof(Minion.SFX); } }

    public void PlayLand() {
      if (Game.Camera.InView(Base))
        Game.SFX.Play("MinionStep" + Variation(3), 0.5f);
    }

    public void PlayDie() {
      Game.SFX.Play("MinionDie", 0.1f);
    }

    public void PlayGiggle() {
      Game.SFX.Play("MinionGiggle" + Variation(2), 0.5f);
    }
  }
}
