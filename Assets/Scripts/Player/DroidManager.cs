using System.Collections.Generic;
using UnityEngine;

//Overall class for all droids, regardless of controller
public class DroidManager : MonoBehaviour
{
    [SerializeField] private List<DroidController> droidList;

    private GameManager game;

    private void Start() {
        game = GameManager.Instance;
    }

    public void AssignNextAvailableDroid(CharacterController character) {
        DroidController currentDroid = character.GetCurrentDroid();
        int currentDroidId = droidList.IndexOf(currentDroid);
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
            currentIndex = (currentIndex + 1) % droidList.Count;

            if (!droidList[currentIndex].IsDroidActive) {
                return droidList[currentIndex];
            }

        } while (currentIndex != startIndex);

        Debug.LogWarning("No alternative droid found to switch to");
        return droidList[startIndex];
    }

    public void AddNewDroid(DroidController droid) {
        droidList.Add(droid);
    }
}
