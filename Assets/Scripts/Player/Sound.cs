using System;

namespace Player {
  public class Sound : SoundBlock {
    public Sound() { enabled = true; }
    public override Type Lib { get { return typeof(Player.SFX); } }

    /***********************************
     * SINGLES
     **********************************/

    public void PlayJump() {
      Game.SFX.Play("CatJump" + Variation(5), 0.8f);
      Game.SFX.Play("Footstep" + Variation(6), 0.2f);
    }

    public void PlayFootstep() {
      (Base as Player.Base).CreateWalkPuffs(2);
      Game.SFX.Play("Footstep" + Variation(6), 0.1f);
    }

    public void PlayLand() {
      Game.SFX.Play("Footstep" + Variation(6), 0.5f);
    }

    public void PlayThump() {
      Game.SFX.Play("Thump", 0.25f);
    }

    /***********************************
     * LOOPS
     **********************************/

    public void StartSlide() {
      Loops["Slide"].volume = 0.15f;
    }

    public void StopSlide() {
      Loops["Slide"].volume = 0f;
    }
  }
}
