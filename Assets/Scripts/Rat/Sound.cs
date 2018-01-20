using System;

namespace Rat {
  public class Sound : SoundBlock {
    public Sound() { enabled = true; }
    public override Type Lib { get { return typeof(Rat.SFX); } }

    public void PlayFootstep() {
      Game.SFX.Play("Footstep" + Variation(6), 0.1f);
    }

    public void PlaySqueak() {
      if (Game.Camera.InView(Base) && !Base.Is("Dead"))
        Game.SFX.Play("RatSqueak", 0.1f);
    }

    public void PlayDie() {
      Game.SFX.Play("RatDie", 0.75f);
    }
  }
}
