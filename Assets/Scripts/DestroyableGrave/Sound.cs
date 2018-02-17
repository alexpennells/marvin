using System;

namespace DestroyableGrave {
  public class Sound : SoundBlock {
    public Sound() { enabled = true; }
    public override Type Lib { get { return typeof(DestroyableGrave.SFX); } }

    public void PlayCrack() {
      Game.SFX.Play("GraveCrack", 0.5f);
    }
  }
}
