using System;
using System.Collections.Generic;
using Repopulate.Inventory;
using UnityEngine;

//Overall class for all droids, regardless of controller
namespace Repopulate.Player {
    public class DroidManager : MonoBehaviour {

        [SerializeField] private PreviewConstruct _previewConstruct;
        [SerializeField] private List<DroidControllerBase> _droidList;

        public void Start() {
            int droidCount = _droidList.Count;
            for (int i = 0; i < droidCount; i++) {
                _droidList[i].InitializeDroid(this, _previewConstruct);
            }
        }

        public void AssignNextAvailableDroid(CharacterController character) {
            DroidControllerBase currentDroid = character.GetCurrentDroid();
            int currentDroidId = _droidList.IndexOf(currentDroid);
            if (currentDroidId == -1) {
                Debug.LogError($"Unable to find droid {currentDroid.name} in the droid list!");
                return;
            }
            currentDroid.SetDroidActive(false);
            DroidControllerBase newDroid = FindNextAvailableDroid(currentDroidId);
            character.SetDroid(newDroid);
            newDroid.SetDroidActive(true);
        }
    
        private DroidControllerBase FindNextAvailableDroid(int startIndex)
        {
            int currentIndex = startIndex;

            do {
                currentIndex = (currentIndex + 1) % _droidList.Count;

                if (!_droidList[currentIndex].IsDroidActive) {
                    return _droidList[currentIndex];
                }

            } while (currentIndex != startIndex);

            Debug.LogWarning("No alternative droid found to switch to");
            return _droidList[startIndex];
        }

        public void AddNewDroid(DroidControllerBase droid) {
            _droidList.Add(droid);
        }
    }
}
