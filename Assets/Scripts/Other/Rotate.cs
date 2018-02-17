using UnityEngine;
using System;

public class Rotate : MonoBehaviour {
  public float rotateSpeed = 1f;

  void Update() {
    transform.Rotate(Vector3.forward * Time.deltaTime * rotateSpeed, Space.World);
  }
}
