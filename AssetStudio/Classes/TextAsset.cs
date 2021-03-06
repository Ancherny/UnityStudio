﻿
namespace AssetStudio
{
    class TextAsset
    {
        public string m_Name;
        public byte[] m_Script;
        public string m_PathName;

        public TextAsset(AssetPreloadData preloadData, bool readSwitch)
        {
            AssetsFile sourceFile = preloadData.sourceFile;
            EndianStream a_Stream = preloadData.sourceFile.a_Stream;
            a_Stream.Position = preloadData.Offset;
            preloadData.extension = ".txt";

            if (sourceFile.platform == -2)
            {
                uint m_ObjectHideFlags = a_Stream.ReadUInt32();
                PPtr m_PrefabParentObject = sourceFile.ReadPPtr();
                PPtr m_PrefabInternal = sourceFile.ReadPPtr();
            }

            m_Name = a_Stream.ReadAlignedString(a_Stream.ReadInt32());

            int m_Script_size = a_Stream.ReadInt32();

            if (readSwitch) //asset is read for preview or export
            {
                m_Script = new byte[m_Script_size];
                a_Stream.Read(m_Script, 0, m_Script_size);

                if (m_Script[0] == 93) { m_Script = SevenZip.Compression.LZMA.SevenZipHelper.Decompress(m_Script); }
                if (m_Script[0] == 60 || (m_Script[0] == 239 && m_Script[1] == 187 && m_Script[2] == 191 && m_Script[3] == 60)) { preloadData.extension = ".xml"; }
            }
            else
            {
                byte lzmaTest = a_Stream.ReadByte();
                if (lzmaTest == 93)
                {
                    a_Stream.Position += 4;
                    preloadData.exportSize = a_Stream.ReadInt32(); //actualy int64
                    a_Stream.Position -= 8;
                }
                else { preloadData.exportSize = m_Script_size; }

                a_Stream.Position += m_Script_size - 1;

                if (m_Name != "") { preloadData.Text = m_Name; }
                else { preloadData.Text = preloadData.TypeString + " #" + preloadData.uniqueID; }
                preloadData.SubItems.AddRange(new string[] { preloadData.TypeString, preloadData.exportSize.ToString() });
            }
            a_Stream.AlignStream(4);

            m_PathName = a_Stream.ReadAlignedString(a_Stream.ReadInt32());
        }
    }
}
