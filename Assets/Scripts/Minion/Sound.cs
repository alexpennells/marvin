using System;

namespace Minion {
  public class Sound : SoundObj {
    public override Type Lib { get { return typeof(Minion.SFX); } }

    public void PlayStep() {
      if (Game.Camera.InView(Base))
        Game.SFX.Play("MinionStep" + Variation(3), 0.2f);
    }

    public void PlayLand() {
      if (Game.Camera.InView(Base))
        Game.SFX.Play("MinionStep" + Variation(3), 0.5f);
    }

    public void PlayGiggle() {
      Game.SFX.Play("MinionGiggle" + Variation(2), 0.5f);
    }

    public void PlayLaugh() {
      Game.SFX.Play("MinionLaugh", 0.1f);
    }
  }
}
