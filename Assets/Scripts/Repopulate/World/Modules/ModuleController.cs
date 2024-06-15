using System.Collections.Generic;
using Repopulate.UI;
using Repopulate.World.Constructs;
using Repopulate.World.Utilities;
using UnityEngine;

namespace Repopulate.World.Modules {
    public class ModuleController : MonoBehaviour, UIFillable {

        [SerializeField] private bool _isBuilt = false;
        [SerializeField] private bool _isEnterable = false; //Whether there should be doors, oxygen, etc
        [SerializeField] private int _buildTime;
        [SerializeField] private ConstructGrid _grid;
    
        private float _oxygenPressure;

        private int _currentBuilt;
    
        void FixedUpdate() {
            if (_isBuilt) {
                return;
            }
        
            if (_currentBuilt < _buildTime) {
                _currentBuilt++;
            } else {
                _isBuilt = true;
                Debug.Log("Module constructed!");
                if (_isEnterable) {
                    //TODO handle entering/door controls
                }
            }
        }
    
        public float GetProgress() {
            return _currentBuilt / (float) _buildTime;
        }

        public float GetProducedOxygen() {
            List<GameObject> objects = _grid.GetAllAttachedObjects();
            _oxygenPressure = 0;

            for (int i = 0; i < objects.Count; i++) {
                OxygenProducer oxygen = objects[i].GetComponent<OxygenProducer>();
                if (oxygen != null) {
                    _oxygenPressure += oxygen.GetProducedAmount();
                }
            }
        
            return _oxygenPressure;
        }
    }
}
