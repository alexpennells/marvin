using System;

namespace Skull {
  public class Sound : SoundObj {
    public override Type Lib { get { return typeof(Minion.SFX); } }

    public void PlayLand() {
      if (Game.Camera.InView(Base))
        Game.SFX.Play("MinionStep" + Variation(3), 0.5f);
    }
  }
}
