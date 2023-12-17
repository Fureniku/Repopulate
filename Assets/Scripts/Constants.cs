using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Constants 
{
    public static LayerMask MASK_STANDABLE = LayerMask.GetMask("Floor") | LayerMask.GetMask("Wall") | LayerMask.GetMask("BuildingGrid") | LayerMask.GetMask("Placeable");
    public static LayerMask MASK_BUILDABLE = LayerMask.GetMask("BuildingGrid") | LayerMask.GetMask("Placeable");
}
