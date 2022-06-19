using System;

namespace CleanCodeTask2
{
    class CleanCode_ExampleTask2
    {
        public static int TryGetElementIndex(int[] array, int element)
        {
            for (int i = 0; i < array.Length; i++)
                if (array[i] == element)
                    return i;

            return -1;
        }
    }
}
