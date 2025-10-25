using Unity.Entities;

public struct CombatLog : IComponentData { }

public struct CombatLogEntry : IBufferElementData //Currently unused
{
    public int SourceEntityIndex;
    public int TargetEntityIndex;
    public float Damage;
    public DamageType Type;
    public float Time;
}
