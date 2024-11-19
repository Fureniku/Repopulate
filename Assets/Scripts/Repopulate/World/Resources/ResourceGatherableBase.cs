using Repopulate.ScriptableObjects;
using Repopulate.World.Resources;
using UnityEngine;

public abstract class ResourceGatherableBase : MonoBehaviour {
    
    protected bool _canBeGathered = true;
    protected int _currentRespawnProgress;

    [SerializeField] protected bool _tickWhenItemAvailable;
    [SerializeField] protected bool _canRespawn;
    [SerializeField] protected int _respawnTicks;

    void FixedUpdate() {
        if (_canBeGathered && !_tickWhenItemAvailable) {
            return;
        }

        if (_canRespawn && !_canBeGathered) {
            _currentRespawnProgress++;
            if (_currentRespawnProgress >= _respawnTicks) {
                RespawnResource();
            }
        }
        
        TickResource();
    }

    protected abstract void OnItemClaimed(ItemStack item);
    protected abstract void OnItemRespawn();
    protected abstract void TickResource();

    protected virtual void ClaimItem() {
        _currentRespawnProgress = 0;
        _canBeGathered = false;
    }

    protected virtual void RespawnResource() {
        _canBeGathered = true;
        OnItemRespawn();
    }
}
