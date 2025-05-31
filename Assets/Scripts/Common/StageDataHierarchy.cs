using System;

[Serializable]
public class StageItemDataHierarchy<T>
{
    public SpawnItemDataGroup<T>[] DataBases = new SpawnItemDataGroup<T>[2];
}
[Serializable]
public class SpawnItemDataGroup<T>
{
    public SpawnItemDataBlock<T>[] DataBlocks = new SpawnItemDataBlock<T>[5];
}
[Serializable]
public class SpawnItemDataBlock<T>
{
    public T[] Cell = new T[3];
}
