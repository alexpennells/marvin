using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace DestroyableGrave {
  public class Base : SolidObj {
    public List<Sprite> sprites = new List<Sprite>();

    private SpriteRenderer spriteRenderer;
    private int stage = 0;

    public override void LoadReferences() {
      spriteRenderer = transform.parent.GetComponent<SpriteRenderer>();
      Sound = new Sound();
      base.LoadReferences();
    }

    public void StateDamage() {
      stage += 1;
      Sound.Play("Crack");

      if (stage <= 2)
        spriteRenderer.sprite = sprites[stage];
      else {
        DestroySelf();
        GameObject.Destroy(transform.parent.gameObject);
      }
    }
  }
}
