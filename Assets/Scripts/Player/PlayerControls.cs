using UnityEngine;

public class PlayerControls
{
  private readonly Transform transform;
  private readonly Rigidbody2D rigidBody;
  private readonly PlayerData playerData;

  private Vector3 currentDirection;

  public PlayerControls(GameObject playerGameObject, PlayerData playerData)
  {
    rigidBody = playerGameObject.GetComponent<Rigidbody2D>();
    transform = playerGameObject.transform;
    this.playerData = playerData;
  }

  private static readonly KeyCode actionButton = KeyCode.Space;
  private static readonly KeyCode dashButton = KeyCode.LeftShift;
  private float dashCooldownTimeLeft = 0;

  public void CustomUpdate()
  {
    float horizontalInput = Input.GetAxisRaw("Horizontal");
    float verticalInput = Input.GetAxisRaw("Vertical");
    if (horizontalInput != 0)
    {
      currentDirection += horizontalInput > 0 ? Vector3.right : Vector3.left;
      rigidBody.AddForce(horizontalInput * playerData.defaultSpeed * playerData.speedMultiplier * Time.deltaTime * transform.right);
    }

    if (verticalInput != 0)
    {
      currentDirection += verticalInput > 0 ? Vector3.up : Vector3.down;
      rigidBody.AddForce(playerData.defaultSpeed * playerData.speedMultiplier * Time.deltaTime * verticalInput * transform.up);
    }

    if (horizontalInput == 0 && verticalInput == 0)
      currentDirection = Vector3.zero;

    currentDirection = currentDirection.normalized;

    handleDash();
  }

  private void handleDash()
  {
    if (Input.GetKey(dashButton) && dashCooldownTimeLeft <= 0)
    {
      dashCooldownTimeLeft = playerData.dashCooldownS;
      rigidBody.AddForce(playerData.dashStrength * currentDirection.normalized);
    }

    if (dashCooldownTimeLeft >= 0f)
    {
      dashCooldownTimeLeft -= Time.deltaTime;
    }
  }

  public static bool IsActionButtonHeld() => Input.GetKey(actionButton);
  public Vector3 getCurrentDirection() => currentDirection;
}