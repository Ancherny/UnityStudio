﻿
namespace AssetStudio
{
    public class MeshFilter
    {
        public long preloadIndex;
        public PPtr m_GameObject = new PPtr();
        public PPtr m_Mesh = new PPtr();

        public MeshFilter(AssetPreloadData preloadData)
        {
            AssetsFile sourceFile = preloadData.sourceFile;
            EndianStream a_Stream = preloadData.sourceFile.a_Stream;
            a_Stream.Position = preloadData.Offset;

            if (sourceFile.platform == -2)
            {
                uint m_ObjectHideFlags = a_Stream.ReadUInt32();
                PPtr m_PrefabParentObject = sourceFile.ReadPPtr();
                PPtr m_PrefabInternal = sourceFile.ReadPPtr();
            }

            m_GameObject = sourceFile.ReadPPtr();
            m_Mesh = sourceFile.ReadPPtr();
        }
    }
}
