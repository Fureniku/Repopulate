using Repopulate.Physics.Gravity;
using Repopulate.Player;
using Repopulate.ScriptableObjects;
using UnityEngine;

namespace Repopulate.Inventory {
    public class WorldItem : MonoBehaviour, IInteractable
    {
        private GravityAffectedObject _gao;
        private int _aliveTime = 0;

        [Tooltip("How long in seconds before the object should despawn")]
        [SerializeField] private int _maxAliveTime = 300;

        [SerializeField] private Item _itemType;
        [SerializeField] private int _resourceCount;

        void Awake()
        {
            _gao = GetComponent<GravityAffectedObject>();
        }

        void FixedUpdate()
        {
            if (_aliveTime < _maxAliveTime / Time.fixedDeltaTime) {
                _aliveTime++;
            }
            else {
                Destroy(gameObject);
            }

            _gao.UpdateGravity();
        }

        public void TransferToInventory(InventoryManager inventory) {
            int overspill = inventory.InsertItem(_itemType, _resourceCount);
            if (overspill > 0) {
                _resourceCount = 0;
            }
            else {
                Debug.Log($"All resources picked up!");
                Destroy(gameObject);
            }
        }

        public void OnInteract(PlayerControllable controllable) {
            switch (controllable) {
                case DroidController droid:
                    TransferToInventory(droid.DroidInventory);
                    return;
            }
            throw new System.NotImplementedException();
        }
    }
}
