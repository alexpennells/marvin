using System;

namespace Minion {
  public class Sound : SoundObj {
    public override Type Lib { get { return typeof(Minion.SFX); } }

    public void PlayStep() {
      Game.SFX.Play("MinionStep" + Variation(3), 0.1f);
    }
  }
}
