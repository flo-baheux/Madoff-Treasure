using System;
using UnityEngine;

public class Gameplay : MonoBehaviour
{
  [SerializeField] private GameObject mainShip;
  [SerializeField] private GameObject playerShip;
  [SerializeField] private IntroScript introScript;

  private bool gameStarted = false;
  private bool mainShipReadyToEscapeTaxes = false;
  private bool lastCoinsDropped = false;

  public float timerUntilGameEndsSecs = 60 * 3;
  public Action OnGameStarted;
  public Action<bool> OnGameOver;

  void Start()
  {
    introScript.IntroDone += IntroDoneHandler;

    playerShip.SetActive(false);

    mainShip.GetComponent<MainShip>().OnEnoughToDodgeTaxes += OnEnoughToDodgeTaxesHandler;
    playerShip.GetComponent<PlayerShip>().OnCoinsDropped += OnCoinsDroppedHandler;
    OnGameOver += GameOverHandler;
  }

  void Update()
  {
    if (gameStarted)
      timerUntilGameEndsSecs -= Time.deltaTime;
    if (mainShipReadyToEscapeTaxes && lastCoinsDropped)
      OnGameOver?.Invoke(true);
    else if (timerUntilGameEndsSecs <= 0)
      OnGameOver?.Invoke(false);
  }

  private void IntroDoneHandler() => startGame();

  public void startGame()
  {
    gameStarted = true;
    playerShip.SetActive(true);
    mainShip.SetActive(true);
    OnGameStarted?.Invoke();
  }

  private void GameOverHandler(bool playerWon) => Time.timeScale = 0;
  private void OnEnoughToDodgeTaxesHandler() => mainShipReadyToEscapeTaxes = true;
  private void OnCoinsDroppedHandler() => lastCoinsDropped = mainShipReadyToEscapeTaxes;
}
