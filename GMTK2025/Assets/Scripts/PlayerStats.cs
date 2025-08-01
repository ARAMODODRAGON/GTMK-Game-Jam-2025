using NUnit.Framework;
using UnityEngine;

[CreateAssetMenu(fileName = "NewStats", menuName = "Asset/Player Stats")]
public class PlayerStats : ScriptableObject {

	// the maximum movement speed of the player
	[SerializeField] public float moveSpeed;

	// the initial velocity at the begining of your jump
	[SerializeField] public float jumpHeight;

}
