using UnityEngine;
using System;
using System.Collections;

namespace Bullet {
  public class Base : BaseObj {
    [Tooltip("What type of ammo is this")]
    public eAmmoType ammoType = eAmmoType.SKULL;

    private GameObject crosshair;
    private bool boomerang = false;
    private bool doneReturnToTig = false;
    private bool ghostAbsorbed = false;
    private Player.Base player;

    public override void LoadReferences() {
      Sprite = new Sprite();
      Sound = new Sound();
      Physics = new Physics(Physics);
      base.LoadReferences();
    }

    public override void Init() {
      crosshair = GameObject.Find("Crosshair");
      player = GameObject.Find("Player").GetComponent<Player.Base>() as Player.Base;
      Physics.MoveTo(crosshair.transform.position, 10f);
      // Physics.hspeedMax = 20f;
      // Physics.hspeed = 20f;

      if (ammoType == eAmmoType.BALL) {
        StartCoroutine("DelayGravity");
      } else if (ammoType == eAmmoType.HEAD) {
        Physics.Ground.enabled = false;
        StartCoroutine("Boomerang");
      }

      base.Init();
    }

    public override void DestroySelf() {
      switch (ammoType) {
        case eAmmoType.HEAD:
          if (doneReturnToTig) { base.DestroySelf(); }
          break;
        case eAmmoType.BALL:
          base.DestroySelf();
          break;
        case eAmmoType.SKULL:
          base.DestroySelf();
          break;
      }
    }

    /***********************************
     * CO-ROUTINES
     **********************************/

    private IEnumerator Boomerang() {
      boomerang = true;
      yield return new WaitForSeconds(0.3f);

      Physics.hspeed = 0;
      Physics.vspeed = 0;
      yield return new WaitForSeconds(0.5f);

      StartCoroutine("ReturnToTig");
    }

    private IEnumerator DelayGravity() {
      float gravity = Physics.gravity;
      Physics.gravity = 0;
      yield return new WaitForSeconds(0.3f);
      Physics.gravity = gravity;
    }

    private IEnumerator AbsorbGhost() {
      ghostAbsorbed = true;
      RestartCoroutine("Shrink");
      yield return new WaitForSeconds(0.25f);
      StartCoroutine("ReturnToTig");
    }

    private IEnumerator Shrink() {
      for (float t = 0; t < 1f; t += 0.2f) {
        transform.localScale = Vector3.Lerp(Vector3.one * 2, Vector3.one, t);
        yield return null;
      }
    }

    private IEnumerator ReturnToTig() {
      for (float t = 0; t < 0.8f; t += 0.05f) {
        Position = Vector3.Lerp(Position, player.Mask.Center, t);
        yield return null;
      }

      if (ghostAbsorbed)
        player.State("ReviveHead");
      else
        Stitch.AddToAmmo(eAmmoType.HEAD);

      doneReturnToTig = true;
      DestroySelf();
    }
  }
}
