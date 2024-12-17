using UnityEngine;

public static class Constants 
{
    public static LayerMask MASK_STANDABLE = LayerMask.GetMask("Floor") | LayerMask.GetMask("Wall") | LayerMask.GetMask("BuildingGrid") | LayerMask.GetMask("Placeable") | LayerMask.GetMask("PlaceableBase");
    public static LayerMask MASK_BUILDABLE = LayerMask.GetMask("BuildingGrid") | LayerMask.GetMask("Placeable") | LayerMask.GetMask("Interactable");
    public static LayerMask MASK_PLACEABLE = LayerMask.GetMask("Placeable") | LayerMask.GetMask("Interactable");
    public static LayerMask MASK_INTERACTABLE = LayerMask.GetMask("Interactable");
}
