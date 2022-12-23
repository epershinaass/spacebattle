
namespace SpaceBattle
{
    public class Vector
    {
        private int[] array;
        public Vector(params int[] array)
        {
            this.array = array; 
        }

        public static bool IsSameSize(Vector first, Vector second)
        {
            return first.array.Length == second.array.Length;
        }


        public static Vector operator + (Vector first, Vector second){
                if (first.array.Length != second.array.Length)
                throw new System.ArgumentException();
            else{
                int[] arr = new int[first.array.Length];
                for (int i = 0; i < first.array.Length; i++)
                {
                    arr[i] = first.array[i] + second.array[i];
                }
                return new Vector(arr);
            }
        }

        public static Vector operator - (Vector first, Vector second){
        if (first.array.Length != second.array.Length)
            throw new System.ArgumentException();
        else{
            int[] sum = new int[first.array.Length];
            for (int i = 0; i < first.array.Length; i++)
                sum[i] = first.array[i] - second.array[i];
            return new Vector(sum);
        }
        }

        public static bool operator == (Vector first, Vector second){
        if (first.array.Length != second.array.Length)
            return false;
        bool f = true;
        for (int i = 0; i < first.array.Length; i++)
            if (first.array[i] != second.array[i]) f = false;
        return f;
        }
        public static bool operator != (Vector first, Vector second){
            return !(first==second);
        }

        public override bool Equals(Object? obj)
        {
            return obj is Vector v && array.SequenceEqual(v.array);
        }

        public override int GetHashCode() {
        HashCode hash = new();
        foreach(int i in array){
            hash.Add(array[i]);
        }
        return hash.ToHashCode();

    }
    }
}



