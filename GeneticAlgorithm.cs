namespace NQueenGeneticAlgorithm
{
    class GeneticAlgorithm
    {
        public GeneticAlgorithmResult Run(int size, int populationSize, double crossoverRate, double mutationRate, int maxGenerations = -1)
        {
            var result = new GeneticAlgorithmResult(size, populationSize, crossoverRate, mutationRate);

            while (maxGenerations == -1 || maxGenerations > result.GetGeneration()) {
                result.EvolvePopulation();

                if (result.BestIndividual.Fitness == 0) break;
            }

            result.FinishWatch();

            return result;
        }
    }
}