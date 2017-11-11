using System;

public class Player_Sound : SoundObj {
  public override Type Lib { get { return typeof(Player_SFXLib); } }

  /***********************************
   * SINGLES
   **********************************/

  public void PlayJump() {
    Game.SFX.Play("CatJump" + Variation(5), 0.8f);
    Game.SFX.Play("Footstep" + Variation(6), 0.2f);
  }

  public void PlayFootstep() {
    Game.SFX.Play("Footstep" + Variation(6), 0.1f);
  }

  public void PlayLoudFootstep() {
    Game.SFX.Play("Footstep" + Variation(6), 0.2f);
  }

  public void PlayLand() {
    Game.SFX.Play("Footstep" + Variation(6), 0.5f);
  }

  /***********************************
   * LOOPS
   **********************************/

  public void StartSlide() {
    Loops["Slide"].volume = 0.2f;
  }

  public void StopSlide() {
    Loops["Slide"].volume = 0f;
  }
}
