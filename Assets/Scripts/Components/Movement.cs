using Unity.Entities;
using Unity.Mathematics;

public struct Movement : IComponentData
{
    public float3 Target;
    public float MoveSpeed;
    public float Turnrate;
}
public struct SpatialHashCell : IComponentData
{
    public int2 Value;
}

public struct Radius : IComponentData
{
    public float Value;
}