using System;

namespace PlayerGhost {
  public class Sound : SoundObj {
    public override Type Lib { get { return typeof(Player.SFX); } }
  }
}
