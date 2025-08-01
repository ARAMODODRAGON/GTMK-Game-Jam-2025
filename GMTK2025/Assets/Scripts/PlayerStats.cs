using NUnit.Framework;
using UnityEngine;

[CreateAssetMenu(fileName = "NewStats", menuName = "Asset/Player Stats")]
public class PlayerStats : ScriptableObject {

	// the maximum movement speed of the player
	public float moveSpeed;

	// the initial velocity at the begining of your jump
	public float jumpInitialSpeed;

	// gravity
	public float fallAcceleration;

	// the maximum fall speed
	public float maxFallSpeed;
}
