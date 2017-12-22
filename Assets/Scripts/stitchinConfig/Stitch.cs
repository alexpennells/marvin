using UnityEngine;
using System;

public class Stitch : MonoBehaviour
{
  /***********************************
   * DRAWING CONSTANTS
   **********************************/

  public static bool SHOW_GAME_GRID = false;
  public static bool SHOW_OBJ_ORIGIN = true;
  public static bool SHOW_MASK_BOUNDS = true;
  public static bool SHOW_TERRAIN_BOUNDS = true;
  public static bool SHOW_WATER_BOUNDS = true;
  public static bool SHOW_RAIL_PATHS = true;
  public static bool SHOW_CAMERA_BOUNDS = true;
  public static bool SHOW_EXIT_BOUNDS = true;
  public static bool SHOW_ENTRANCES = true;

  /***********************************
   * GAME GLOBALS
   **********************************/

  void Awake() {
    instance = this;
  }

  // Treat this class as a singleton. This will hold the instance of the class.
  private static Stitch instance;
  public static Stitch Instance { get { return instance; } }
}
