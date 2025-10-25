using Unity.Entities;
using Unity.Mathematics;

public struct Regenable : IComponentData, IEnableableComponent {}

public struct Health : IComponentData
{
    public float Current;
    public float Max;
    public float Regen;
}

public struct DamageOverTime : IBufferElementData
{
    public float Damage;
    public float Interval;
    public float Timer;
    public DamageType Type;
    public float Duration;
}
public struct HealingOverTime : IBufferElementData
{
    public float Heal;
    public float Interval;
    public float Timer;
    public float Duration;
}
public struct Mana : IComponentData
{
    public float Current;
    public float Max;
    public float Regen;
}

public struct Shield : IComponentData
{
    public float Value;
}