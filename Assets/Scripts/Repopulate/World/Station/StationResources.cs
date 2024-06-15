using System;
using System.Collections.Generic;
using Repopulate.World.Utilities;
using UnityEngine;

namespace Repopulate.World.Station {
    public class StationResources : MonoBehaviour {
    
        [Header("Oxygen")]
        [SerializeField] private List<OxygenProducer> _oxygenProducers;

        [Space(10)] [Header("Energy")]
        [SerializeField] private List<EnergyProducer> _energyProducers;
        [SerializeField] private List<EnergyStorage> _energyStorages;
        [SerializeField] private List<EnergyConsumer> _energyConsumers;

        private void OnEnable() {
            UtilityEventManager.ResourceObjectCreated += AddResourceObject;
            UtilityEventManager.ResourceObjectDestroyed += RemoveResourceObject;
        }

        private void OnDisable() {
            UtilityEventManager.ResourceObjectCreated -= AddResourceObject;
            UtilityEventManager.ResourceObjectDestroyed -= RemoveResourceObject;
        }

        private void AddResourceObject(EnumUtility type, object resourceObject) {
            switch (type) {
                case EnumUtility.EnergyProducer:
                    _energyProducers.Add((EnergyProducer)resourceObject);
                    break;
                case EnumUtility.EnergyStorage:
                    _energyStorages.Add((EnergyStorage)resourceObject);
                    break;
                case EnumUtility.EnergyConsumer:
                    _energyConsumers.Add((EnergyConsumer)resourceObject);
                    break;
            }
        }

        private void RemoveResourceObject(EnumUtility type, object resourceObject) {
            switch (type) {
                case EnumUtility.EnergyProducer:
                    _energyProducers.Remove((EnergyProducer)resourceObject);
                    break;
                case EnumUtility.EnergyStorage:
                    _energyStorages.Remove((EnergyStorage)resourceObject);
                    break;
                case EnumUtility.EnergyConsumer:
                    _energyConsumers.Remove((EnergyConsumer)resourceObject);
                    break;
            }
        }

        void Update() {
            UpdateEnergy();
        }

        //TODO this will be trash for performance. Can we find another way? maybe DOTS?
        private void UpdateEnergy() {
            int consumption = 0;
            int production = 0;
            int storage = 0;
        
            for (int i = 0; i < _energyConsumers.Count; i++) {
                consumption += _energyConsumers[i].GetConsumeAmount();
            }

            for (int i = 0; i < _energyProducers.Count; i++) {
                production += _energyProducers[i].GetProducedAmount();
            }

            int powerBalanced = production - consumption;

            if (powerBalanced > 0) { //Fill storages
                for (int i = 0; i < _energyStorages.Count; i++) {
                    int space = _energyStorages[i].GetCapacity() - _energyStorages[i].GetCurrentFIll();
                    if (space >= powerBalanced) {
                        _energyStorages[i].Fill(powerBalanced);
                        break;
                    }
                    powerBalanced -= space;
                    _energyStorages[i].Fill(space);
                }
            } else if (powerBalanced < 0) { //Drain storages
                int powerConsume = Math.Abs(powerBalanced); //Just to make maths and readability easier.
                for (int i = 0; i < _energyStorages.Count; i++) {
                    if (_energyStorages[i].GetCurrentFIll() >= powerConsume) {
                        _energyStorages[i].Drain(powerConsume);
                        break;
                    }

                    powerConsume -= _energyStorages[i].GetCurrentFIll();
                    _energyStorages[i].Drain(_energyStorages[i].GetCurrentFIll());
                }

                if (powerConsume > 0) {
                    Debug.LogError("Station is out of power! Do some bad stuff here.");
                }
            }
        
            for (int i = 0; i < _energyStorages.Count; i++) {
                storage += _energyStorages[i].GetCurrentFIll();
            }
        
            //Debug.Log($"Energy! Produced {production}, consumed {consumption} and current stored is {storage}");
        }
    }
}
