using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class TargetTerror : MonoBehaviour
{
  private StitchCamera camera;
  private Player.Base player;

  void Awake () {
    camera = GameObject.Find("Camera").GetComponent<StitchCamera>();
    player = GameObject.Find("Player").GetComponent<Player.Base>();
  }

  void Update () {
    if (camera.x < 2f)
      camera.x = 2f;

    // if (player.y > -0.32)
      camera.y = 1f;
  }
}
