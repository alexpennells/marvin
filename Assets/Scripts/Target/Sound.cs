using System;

namespace Target {
  public class Sound : SoundObj {
    public override Type Lib { get { return typeof(Target_SFXLib); } }

    public void PlayBreak() {
      Game.SFX.Play("TargetBreak", 1f);
    }
  }
}
