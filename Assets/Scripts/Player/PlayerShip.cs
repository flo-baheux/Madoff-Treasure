using System;
using System.Collections;
using UnityEngine;

public enum InteractableEntity
{
  MainShip,
  Island,
};

public class PlayerShip : MonoBehaviour
{
  // Smells like a component would be nice to regroup those...
  public event Action<float> OnTimedActionStart;
  public event Action<float> OnTimedActionInProgress;
  public event Action OnTimedActionDone;
  public event Action OnTimedActionCancel;
  public event Action<InteractableEntity> OnActionAvailable;
  public event Action OnActionNotAvailable;
  public event Action OnCoinsDropped;

  [SerializeField] private Sprite defaultSprite;
  [SerializeField] private Sprite lateralMovementSprite;

  private PlayerData playerData;
  private PlayerControls controls;
  private SpriteRenderer spriteRenderer;
  private readonly float actionDurationSeconds = 2;

  void Start()
  {
    playerData = ScriptableObject.CreateInstance<PlayerData>();
    controls = new PlayerControls(gameObject, playerData);
    spriteRenderer = GetComponent<SpriteRenderer>();
  }

  void Update()
  {
    controls.CustomUpdate();

    // No time but this should be somewhere else T.T
    if (controls.getCurrentDirection().x != 0f)
    {
      spriteRenderer.sprite = lateralMovementSprite;
      spriteRenderer.flipX = controls.getCurrentDirection().x < 0f;
    }
    else
    {
      spriteRenderer.sprite = defaultSprite;
    }

    transform.Find("Treasure").gameObject.SetActive(playerData.coins > 0);
  }

  bool CanGetCoinsFromMainShip(MainShip ship) => playerData.coins < playerData.maxCoins && ship.HasCoins();

  // Should those coroutines be in a component?
  // Maybe the thing player interacts with should be the one sporting it
  // and player could be affected via callback
  IEnumerator GetCoinsFromMainShipCoroutine(MainShip mainShip)
  {
    float timeSpentLoading = 0;
    OnTimedActionStart?.Invoke(actionDurationSeconds);
    while (PlayerControls.IsActionButtonHeld() && timeSpentLoading <= actionDurationSeconds)
    {
      timeSpentLoading += Time.deltaTime;
      OnTimedActionInProgress?.Invoke(timeSpentLoading);
      yield return null;
    }
    if (timeSpentLoading >= actionDurationSeconds)
    {
      int coinsFromShip = mainShip.GetCoins(playerData.maxCoins - playerData.coins);
      playerData.coins += coinsFromShip;
      OnTimedActionDone?.Invoke();
    }
    else
    {
      OnTimedActionCancel?.Invoke();
    }
  }

  bool CanDepositCoinsOnIsland(Island island) => playerData.coins > 0 && !island.IsFull();

  IEnumerator DepositCoinsOnIslandCoroutine(Island island)
  {
    float timeSpent = 0;
    OnTimedActionStart?.Invoke(actionDurationSeconds);
    while (PlayerControls.IsActionButtonHeld() && timeSpent <= actionDurationSeconds)
    {
      timeSpent += Time.deltaTime;
      OnTimedActionInProgress?.Invoke(timeSpent);
      yield return null;
    }
    if (timeSpent >= actionDurationSeconds)
    {
      int coinsAddedToIsland = island.GetComponent<Island>().AddCoins(playerData.coins);
      playerData.coins -= coinsAddedToIsland;
      OnCoinsDropped?.Invoke();
      OnTimedActionDone?.Invoke();
    }
    else
    {
      OnTimedActionCancel?.Invoke();
    }
  }

  IEnumerator ApplyObstacleDebuffCoroutine(Obstacle obstacle)
  {
    float duration = obstacle.debuffDuration;
    float speedMultiplierBeforeDebuff = playerData.speedMultiplier;
    while (duration > 0)
    {
      duration -= Time.deltaTime;
      playerData.speedMultiplier = speedMultiplierBeforeDebuff - obstacle.speedMultiplierDebuff;
      yield return null;
    }
    playerData.speedMultiplier = speedMultiplierBeforeDebuff;
  }

  void OnTriggerEnter2D(Collider2D colliderInfo)
  {
    if (colliderInfo.gameObject.TryGetComponent(out Obstacle obstacle))
    {
      StartCoroutine(ApplyObstacleDebuffCoroutine(obstacle));
    }
  }

  void OnTriggerExit2D(Collider2D colliderInfo)
  {
    OnActionNotAvailable?.Invoke();
  }

  void OnTriggerStay2D(Collider2D colliderInfo)
  {
    if (colliderInfo.gameObject.TryGetComponent(out MainShip mainShip) && CanGetCoinsFromMainShip(mainShip))
    {
      OnActionAvailable?.Invoke(InteractableEntity.MainShip);
      if (PlayerControls.IsActionButtonHeld())
        StartCoroutine(GetCoinsFromMainShipCoroutine(mainShip));
    }
    else if (colliderInfo.gameObject.TryGetComponent(out Island island) && CanDepositCoinsOnIsland(island))
    {
      OnActionAvailable?.Invoke(InteractableEntity.Island);
      if (PlayerControls.IsActionButtonHeld())
        StartCoroutine(DepositCoinsOnIslandCoroutine(island));
    }
    else
    {
      OnActionNotAvailable?.Invoke();
    }
  }
}

