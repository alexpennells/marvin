using System;

namespace Player {
  public class Sound : SoundObj {
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
      (Base as Player.Base).CreateWalkPuffs(8);
      Game.SFX.Play("Footstep" + Variation(6), 0.5f);
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
