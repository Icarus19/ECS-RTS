using Unity.Entities;

public struct Modifier : IComponentData
{
    public float Duration; //Negative values will be considered as infinite duration
}

public struct ModifierTarget : IBufferElementData
{
    public Entity Target;
}

public struct Buff : IComponentData { }
public struct Debuff : IComponentData { }

public struct AmplifyOutgoingDamage : IComponentData
{
    public float Value;
    public DamageType Type;
}
public struct AmplifyIncomingDamage : IComponentData
{
    public float Value;
    public DamageType Type;
}

public struct AmplifySpeed : IComponentData
{
    public float Value;
}

public struct AmplifySize : IComponentData
{
    public float Value;
}

public struct AmplifyRegen : IComponentData
{
    public float TickInterval;
    public float TimeSinceLastTick;
    public float TickValue;
    public float DamageType;
}