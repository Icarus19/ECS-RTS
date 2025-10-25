using Unity.Burst;
using Unity.Entities;
using Unity.VisualScripting;
using UnityEngine;

public partial struct InputTest : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<SpawnData>();
        state.RequireForUpdate<CharacterControllerInput>();
    }

    public void OnUpdate(ref SystemState state)
    {
        var input = SystemAPI.GetSingletonRW<CharacterControllerInput>();

        if (input.ValueRO.Ability0Pressed)
        {
            Debug.Log("Ability 0");
            input.ValueRW.Ability0Pressed = false;
        }
        if (input.ValueRO.Ability1Pressed)
        {
            Debug.Log("Ability 1");
            input.ValueRW.Ability1Pressed = false;
        }
        if (input.ValueRO.Ability2Pressed)
        {
            Debug.Log("Ability 2");
            input.ValueRW.Ability2Pressed = false;
        }
        if (input.ValueRO.Ability3Pressed)
        {
            Debug.Log("Ability 3");
            input.ValueRW.Ability3Pressed = false;
        }
        if (input.ValueRO.Ability4Pressed)
        {
            Debug.Log("Ability 4");
            input.ValueRW.Ability4Pressed = false;
        }
/*
        if (input.Ability1)
        {
            Debug.Log("Ability 1");
        }

        if (input.Ability2)
        {
            Debug.Log("Ability 2");
        }

        if (input.Ability3)
        {
            Debug.Log("Ability 3");
        }

        if (input.Ability4)
        {
            Debug.Log("Ability 4");
        }*/
    }

    public void OnDestroy(ref SystemState state)
    {
    }
}
public struct SpawnData : IComponentData
{
    public Entity Prefab;
}
