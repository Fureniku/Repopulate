using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class RaycastDebugger : MonoBehaviour
{
	private Camera mainCamera;

	private void Start()
	{
		mainCamera = Camera.main;
	}

	private void Update()
	{
		// Get the current mouse position from the new Input System
		Vector2 mousePosition = Mouse.current.position.ReadValue();

		// Cast a ray from the mouse position
		Ray ray = mainCamera.ScreenPointToRay(mousePosition);
		RaycastHit hit;

		// Perform the raycast against UI elements
		if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("UI")))
		{
			// Draw a debug ray to visualize the raycast
			Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.green);

			// Log the name of the UI object hit by the raycast
			Debug.Log("Hit UI object: " + hit.collider.gameObject.name);
		}
		else
		{
			// Draw a debug ray to visualize the raycast (if it didn't hit anything)
			Debug.DrawRay(ray.origin, ray.direction * 1000, Color.red);
		}
	}
}