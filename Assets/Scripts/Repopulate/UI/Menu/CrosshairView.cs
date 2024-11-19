using System;
using UnityEngine;
using UnityEngine.UI;

public class CrosshairView : PopupMenuBase {
	
	[SerializeField][Range(0, 100)] private float _centerGapDistance;
	[SerializeField][Range(2, 100)] private float _crosshairLength;
	[SerializeField][Range(2, 100)] private float _crosshairThickness;
	[SerializeField] private bool _hasDot;
	[SerializeField] private bool _isCircle;
	[SerializeField] private bool _isCustom;
	[SerializeField] private Color _color;

	[SerializeField] private RawImage _topLine;
	[SerializeField] private RawImage _leftLine;
	[SerializeField] private RawImage _rightLine;
	[SerializeField] private RawImage _bottomLine;

	private void OnEnable() {
		SetCrosshair();
	}

	private void OnValidate() {
		SetCrosshair();
	}

	protected override void SetData(object value) {
		
	}

	void SetCrosshair() {
		SetCrosshairComponent(_topLine, new Vector2(0, 1));
		SetCrosshairComponent(_bottomLine, new Vector2(0, -1));
		SetCrosshairComponent(_leftLine, new Vector2(-1, 0));
		SetCrosshairComponent(_rightLine, new Vector2(1, 0));
	}

	void SetCrosshairComponent(RawImage crosshairSection, Vector2 axis) {
		float thickness = axis.y != 0 ? _crosshairThickness : _crosshairLength;
		float length = axis.y != 0 ? _crosshairLength : _crosshairThickness;
		float offset = _crosshairLength / 2 + _centerGapDistance;
		crosshairSection.rectTransform.sizeDelta = new Vector2(thickness, length);
		crosshairSection.color = _color;
		crosshairSection.transform.localPosition = new Vector2(offset, offset) * axis;
	}
}
