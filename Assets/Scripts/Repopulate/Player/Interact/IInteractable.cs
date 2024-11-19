namespace Repopulate.Player {
    public interface IInteractable {
    
        public void OnInteract(PlayerControllable controllable);

        public void OnLookAt(PlayerControllable controllable);
    }
}