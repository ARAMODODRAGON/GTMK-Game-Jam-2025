using UnityEngine;
using UnityEngine.Events;

public abstract class BaseMenuScreen : MonoBehaviour {

	// callback for the menu that allows it to change to another menu
	// args:
	// int menuIndex
	public UnityEvent<int> changeMenu;

	// callback for when the menu wants to exit
	public UnityEvent exitMenu;

	// called to update this menu
	public abstract void UpdateMenu(bool upInput, bool downInput, bool leftInput, bool rightInput, bool confirmInput);

	// sets the visibility of the menu
	public abstract void SetAlpha(float alpha);

}
