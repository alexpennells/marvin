using UnityEngine;
using System;
using System.Collections.Generic;

public class Stitch : MonoBehaviour {
  /***********************************
   * DRAWING CONSTANTS
   **********************************/

  public static bool SHOW_GAME_GRID = false;
  public static bool SHOW_OBJ_ORIGIN = false;
  public static bool SHOW_MASK_BOUNDS = false;
  public static bool SHOW_TERRAIN_BOUNDS = false;
  public static bool SHOW_WATER_BOUNDS = false;
  public static bool SHOW_RAIL_PATHS = false;
  public static bool SHOW_CAMERA_BOUNDS = false;
  public static bool SHOW_EXIT_BOUNDS = false;
  public static bool SHOW_ENTRANCES = false;

  /***********************************
   * GAME GLOBALS
   **********************************/

  private static List<eAmmoType> ammo = new List<eAmmoType>();
  public static List<eAmmoType> Ammo { get { return ammo; } }

  private Player.Base player;
  private List<Vector3> playerPositionQueue = new List<Vector3>();
  private List<GameObject> followAmmo = new List<GameObject>();
  private int followDistance = 4;

  // Treat this class as a singleton. This will hold the instance of the class.
  private static Stitch instance;
  public static Stitch Instance { get { return instance; } }

  /***********************************
   * FUNCTIONS
   **********************************/

  void Awake() { instance = this; }

  void Start() {
    player = GameObject.Find("Player").GetComponent<Player.Base>();

    while (playerPositionQueue.Count < followDistance * 5)
      playerPositionQueue.Add(player.Position);
  }

  void Update() {
    updatePlayerPositionQueue();
    updateFollowAmmoPosition();
  }

  public static void AddToAmmo(eAmmoType t) {
    if (t == eAmmoType.HEAD)
      ammo.Insert(0, t);
    else
      ammo.Add(t);
  }
  public static void PopAmmo() { ammo.RemoveAt(0); }
  public void CycleAmmo() {
    if (Stitch.Ammo.Count > 0) {
      eAmmoType oldAmmo = Stitch.Ammo[0];
      Stitch.PopAmmo();
      Stitch.AddToAmmo(oldAmmo);
      cleanFollowAmmo();
    }
  }

  /***********************************
   * FUNCTIONS
   **********************************/

  private void updatePlayerPositionQueue() {
    if (player && playerPositionQueue.Count <= followDistance * 5)
      playerPositionQueue.Add(player.Position);

    if (playerPositionQueue.Count > followDistance * 5)
      playerPositionQueue.RemoveAt(0);
  }

  private void updateFollowAmmoPosition() {
    if (followAmmo.Count != Stitch.Ammo.Count) {
      cleanFollowAmmo();

      int sortingLayer = -1;
      foreach (eAmmoType a in Stitch.Ammo) {
        GameObject obj = Instantiate(Resources.Load("Objects/Ammo/Follow/" + Stitch.GetAmmoType(a))) as GameObject;
        obj.GetComponent<SpriteRenderer>().sortingOrder = sortingLayer;
        obj.transform.parent = player.transform;
        sortingLayer--;

        followAmmo.Add(obj);
      }
    }

    bool first = true;
    int i = followDistance * 4;
    foreach (GameObject a in followAmmo) {
      if (first && player.Is("Throwing") && player.AmmoPosition() != Vector3.zero) {
        a.GetComponent<SpriteRenderer>().sortingOrder = 1;
        a.transform.localPosition = player.AmmoPosition();
      } else {
        if (first) { a.GetComponent<SpriteRenderer>().sortingOrder = -1; }
        a.transform.position = playerPositionQueue[i] + Vector3.up * 0.06f;
      }

      a.transform.rotation = Quaternion.Euler(0, 0, 0);
      a.transform.localScale = Vector3.one;
      i = i - followDistance;
      first = false;
    }
  }

  private void cleanFollowAmmo() {
    foreach (GameObject a in followAmmo) { Destroy(a); }
    followAmmo.Clear();
  }

  public static string GetAmmoType(eAmmoType a) {
    switch (a) {
      case eAmmoType.HEAD:
        return "Head";
      case eAmmoType.BALL:
        return "Ball";
      default:
        return "Skull";
    }
  }
}
