using UnityEngine;
using UnityEngine.Events;

public abstract class BaseMenuScreen : MonoBehaviour {

	// callback for the menu that allows it to change to another menu
	// args:
	// int menuIndex
	[HideInInspector] public UnityEvent<int> changeMenu;

	// callback for when the menu wants to exit
	[HideInInspector] public UnityEvent exitMenu;

	// called to update this menu
	public abstract void UpdateMenu(Vector2 p1Direction, bool p1Confirm, Vector2 p2Direction, bool p2Confirm);

	// sets the visibility of the menu
	public abstract void SetAlpha(float alpha);

}
