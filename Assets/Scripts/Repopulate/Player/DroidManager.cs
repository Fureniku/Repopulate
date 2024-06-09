using System.Collections.Generic;
using UnityEngine;

//Overall class for all droids, regardless of controller
namespace Repopulate.Player {
    public class DroidManager : MonoBehaviour {
        
        [SerializeField] private List<DroidController> _droidList;

        public void AssignNextAvailableDroid(CharacterController character) {
            DroidController currentDroid = character.GetCurrentDroid();
            int currentDroidId = _droidList.IndexOf(currentDroid);
            if (currentDroidId == -1) {
                Debug.LogError($"Unable to find droid {currentDroid.name} in the droid list!");
                return;
            }
            currentDroid.SetDroidActive(false);
            DroidController newDroid = FindNextAvailableDroid(currentDroidId);
            character.SetDroid(newDroid);
            newDroid.SetDroidActive(true);
        }
    
        private DroidController FindNextAvailableDroid(int startIndex)
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

        public void AddNewDroid(DroidController droid) {
            _droidList.Add(droid);
        }
    }
}
