using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DisclaimerSceneSkip : MonoBehaviour
{

  [SerializeField] private float timeUntilTransition = 3f;

  void Start() => StartCoroutine(TransitionToMainScene());

  IEnumerator TransitionToMainScene()
  {
    while (timeUntilTransition >= 0f)
    {
      if (Input.anyKeyDown)
        break;
      timeUntilTransition -= Time.deltaTime;
      yield return null;
    }
    SceneManager.LoadSceneAsync("MainScene");
  }
}
