using System;

namespace PlayerGhost {
  public class Sound : SoundBlock {
    public Sound() { enabled = true; }
    public override Type Lib { get { return typeof(Player.SFX); } }
  }
}
