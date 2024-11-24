using Repopulate.ScriptableObjects;
using UnityEngine;
using UnityEngine.InputSystem;

//Wrapper to control the currently selected droid from the player, plus anything global for the player
namespace Repopulate.Player {
    public class CharacterController : MonoBehaviour {

        [Header("Gameplay stuff")]
        [SerializeField] private DroidControllerBase _currentDroid;

        private bool _isPlayerDroidActive = true; //True when a droid is currently controllable, false when in a UI or another mode

        public DroidControllerBase GetCurrentDroid() => _currentDroid;

        public void SetDroid(DroidControllerBase newDroid) {
            _currentDroid.SetCameraStatus(false);
            _currentDroid = newDroid;
            _currentDroid.SetCameraStatus(true);
            transform.SetParent(newDroid.transform, false);
        }

        private void Awake() {
            Cursor.lockState = CursorLockMode.Locked;
            _currentDroid.SetDroidActive(true);
        }

        public void SetPlayerActive(bool active) {
            Cursor.lockState = active ? CursorLockMode.Locked : CursorLockMode.None;
            _isPlayerDroidActive = active;
            _currentDroid.SetDroidActive(true);
        }

        public void SetActive(bool active) {
            Debug.Log($"Droid active: {active}");
            _currentDroid.SetDroidActive(active);
        }
        
        public void ResetCamera() {
            _currentDroid.ResetCamera();
        }

        #region Input Controls

        #region Constant controls
        //Handle moving the droid
        public void HandleMovement(InputAction.CallbackContext context) {
            if (_isPlayerDroidActive && Cursor.lockState == CursorLockMode.Locked) {
                _currentDroid.HandleMovement(context.ReadValue<Vector2>());
            }
        }
        
        //Move the camera and look around, also aims the droid
        public void HandleCamera(InputAction.CallbackContext context) {
            if (_isPlayerDroidActive && Cursor.lockState == CursorLockMode.Locked) {
                _currentDroid.HandleCamera(context);
            }
        }
        
        //While in space, stabilise the droid and stop momentum (TODO unimplemented)
        public void HandleStabilisation(InputAction.CallbackContext context) {
            if (ShouldProcess(context, true)) {
                _currentDroid.HandleStabilisation();
            }
        }
        #endregion
        
        #region Tap controls
        //Switch to another control scheme
        public void HandleSwitch(InputAction.CallbackContext context) {
            if (ShouldProcess(context, true)) {
                SetActive(false);
                GameManager.Instance.SwitchControlType(EnumControlType.SHIP);
            }
        }
        
        //Open the droid's inventory
        public void HandleOpenInventory(InputAction.CallbackContext context) {
            if (context.performed) {
                _isPlayerDroidActive = !_isPlayerDroidActive;
                Cursor.lockState = _isPlayerDroidActive ? CursorLockMode.Locked : CursorLockMode.None;
                _currentDroid.InventoryVisible(!_isPlayerDroidActive);
            }
        }

        //Open pause if we're playing, else close any open menus and return to gameplay.
        public void HandlePause(InputAction.CallbackContext context) {
            if (context.performed) {
                _currentDroid.InventoryVisible(false);
                if (_isPlayerDroidActive) {
                    //pause
                }
                else {
                    
                }
                _isPlayerDroidActive = !_isPlayerDroidActive;
            }
        }
        
        //Switch the currently active droid - TODO this cycles, but when we have a lot there might be a better option. 
        public void HandleSwitchDroid(InputAction.CallbackContext context) {
            if (ShouldProcess(context)) {
                GameManager.Instance.GetDroidManager.AssignNextAvailableDroid(this);
            }
        }
        
        //Rotate the held construct
        public void HandleModifierInput(InputAction.CallbackContext context) {
            if (ShouldProcess(context, true)) {
                _currentDroid.DroidModifierInput();
            }
        }
        
        //Make the droid jump
        public void HandleJump(InputAction.CallbackContext context) {
            if (ShouldProcess(context, true)) {
                _currentDroid.HandleJump();
            }
        }
        
        //Places the currently selected construct in the world
        public void HandlePlaceObject(InputAction.CallbackContext context) {
            if (ShouldProcess(context, true)) {
                _currentDroid.DroidCreativeInput(context);
            }
        }
        
        //Handles interacting with the world from the droid (e.g. opening construct UIs or interacting with switches)
        public void HandleInteractPrimary(InputAction.CallbackContext context) {
            if (ShouldProcess(context, true)) {
                _currentDroid.InteractPrimary();
            }
        }
        
        public void HandleInteractSecondary(InputAction.CallbackContext context) {
            if (ShouldProcess(context, true)) {
                _currentDroid.InteractSecondary();
            }
        }
        
        public void HandleInteractTertiary(InputAction.CallbackContext context) {
            if (ShouldProcess(context, true)) {
                _currentDroid.InteractTertiary();
            }
        }
        #endregion

        //TODO what does this do?
        public void HandleActiveToggle(InputAction.CallbackContext context) {
            if (ShouldProcess(context, true)) {
                SetPlayerActive(!_isPlayerDroidActive);
            }
        }
        
        //TODO what does this do?
        public void HandleUIInput(InputAction.CallbackContext context) {
            if (_isPlayerDroidActive) {
                _currentDroid.HandleUIControlInput(context);
            }
        }
        #endregion

        private bool ShouldProcess(InputAction.CallbackContext context, bool lockedOnly = false) {
            if (lockedOnly && Cursor.lockState != CursorLockMode.Locked) {
                return false;
            }
            return context.performed && _isPlayerDroidActive;
        }
    }
}