using System;
using UnityEngine;

public class PlayerData : ScriptableObject
{
  // Coins
  public int coins = 0;
  public int maxCoins = 100;

  // Speed
  public float speedMultiplier = 1;
  public readonly int defaultSpeed = 300;

  // Dash
  public float dashStrength = 300;
  public float dashCooldownS = 3;

  // Status?
}