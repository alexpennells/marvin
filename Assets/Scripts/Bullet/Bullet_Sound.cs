using System;

public class Bullet_Sound : SoundObj {
  public override Type Lib { get { return typeof(Bullet_SFXLib); } }

  public void PlayPop() {
    Game.SFX.Play("BulletPop", 0.5f);
  }

  public void PlayShoot() {
    Game.SFX.Play("BulletShoot", 0.6f);
  }
}
