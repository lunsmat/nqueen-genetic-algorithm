namespace NQueenGeneticAlgorithm
{
    class Program
    {
        private const int Size = 8;

        private const int PopulationSize = 20;
        
        private const double CrossoverRate = 0.8;
        
        private const double MutationRate = 0.03;
        
        private const int GenerationsLimit = 1000;

        public static void Main()
        {
            var algorithm = new GeneticAlgorithm();
            var results = new List<GeneticAlgorithmResult>();

            const int iterations = 50;

            for (int i = 0; i < iterations; i++) {
                var result = algorithm.Run(Size, PopulationSize, CrossoverRate, MutationRate, GenerationsLimit);

                results.Add(result);
            }

            var timeList = from result in results select result.GetWatchMilliSeconds();
            var generationList = from result in results select (long) result.GetGeneration();

            var timeAverage = timeList.Average();
            var generationAverage = generationList.Average();

            var timeStandardDeviation = StandardDeviation(timeList);
            var generationStandardDeviation = StandardDeviation(generationList);

            var minorTime = timeList.Min();
            var minorGenerations = generationList.Min();

            var fiveBestSolutions = results
                .OrderBy(result => result.BestIndividual.Fitness)
                .OrderBy(result => result.GetGeneration())
                .Take(5);

            Console.WriteLine($"======================================= Resultados =======================================");
            Console.WriteLine($"Media de Tempo: {timeAverage} ms");
            Console.WriteLine($"Media de Gerações: {generationAverage} Gerações");
            Console.WriteLine($"Desvio Padrão de Tempo: {timeStandardDeviation} ms");
            Console.WriteLine($"Desvio Padrão de Gerações: {generationStandardDeviation} Gerações");
            Console.WriteLine($"Menor Tempo: {minorTime} ms");
            Console.WriteLine($"Menor Número de Gerações: {minorGenerations} Gerações");

            Console.WriteLine();
            Console.WriteLine("5 Melhores resultados");

            foreach (var result in fiveBestSolutions)
            {
                Console.WriteLine();
                Console.WriteLine(result);
            }
        }

        public static double StandardDeviation(IEnumerable<long> values)
        {
            double ret = 0;
            int count = values.Count();
            double avg = values.Average();
            double sum = values.Sum(d => (d - avg) * (d - avg));
            ret = Math.Sqrt(sum / count);
            
            return ret;
        }
    }
}
