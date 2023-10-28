using System;
using UnityEngine;

namespace Repopulate.World {
	[Serializable]
	public class CelestialData {

		[SerializeField] private string _objectName;
		[SerializeField] private CelestialType _objectType;
		[Tooltip("The rotation/orbit speed in days per year")]
		[SerializeField] private float _orbitSpeed = 365f; // Adjust this to change the rotation speed (degrees per second)
		[SerializeField] private float _calculatedOrbitSpeed = 0.01f; // Adjust this to change the rotation speed (degrees per second)
		[SerializeField] private int _distanceFromCentre = 100;
		
		[Header("Dynamic Rescaling")]
		[SerializeField] private bool _canDynamiclyRescale = true;
		[Tooltip("The distance where scale will be at maximum, and no longer increase")]
		[SerializeField] private float _minDistance = 5000f;
		[Tooltip("The distance at which scale for the object would be minimum.")]
		[SerializeField] private float _maxDistance = 100000f;
		[Tooltip("The distance from the *planets surface* (not centre) where scale should be max")]
		[SerializeField] private float _scaleMax;
		[SerializeField] private float _scaleMin;

		public string Name => _objectName;
		public CelestialType BodyType => _objectType;
		public float OrbitSpeed => _calculatedOrbitSpeed;
		public float Distance => _distanceFromCentre;
		public bool CanDynamicallyRescale => _canDynamiclyRescale;
		public float MinimumDistance => _minDistance;
		public float MaximumDistance => _maxDistance;
		public float MinimumScale => _scaleMin;
		public float MaximumScale => _scaleMax;
		public CelestialBodyController CelestialController { get; private set; }

		public void SetData(CelestialBodyController controller) {
			CelestialController = controller;
			controller.SetData(this);
		}

		public void SetCalculatedSpeed(float degsPerSecond) {
			_calculatedOrbitSpeed = _orbitSpeed == 0 ? 0 : degsPerSecond * (365 / _orbitSpeed);
		}
	}

	public enum CelestialType {
		STAR,
		PLANET_ROCK,
		PLANET_GAS,
		PLANET_DWARF,
		MOON,
		ASTEROID,
		COMET,
		ARTIFICIAL
	}
}

