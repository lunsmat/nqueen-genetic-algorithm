using System.Diagnostics;

namespace NQueenGeneticAlgorithm
{
    class GeneticAlgorithmResult
    {
        private int Size;
        
        private int PopulationSize;

        private double CrossoverRate;

        private double MutationRate;

        private int Generation = 0;

        private Individual[] Population;

        private Stopwatch Watch = Stopwatch.StartNew();

        public Individual BestIndividual => Population.OrderBy(individual => individual.Fitness).First();

        public GeneticAlgorithmResult(int size, int populationSize, double crossoverRate, double mutationRate)
        {
            Size = size;
            PopulationSize = populationSize;
            CrossoverRate = crossoverRate;
            MutationRate = mutationRate;
            Population = new Individual[PopulationSize];

            InitializePopulation();
        }

        private void InitializePopulation()
        {
            for (int i = 0; i < PopulationSize; i++) {
                var genes = Enumerable.Range(0, Size).OrderBy(x => new Random().Next()).ToArray();
                Population[i] = new Individual(genes);
            }
        }

        public void EvolvePopulation()
        {
            var newPopulation = new Individual[PopulationSize];

            var bestIndividual = Population.OrderBy(individual => individual.Fitness).First();
            newPopulation[0] = bestIndividual;

            for (int i = 1; i < PopulationSize; i += 2) {
                var father = SelectParent();
                var mother = SelectParent();

                var (first, second) = Crossover(father, mother);

                Mutate(ref first);
                Mutate(ref second);

                newPopulation[i] = first;

                if (i + 1 >= PopulationSize) break;

                newPopulation[i + 1] = second;
            }

            Population = newPopulation;

            Generation++;
        }

        public void FinishWatch()
        {
            Watch.Stop();
        }

        public long GetWatchMilliSeconds()
        {
            return Watch.ElapsedMilliseconds;
        }

        private Individual SelectParent()
        {
            double totalFitness = Population.Sum(individual => 1.0 / (individual.Fitness + 1));
            double randomValue = new Random().NextDouble() * totalFitness;

            double cumulativeFitness = 0;

            foreach (var individual in Population)
            {
                cumulativeFitness += 1.0 / (individual.Fitness + 1);

                if (cumulativeFitness >= randomValue) return individual;
            }

            return Population.Last();
        }

        private (Individual, Individual) Crossover(Individual father, Individual mother)
        {
            if (new Random().NextDouble() > CrossoverRate)
                return (new Individual(father.GetGenes()), new Individual(mother.GetGenes()));

            int crossoverPoint = new Random().Next(Size);
            var firstGenes = father.GetGenes().Take(crossoverPoint).Concat(mother.GetGenes().Skip(crossoverPoint)).ToArray();
            var secondGenes = father.GetGenes().Take(crossoverPoint).Concat(mother.GetGenes().Skip(crossoverPoint)).ToArray();

            return (new Individual(firstGenes), new Individual(secondGenes));
        }

        private void Mutate(ref Individual individual)
        {
            if (new Random().NextDouble() < MutationRate) {
                int bitToFlip = new Random().Next(3); // Only works with population 8
                int index = new Random().Next(Size);

                int gene = individual.GetGenes().ElementAt(index);

                gene ^= (1 << bitToFlip);
                individual.UpdateGenes(index, (int) gene);
            }
        }

        public int GetGeneration()
        {
            return Generation;
        }

        public override string ToString()
        {
            var result = String.Empty;

            result += $"Tamanho População: {PopulationSize}\n";
            result += $"Taxa de cruzamento: {CrossoverRate}\n";
            result += $"Taxa de Mutação: {MutationRate}\n";
            result += $"Gerações: {Generation}\n";
            result += $"Tempo: {Watch.ElapsedMilliseconds} ms\n";
            var optimal = BestIndividual.Fitness == 0 ? "Alcançada" : "Não Alcançada";
            result += $"Solução Ótima: {optimal}\n";
            result += $"Genes Melhor Indivíduo: {BestIndividual}\n";

            return result;
        }
    }
}