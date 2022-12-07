
namespace SpaceBattle
{
    public class Vector
    {
        public int[] array;
        public Vector(params int[] array)
        {
            this.array = array;   
        }

        private static bool IsSameSize(Vector first, Vector second)
        {
            return first.array.Length == second.array.Length;
        }

        public static Vector Sum (Vector first, Vector second)
        {
            if (!IsSameSize(first, second))
            {
                throw new ArgumentException("Разные размеры у векторов! Мы как их складывать-то будем???????");
            } else {
                int[] arr = new int[first.array.Length];
                for (int i = 0; i < first.array.Length; i++)
                {
                    arr[i] = first.array[i] + second.array[i];
                }
                return new Vector(arr);
            }
        }

        public static Vector operator + (Vector first, Vector second)
        {
            return Sum(first, second);
        }

        public static bool AreEquals(Vector first, Vector second)
        {
            return Enumerable.SequenceEqual(first.array, second.array);
        }

        public override bool Equals(Object? obj)
        {
            return obj is Vector v && array.SequenceEqual(v.array);
        }

        public int GetNComponent(int n)
        {
            int? nComponent = array.ElementAtOrDefault(n - 1);
            if (nComponent == null)
            {
                throw new ArgumentException("Вектор не содержит указанной компоненты");
            }
            return nComponent.Value;
        }
    }
}



