using System;

namespace Balloon {
  public class Sound : SoundBlock {
    public Sound() { enabled = true; }
    public override Type Lib { get { return typeof(Balloon.SFX); } }

    public void PlayPop() {
      Game.SFX.Play("BalloonPop", 0.5f);
      Game.SFX.Play("SoulsEscape", 1f);
    }
  }
}
