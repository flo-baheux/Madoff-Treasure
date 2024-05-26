using System;
using TMPro;
using UnityEngine;

public class Island : MonoBehaviour
{
  [SerializeField] private int coinsOnIsland = 0;
  [SerializeField] private int maxCoinsOnIsland = 100;

  // void Update()
  // {
  //   GetComponentInChildren<TextMeshPro>().text = coinsOnIsland + "/" + maxCoinsOnIsland;
  // }

  public bool IsFull() => coinsOnIsland == maxCoinsOnIsland;

  public int AddCoins(int amountToAdd)
  {
    int amountAdded = coinsOnIsland + amountToAdd > maxCoinsOnIsland ? maxCoinsOnIsland - coinsOnIsland : amountToAdd;
    coinsOnIsland += amountAdded;
    transform.Find("Gold").gameObject.SetActive(true);
    return amountAdded;
  }
}
