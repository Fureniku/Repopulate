using System;
using UnityEngine;

[ExecuteInEditMode]
public class ModularCorridorSegmentController : MonoBehaviour {
    
    [Header("Debug functions")]
    [ButtonInvoke(nameof(BuildSegment))] public bool _debugBuild;
    
    [Header("Gameplay")]
    [SerializeField] private GameObject _floorBasic;
    [SerializeField] private GameObject _floorWindow;
    //[SerializeField] private GameObject _floorMidStairs;
    //[SerializeField] private GameObject _floorMidStairsHole;
    [SerializeField] private GameObject _wallBasic;
    [SerializeField] private GameObject _wallWindow;
    [SerializeField] private GameObject _wallDoor;
    [SerializeField] private GameObject _wallStairsA;
    [SerializeField] private GameObject _wallStairsB;
    [SerializeField] private GameObject _wallStairsInA;
    [SerializeField] private GameObject _wallStairsInB;
    [SerializeField] private GameObject _ceilingBasic;
    [SerializeField] private GameObject _ceilingWindow;
    //[SerializeField] private GameObject _ceilingStairsA;
    //[SerializeField] private GameObject _ceilingStairsB;

    [SerializeField] private ModuleSlotContents _leftSide;
    [SerializeField] private ModuleSlotContents _rightSide;

    [SerializeField] private bool _rebuild = false;

    private GameObject _floor;
    private GameObject _wallLeft;
    private GameObject _wallRight;
    private GameObject _ceiling;

    private bool _leftOccupied = false;
    private bool _rightOccupied = false;
    private bool _stairsBelow = false;

    void Update() {
        if (_rebuild) {
            ClearSegment();
            BuildSegment();
        }
    }

    void ClearSegment() {
        DestroyImmediate(_floor);
        DestroyImmediate(_wallLeft);
        DestroyImmediate(_wallRight);
        DestroyImmediate(_ceiling);
    }
    
    void BuildSegment() {
        if (_stairsBelow) {
            //_floor = Instantiate(_floorMidStairsHole, transform);
        }
        else {
            _floor = Instantiate(_floorBasic, transform);
        }

        _wallLeft = Instantiate(SelectPrefabForSlot(_leftSide), transform);
        _wallRight = Instantiate(SelectPrefabForSlot(_rightSide), Vector3.zero, Quaternion.Euler(0, 180, 0), transform);
        _ceiling = Instantiate(_ceilingBasic, transform);

        _rebuild = false;
    }

    GameObject SelectPrefabForSlot(ModuleSlotContents slotContents) {
        switch (slotContents) {
            case ModuleSlotContents.NONE:
                return _wallBasic;
            case ModuleSlotContents.WINDOW:
                return _wallWindow;
            case ModuleSlotContents.DOOR:
                return _wallDoor;
            case ModuleSlotContents.STAIRS_UP:
                return _wallStairsA;
            case ModuleSlotContents.STAIRS_DOWN:
                return _wallStairsB;
        }
        return null;
    }
}

enum ModuleSlotContents {
    NONE,
    WINDOW,
    DOOR,
    STAIRS_UP,
    STAIRS_DOWN,
}
