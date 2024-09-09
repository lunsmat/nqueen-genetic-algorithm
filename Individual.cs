namespace NQueenGeneticAlgorithm
{
    class Individual
    {
        private int[] Genes;

        public int Fitness => CalculateFitness();

        public int Size;

        public Individual(int[] genes)
        {
            Size = genes.Length;
            Genes = new int[Size];

            for (int i = 0; i < Size; i++)
                Genes[i] = genes[i];
        }

        public ref int[] GetGenes()
        {
            return ref Genes;
        }

        public int CalculateFitness()
        {
            var collisions = 0;

            for (int i = 0; i < Size; i++)
                collisions += GetPositionCollisions(i);

            return collisions / 2;
        }

        public void UpdateGenes(int index, int value)
        {
            Genes[index] = value;
        }

        private int GetPositionCollisions(int index)
        {
            int collisions = 0;

            for (int i = 0; i < Size; i++) {
                if (i == index) continue;
                if (Genes[index] == Genes[i]) collisions++;

                int diffHorizontally = Math.Abs(i - index);
                int diffVertically = Math.Abs(Genes[i] - Genes[index]);

                if (diffHorizontally == diffVertically) collisions++;
            }

            return collisions;
        }

        public override string ToString()
        {
            var result = String.Empty;

            for (int i = 0; i < Size; i++)
                result += $"{Genes[i]} ";

            return result;
        }
    }
}