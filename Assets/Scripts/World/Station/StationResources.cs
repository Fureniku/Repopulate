using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationResources : MonoBehaviour {

    [SerializeField] private StationController stationController;
    
    [Header("Oxygen")]
    [SerializeField] private List<OxygenProducer> oxygenProducers;

    [Space(10)] [Header("Energy")]
    [SerializeField] private List<EnergyProducer> energyProducers;
    [SerializeField] private List<EnergyStorage> energyStorages;
    [SerializeField] private List<EnergyConsumer> energyConsumers;

    private void OnEnable() {
        ResourceEventManager.ResourceObjectCreated += AddResourceObject;
        ResourceEventManager.ResourceObjectDestroyed += RemoveResourceObject;
    }

    private void OnDisable() {
        ResourceEventManager.ResourceObjectCreated -= AddResourceObject;
        ResourceEventManager.ResourceObjectDestroyed -= RemoveResourceObject;
    }

    private void AddResourceObject(EnumResource type, object resourceObject) {
        switch (type) {
            case EnumResource.EnergyProducer:
                energyProducers.Add((EnergyProducer)resourceObject);
                break;
            case EnumResource.EnergyStorage:
                energyStorages.Add((EnergyStorage)resourceObject);
                break;
            case EnumResource.EnergyConsumer:
                energyConsumers.Add((EnergyConsumer)resourceObject);
                break;
        }
    }

    private void RemoveResourceObject(EnumResource type, object resourceObject) {
        switch (type) {
            case EnumResource.EnergyProducer:
                energyProducers.Remove((EnergyProducer)resourceObject);
                break;
            case EnumResource.EnergyStorage:
                energyStorages.Remove((EnergyStorage)resourceObject);
                break;
            case EnumResource.EnergyConsumer:
                energyConsumers.Remove((EnergyConsumer)resourceObject);
                break;
        }
    }

    void Update() {
        UpdateEnergy();
    }

    private void UpdateEnergy() {
        int consumption = 0;
        int production = 0;
        int storage = 0;
        
        for (int i = 0; i < energyConsumers.Count; i++) {
            consumption += energyConsumers[i].GetConsumeAmount();
        }

        for (int i = 0; i < energyProducers.Count; i++) {
            production += energyProducers[i].GetProducedAmount();
        }

        int powerBalanced = production - consumption;

        if (powerBalanced > 0) { //Fill storages
            for (int i = 0; i < energyStorages.Count; i++) {
                int space = energyStorages[i].GetCapacity() - energyStorages[i].GetCurrentFIll();
                if (space >= powerBalanced) {
                    energyStorages[i].Fill(powerBalanced);
                    break;
                }
                powerBalanced -= space;
                energyStorages[i].Fill(space);
            }
        } else if (powerBalanced < 0) { //Drain storages
            int powerConsume = Math.Abs(powerBalanced); //Just to make maths and readability easier.
            for (int i = 0; i < energyStorages.Count; i++) {
                if (energyStorages[i].GetCurrentFIll() >= powerConsume) {
                    energyStorages[i].Drain(powerConsume);
                    break;
                }

                powerConsume -= energyStorages[i].GetCurrentFIll();
                energyStorages[i].Drain(energyStorages[i].GetCurrentFIll());
            }

            if (powerConsume > 0) {
                Debug.LogError("Station is out of power! Do some bad stuff here.");
            }
        }
        
        for (int i = 0; i < energyStorages.Count; i++) {
            storage += energyStorages[i].GetCurrentFIll();
        }
        
        Debug.Log($"Energy! Produced {production}, consumed {consumption} and current stored is {storage}");
    }
}
