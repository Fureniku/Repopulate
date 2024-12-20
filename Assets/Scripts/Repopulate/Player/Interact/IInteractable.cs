using Repopulate.ScriptableObjects;

namespace Repopulate.Player {
    public interface IInteractable {
    
        public void OnInteract(PlayerControllable controllable, InteractLevel interactLevel);

        public void OnLookAt(PlayerControllable controllable);

        public Construct GetConstruct();
    }

    public enum InteractLevel {
        PRIMARY,
        SECONDARY,
        TERTIARY
    }
}