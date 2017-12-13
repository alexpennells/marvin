using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class TargetTerror : MonoBehaviour
{
  private StitchCamera camera;

  void Awake () {
    camera = GameObject.Find("Camera").GetComponent<StitchCamera>();
  }

  void Update () {
    if (camera.x < 2f)
      camera.x = 2f;

    // if (camera.y < 1f)
      camera.y = 1f;
  }
}
