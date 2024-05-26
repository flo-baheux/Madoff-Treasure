using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
  [SerializeField] private float dampTime = 0.4f;
  [SerializeField] private Transform player;
  private Vector3 velocity = Vector3.zero;

  void FixedUpdate()
  {
    if (player.gameObject.activeSelf)
    {
      // TODO: Find out why it seems overcomplicated when checking online
      Vector3 target = new(player.position.x, player.position.y, transform.position.z);
      transform.position = Vector3.SmoothDamp(transform.position, target, ref velocity, dampTime);
    }
  }
}
