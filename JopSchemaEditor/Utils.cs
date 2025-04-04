using System.IO;

namespace JopSchemaEditor
{
    static class Utils
    {
        public static void Clear2DArray<T>(T[,] array)
        {
            for (int i = 0; i < array.GetLength(0); i++)
            {
                for (int j = 0; j < array.GetLength(1); j++)
                {
                    array[i, j] = default!;
                }
            }
        }

        public static unsafe ReadOnlySpan<byte> ConvertToBytes<T>(T data) where T : unmanaged
        {
            byte* pointer = (byte*)&data;
            int size = sizeof(T);

            byte[] bytes = new byte[size];
            for (int i = 0; i < size; i++)
            {
                bytes[i] = pointer[i];
            }

            return bytes;
        }

        public static unsafe T ConvertFromBytes<T>(ReadOnlySpan<byte> bytes) where T : unmanaged
        {
            if (bytes.Length != sizeof(T))
                throw new ArgumentException($"Byte array length must be {sizeof(T)}");

            fixed (byte* pointer = bytes)
            {
                return *(T*)pointer;
            }
        }

        public static void Write<T>(this BinaryWriter writer, T value) where T : unmanaged
        {
            ReadOnlySpan<byte> bytes = ConvertToBytes(value);
            writer.Write(bytes);
        }

        public static unsafe T ReadStruct<T>(this BinaryReader reader) where T : unmanaged
        {
            byte[] bytes = reader.ReadBytes(sizeof(T));
            return ConvertFromBytes<T>(bytes);
        }
    }
}
