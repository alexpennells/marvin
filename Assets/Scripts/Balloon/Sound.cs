using System;

namespace Balloon {
  public class Sound : SoundObj {
    public override Type Lib { get { return typeof(Balloon.SFX); } }

    public void PlayPop() {
      Game.SFX.Play("BalloonPop", 0.5f);
    }
  }
}
