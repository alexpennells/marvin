using UnityEngine;
using System;
using System.Collections;

namespace AmmoSkull {
  public class Base : BaseObj {
    private bool collectable = false;

    public override void LoadReferences() {
      Physics = new Physics(Physics);
      Sound = new Sound();
      Sprite = new Sprite();
      base.LoadReferences();
    }

    public override void Init () {
      StartCoroutine("Collectable");
      base.Init();
    }

    /***********************************
     * STATE CHECKERS
     **********************************/

    public bool IsCollectable() { return collectable; }

    /***********************************
     * CO-ROUTINES
     **********************************/

    private IEnumerator Collectable() {
      yield return new WaitForSeconds(0.5f);
      collectable = true;
    }
  }
}
