using Repopulate.Inventory;
using Repopulate.Physics.Gravity;
using Repopulate.Player;
using UnityEngine;

public class DebugUIHandlerDroid : DebugUIHandlerBase {

    [SerializeField] private DroidControllerBase droid;
    
    private bool Grounded => droid.IsGrounded;
    private bool ForceNotGrounded => droid.ForcedNotGrounded;
    private bool IsInGravity => droid.DroidGao.IsInGravity;
    private GravityBase CurrentGravitySource => droid.CurrentGravitySource;
    
    private GameObject CurretTarget => droid.CurrentAimTarget;

    private PreviewConstruct CurrentHeldItem = GameManager.Instance.PreviewConstruct;

    protected override void SetupTexts() {
        if (droid == null) {
            Debug.LogError("Droid is null, skipping debug setup");
            return;
        }

        if (texts == null) {
            Debug.LogError("Texts are null? aborting");
            return;
        }
        texts.Add(CreateEntry(nameof(Grounded)));
        texts.Add(CreateEntry(nameof(ForceNotGrounded)));
        texts.Add(CreateEntry(nameof(IsInGravity)));
        texts.Add(CreateEntry(nameof(CurrentGravitySource)));
        texts.Add(CreateEntry(nameof(CurretTarget)));

        if (droid.DroidType == DroidType.CONSTRUCTION) {
            texts.Add(CreateEntry(nameof(CurrentHeldItem)));
        }
    }

    protected override void UpdateTexts() {
        if (droid == null) {
            Debug.LogError("Droid is null, skipping debug update");
            return;
        }
        UpdateText(nameof(Grounded), $"Grounded:: {Grounded}");
        UpdateText(nameof(ForceNotGrounded), $"Forced Grounded: {ForceNotGrounded}, count: {droid.ForcedNotGroundedCount}");
        UpdateText(nameof(IsInGravity), $"In Gravity: {IsInGravity}");
        UpdateText(nameof(CurrentGravitySource), $"Current gravity source: {CurrentGravitySource}");
        UpdateText(nameof(CurretTarget), $"Current aimed object: {CurretTarget}");
        if (droid.DroidType == DroidType.CONSTRUCTION) {
            UpdateText(nameof(CurrentHeldItem), $"Held item: {CurrentHeldItem.GetObject().name}");
        }
    }
}
