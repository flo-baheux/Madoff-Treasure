using System;
using UnityEngine;

public class IntroScript : MonoBehaviour
{
  public Action IntroDone;

  [SerializeField] private GameObject introGameObject;

  private enum IntroState
  {
    ShipMoving,
    ShipStopped,
    DialogueIsActive,
    Done
  };
  private IntroState introState = IntroState.ShipMoving;

  void Awake()
  {
    introGameObject.SetActive(false);
  }

  void Update()
  {
    if (introState == IntroState.ShipStopped)
    {
      introGameObject.SetActive(true);
      introState = IntroState.DialogueIsActive;
    }

    if (introState == IntroState.DialogueIsActive && Input.anyKeyDown)
    {
      introState = IntroState.Done;
      IntroDone?.Invoke();
      introGameObject.SetActive(false);
    }
  }

  public void AnimationEventMainShipStoppedMoving() => introState = IntroState.ShipStopped;
}
