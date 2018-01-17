using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Hand {
  public class Base : MovingSolidObj {
    private bool moveUp = false;

    protected override void Init () {
      Physics.vspeed = (float)Game.Random.Next(0, 101) / 400f - 0.125f;
      StartCoroutine("Grow");
      transform.localScale = new Vector3(0, 1, 1);
    }

    protected override void Move() {
      if (moveUp)
        Physics.vspeed += 0.005f;
      else
        Physics.vspeed -= 0.005f;

      if (Physics.vspeed > 0.125f)
        moveUp = false;
      else if (Physics.vspeed < -0.125f)
        moveUp = true;
    }

    public override void DestroySelf() {
      Game.CreateParticle("WillyFistPuff", Position);
      base.DestroySelf();
    }

    /***********************************
     * CO-ROUTINES
     **********************************/

    private IEnumerator Grow() {
      while (transform.localScale.x < 1) {
        transform.localScale = new Vector3(transform.localScale.x + 0.1f, 1, 1);
        yield return new WaitForSeconds(0.01f);
      }
    }
  }
}
