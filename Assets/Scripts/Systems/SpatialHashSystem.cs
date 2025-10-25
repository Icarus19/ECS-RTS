using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

[BurstCompile]
public partial class SpatialHashSystem : SystemBase
{
    public NativeParallelMultiHashMap<int2, Entity> HashMap; 
    float CellSize;
    
    protected override void OnCreate()
    {
        HashMap = new NativeParallelMultiHashMap<int2, Entity>(capacity: 4096, Allocator.Persistent);
    }

    protected override void OnDestroy()
    {
        if (HashMap.IsCreated)
            HashMap.Dispose();
    }

    protected override void OnUpdate()
    {
        HashMap.Clear();

        var hashMap = HashMap;
        float cellSize = CellSize;
        
        var hashMapWriter = hashMap.AsParallelWriter();

        Entities
            .WithName("RebuildSpatialHash")
            .ForEach((Entity entity, ref SpatialHashCell cell, in LocalTransform transform) =>
            {
                int2 newCell = new int2(
                    (int)math.floor(transform.Position.x / cellSize),
                    (int)math.floor(transform.Position.y / cellSize)
                );

                // Update cached cell in component
                cell.Value = newCell;

                // Add entity to the hash map safely in parallel
                hashMapWriter.Add(newCell, entity);

            }).ScheduleParallel();
    }
    
    public int2 GetCell(float3 position)
    {
        return new int2(
            (int)math.floor(position.x / CellSize),
            (int)math.floor(position.y / CellSize)
        );
    }
}
public static class SpatialHashUtility
{
    // All 9 neighboring offsets including the center cell
    static readonly int2[] NeighborOffsets = new int2[]
    {
        new int2(-1, -1), new int2(0, -1), new int2(1, -1),
        new int2(-1,  0), new int2(0,  0), new int2(1,  0),
        new int2(-1,  1), new int2(0,  1), new int2(1,  1)
    };

    // Get entities in nearby cells
    public static void QueryNeighbors(
        NativeParallelMultiHashMap<int2, Entity> hashMap,
        int2 centerCell,
        NativeList<Entity> results,
        Allocator allocator)
    {
        results.Clear();

        foreach (var offset in NeighborOffsets)
        {
            int2 cell = centerCell + offset;

            NativeParallelMultiHashMapIterator<int2> it;
            Entity neighbor;

            if (hashMap.TryGetFirstValue(cell, out neighbor, out it))
            {
                do
                {
                    results.Add(neighbor);
                }
                while (hashMap.TryGetNextValue(out neighbor, ref it));
            }
        }
    }
}
