using System;

namespace Player {
  public class Sound : SoundBlock {
    public Sound() { enabled = true; }
    public override Type Lib { get { return typeof(Player.SFX); } }

    public override void Step() {
      if (Base.Is("Ducking"))
        StartLoop("Purr");
      else
        StopLoop("Purr");

      if (Base.Is("Throwing") && Base.Sprite.speed == 0)
        StartLoop("Aim");
      else
        StopLoop("Aim");

      base.Step();
    }

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

    public void PlayPound() {
      Game.SFX.Play("Pound", 0.1f);
    }

    public void PlayGroundPound() {
      Game.SFX.Play("Thump", 0.5f);
    }

    /***********************************
     * LOOPS
     **********************************/

    public void StartPurr() {
      Loops["Purr"].volume = Math.Min(Loops["Purr"].volume + 0.001f, 0.2f);
    }

    public void StopPurr() {
      Loops["Purr"].volume = Math.Max(Loops["Purr"].volume - 0.01f, 0f);
    }

    public void StartAim() {
      Loops["Aim"].volume = Math.Min(Loops["Aim"].volume + 0.05f, 0.8f);
    }

    public void StopAim() {
      Loops["Aim"].volume = Math.Max(Loops["Aim"].volume - 0.05f, 0f);
    }

    public void StartSlide() {
      Loops["Slide"].volume = 0.15f;
    }

    public void StopSlide() {
      Loops["Slide"].volume = 0f;
    }
  }
}
