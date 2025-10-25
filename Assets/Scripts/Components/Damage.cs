using Unity.Collections;
using Unity.Entities;

/*
public struct DamageEvent : IBufferElementData
{
    public float Value;
    public DamageType Type;
    public NativeStream Stream;
}
*/
public enum DamageType
{
    Physical,
    Magical,
    Holy,
    Fire,
    Ice,
    Poison
};

public struct Target : IComponentData
{
    public Entity Value;
}

public struct MeleeAttack : IComponentData
{
    public float Damage;
    public DamageType Type;
}
public struct Armor : IComponentData
{
    public float Amplification;
    public float PhysicalResistance;
    public float MagicalResistance;
    public float HolyResistance;
    public float FireResistance;
    public float IceResistance;
    public float PoisonResistance;
}

//[WriteGroup(typeof(Armor))] I guess the writegroup is unnecessary since we update duration every frame, but we only apply the effect during damage events
public struct ArmorModifier : IBufferElementData
{
    public float Amplification;
    public float PhysicalResistance;
    public float MagicalResistance;
    public float HolyResistance;
    public float FireResistance;
    public float IceResistance;
    public float PoisonResistance;
    public float Duration;
}