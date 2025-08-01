using NUnit.Framework;
using UnityEngine;

[CreateAssetMenu(fileName = "NewStats", menuName = "Asset/Player Stats")]
public class PlayerStats : ScriptableObject {

	[Header("Horizontal Movement")]

	// the maximum movement speed of the player
	public float maxSpeed;

	// the horizontal acceleration
	public float acceleration;

	[Header("Vertical Movement")]

	// the initial velocity at the begining of your jump
	public float jumpInitialSpeed;

	// gravity
	public float fallAcceleration;

	// the maximum fall speed
	public float maxFallSpeed;

	[Header("Wall Jump Physics")]

	// the initial jump velocity when jumping from a wall
	float wallJumpInitialSpeed;

	// the initial launch speed when jumping from a wall
	/// basically when jumping off the wall the game knows to move you away by giving this speed
	float wallJumpLaunchSpeed;
}
