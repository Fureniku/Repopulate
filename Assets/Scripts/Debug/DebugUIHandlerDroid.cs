using UnityEngine;

public class DebugUIHandlerDroid : DebugUIHandlerBase {

    [SerializeField] private DroidController droid;
    
    private bool Grounded => droid.isGrounded;
    private bool ForceNotGrounded => droid.forcedNotGrounded;
    private bool IsInGravity => droid.DroidGao.IsInGravity;
    private GravityBase CurrentGravitySource => droid.CurrentGravitySource;
    private PreviewItem CurrentHeldItem => droid.GetPreviewItem();

    protected override void SetupTexts() {
        texts.Add(CreateEntry(nameof(Grounded)));
        texts.Add(CreateEntry(nameof(ForceNotGrounded)));
        texts.Add(CreateEntry(nameof(IsInGravity)));
        texts.Add(CreateEntry(nameof(CurrentGravitySource)));
        texts.Add(CreateEntry(nameof(CurrentHeldItem)));
    }

    protected override void UpdateTexts() {
        UpdateText(nameof(Grounded), $"Grounded:: {Grounded}");
        UpdateText(nameof(ForceNotGrounded), $"Forced Grounded: {ForceNotGrounded}, count: {droid.forcedNotGroundedCount}");
        UpdateText(nameof(IsInGravity), $"In Gravity: {IsInGravity}");
        UpdateText(nameof(CurrentGravitySource), $"Current gravity source: {CurrentGravitySource}");
        UpdateText(nameof(CurrentHeldItem), $"Held item: {CurrentHeldItem.GetObject().name}");
    }
}
