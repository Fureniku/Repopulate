using TMPro;
using UnityEngine;

public class DebugUIHandler : MonoBehaviour {

    [SerializeField] private CharacterController character;

    [SerializeField] private TMP_Text grounded;
    [SerializeField] private TMP_Text forcedGrounded;
    [SerializeField] private TMP_Text inGravity;
    [SerializeField] private TMP_Text currentGravity;
    
    

    // Update is called once per frame
    void Update() {
        DroidController droid = character.GetCurrentDroid();
        
        grounded.SetText($"Grounded: {droid.isGrounded}");
        forcedGrounded.SetText($"Forced Grounded: {droid.forcedNotGrounded}, count: {droid.forcedNotGroundedCount}");
        inGravity.SetText($"In Gravity: {droid.isInGravity}");
        currentGravity.SetText($"Current gravity source: {droid.CurrentGravitySource()}");


    }
}
