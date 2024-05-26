using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
  [SerializeField] private GameObject actionSlider;
  [SerializeField] private GameObject playerShip;
  [SerializeField] private GameObject treasure;
  [SerializeField] private GameObject gameplay;
  [SerializeField] private GameObject callToAction;
  [SerializeField] private GameObject gameTimer;
  [SerializeField] private GameObject minimap;
  [SerializeField] private GameObject arrowToShip;
  [SerializeField] private GameObject gameOverScreen;

  private bool gameStarted = false;
  private bool interactionInProgress = false;
  private bool canInteractWithSomething = false;

  private string TimerFloatToString(float timer)
  {
    int mins = (int)(timer / 60f);
    int secs = (int)(timer % 60f);
    return $"{mins:00}:{secs:00}";
  }

  void Awake()
  {
    playerShip.GetComponent<PlayerShip>().OnTimedActionStart += OnTimedActionStartHandler;
    playerShip.GetComponent<PlayerShip>().OnTimedActionInProgress += OnTimedActionInProgressHandler;
    playerShip.GetComponent<PlayerShip>().OnTimedActionDone += OnTimedActionDoneHandler;
    playerShip.GetComponent<PlayerShip>().OnTimedActionCancel += OnTimedActionCancelHandler;

    playerShip.GetComponent<PlayerShip>().OnActionAvailable += OnActionAvailableHandler;
    playerShip.GetComponent<PlayerShip>().OnActionNotAvailable += OnActionNotAvailableHandler;

    gameplay.GetComponent<Gameplay>().OnGameStarted += OnGameStartedHandler;
    gameplay.GetComponent<Gameplay>().OnGameOver += OnGameOverHandler;

    actionSlider.SetActive(false);
    callToAction.SetActive(false);
    gameTimer.SetActive(false);
    arrowToShip.SetActive(false);
    gameOverScreen.SetActive(false);
    minimap.SetActive(false);
  }

  void Update()
  {
    if (!gameStarted)
      return;

    callToAction.SetActive(!interactionInProgress && canInteractWithSomething);
    actionSlider.SetActive(interactionInProgress);

    float timer = gameplay.GetComponent<Gameplay>().timerUntilGameEndsSecs;
    gameTimer.GetComponent<TextMeshProUGUI>().text = TimerFloatToString(timer);

    if (!treasure.GetComponent<SpriteRenderer>().isVisible)
    {
      arrowToShip.SetActive(true);
      arrowToShip.transform.up = treasure.transform.position - playerShip.transform.position;
    }
    else
    {
      arrowToShip.SetActive(false);
    }
  }

  private void OnTimedActionStartHandler(float timeRequired)
  {
    interactionInProgress = true;
    actionSlider.SetActive(true);
    callToAction.SetActive(false);
    actionSlider.GetComponent<Slider>().maxValue = timeRequired;
  }

  private void OnTimedActionInProgressHandler(float timeSpentLoading)
    => actionSlider.GetComponent<Slider>().value = timeSpentLoading;

  private void OnTimedActionDoneHandler() => interactionInProgress = false;
  private void OnTimedActionCancelHandler() => interactionInProgress = false;
  private void OnActionNotAvailableHandler() => canInteractWithSomething = false;

  private void OnActionAvailableHandler(InteractableEntity entity)
  {
    canInteractWithSomething = true;
    switch (entity)
    {
      // Hardcoded text meh... 
      case InteractableEntity.MainShip:
        callToAction.GetComponent<TextMeshProUGUI>().text = "Press and hold SPACE to load coins from captain's ship";
        break;
      case InteractableEntity.Island:
        callToAction.GetComponent<TextMeshProUGUI>().text = "Press and hold SPACE to drop coins on the island";
        break;
    }
  }

  private void OnGameStartedHandler()
  {
    gameStarted = true;
    gameTimer.SetActive(true);
    minimap.SetActive(true);
  }

  private void OnGameOverHandler(bool playerWon)
  {
    callToAction.SetActive(false);
    actionSlider.SetActive(false);
    gameOverScreen.SetActive(true);
    gameOverScreen.transform.Find(playerWon ? "Canvas/Success" : "Canvas/Failure").gameObject.SetActive(true);
  }
}
