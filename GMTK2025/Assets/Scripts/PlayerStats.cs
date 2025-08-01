using NUnit.Framework;
using UnityEngine;

[CreateAssetMenu(fileName = "NewStats", menuName = "Asset/Player Stats")]
public class PlayerStats : ScriptableObject {

	// tips:
	
	// to make the player less "floaty", increase both the fallAcceleration and the 
	// jumpInitialSpeed to both achieve the same jump height at a different speed
	// eg:
	// fallAcceleration = 10, jumpInitialSpeed = 8
	// vs
	// fallAcceleration = 140, jumpInitialSpeed = 30
	// both achieve the same jump height at different speeds
	
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

	// multiplies the gravity to achieve a smaller jump height when jump is not held
	public float fallMultiplier;

	// the maximum fall speed
	public float maxFallSpeed;

	[Header("Wall Jump Physics")]

	// the initial jump velocity when jumping from a wall
	public float wallJumpInitialSpeed;

	// the initial launch speed when jumping from a wall
	/// basically when jumping off the wall the game knows to move you away by giving this speed
	public float wallJumpLaunchSpeed;

	// the maximum speed you can fall at when touching a wall
	public float maxSlideSpeed;

	// the wall slide acceleration
	/// applied only when holding toward the wall
	public float slideAcceleration;


}
