using Unity.Entities;

[InternalBufferCapacity((6))]
public struct AbilityBuffer : IBufferElementData
{
    public Entity Ability;
}

public struct CastAbilityRequest : IComponentData
{
    public Entity Caster;
    public Entity Ability;
}

public struct AbilityTag : IComponentData
{
    public Targeting Target;
}

public enum Targeting
{
    Active,
    Unit,
    Point,
    Vector,
    Passive
};

public struct Cooldown : IComponentData
{
    public float Value;
    public float Current;
}

public struct ManaCost : IComponentData
{
    public float Value;
}

public struct Damage : IComponentData
{
    public float Value;
}

public struct Range : IComponentData
{
    public float Value;
    public float Radius;
}

public struct Summon : IComponentData
{
    public Entity Prefab;
}