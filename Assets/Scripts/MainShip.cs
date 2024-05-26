using System;
using UnityEngine;

public class MainShip : MonoBehaviour
{
  public Action OnEnoughToDodgeTaxes;
  public Action OnNearlyEnoughToDodgeTaxes;

  [SerializeField] private int maxCoinsLoaded = 1000;
  [SerializeField] private int coinsCurrentlyLoaded = 1000;

  private Vector3 initialTreasureScale;

  void Start()
  {
    initialTreasureScale = transform.Find("Treasure").localScale;
  }

  private void shrinkTreasure()
  {
    float shrinkByValue = (float)coinsCurrentlyLoaded / (float)maxCoinsLoaded;
    Vector3 shrinkVector = new Vector3(shrinkByValue, shrinkByValue, 1);

    transform.Find("Treasure").localScale = Vector3.Scale(initialTreasureScale, shrinkVector);
  }

  public bool HasCoins()
  {
    return coinsCurrentlyLoaded > 0;
  }

  public int GetCoins(int coinsToRemove)
  {
    int coinsRemoved = coinsToRemove >= coinsCurrentlyLoaded ? coinsCurrentlyLoaded : coinsToRemove;
    coinsCurrentlyLoaded -= coinsRemoved;
    shrinkTreasure();
    // if (coinsCurrentlyLoaded <= maxCoinsLoaded / 2)
    //   OnNearlyEnoughToDodgeTaxes?.Invoke();
    if (coinsCurrentlyLoaded <= maxCoinsLoaded / 2)
      OnEnoughToDodgeTaxes?.Invoke();
    return coinsRemoved;
  }
}
