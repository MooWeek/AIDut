using System.Data;

namespace aidut.Pages.Ex1;
public class Ex1Controller
{
    public List<string> Learning { get; private set; } = new ();
    public List<Tuple<int, int[]>> Table => new List<Tuple<int, int[]>>()
    {
        new(0, new[] {0, 0, 0}),
        new(0, new[] {0, 0, 1}),
        new(0, new[] {0, 1, 0}),
        new(1, new[] {0, 1, 1}),
        new(0, new[] {1, 0, 0}),
        new(1, new[] {1, 0, 1}),
        new(0, new[] {1, 1, 0}),
        new(1, new[] {1, 1, 1}),
    };

    public double[] Weights { get; set; } = new double[] {0.0, 0.0, 0.0};

    public double Sigmoid(double x)
    {
        var result = 1.0 / (1.0 + Math.Pow(Math.E, -x));
        return result;
    }

    public double SigmoidDx(double x)
    {
        var sigmoid = Sigmoid(x);
        var result = sigmoid / (1 - sigmoid);
        return result;
    }

    public void Learn(double error, double learningRate, int[] inputs)
    {
        Delta = error * SigmoidDx(Output);

        for (int i = 0; i < Weights.Length; i++)
        {
            var weight = Weights[i];
            var input = inputs[i];

            var newWeight = weight - input * Delta * learningRate;
            Weights[i] = newWeight;
        }
    }

    public double BackPropagation(double expected, int[] inputs)
    {
        var actual = CalculateValue(inputs);

        var difference = actual - expected;

        foreach (var startInputs in Table)
        {
            Learn(difference, 0.00001, startInputs.Item2);
        }

        return difference;
    }

    public double StartLearn(List<Tuple<int, int[]>> dataset, int epoch)
    {
        var error = 0.0;
        for (int i = 0; i < epoch; i++)
        {
            foreach (var data in dataset)
            {
                error = BackPropagation(data.Item1, data.Item2);
            }
            Learning.Add($"Epoch {i}, error= {error}");
        }
        
        var result = error / epoch;
        return result;
    }

    public List<double> UseNeuron(List<Tuple<int, int[]>> dataset)
    {
        var results = new List<double>();
        foreach (var data in dataset)
        {
            var res = CalculateValue(data.Item2);
            results.Add(res);
        }

        for (int i = 0; i < results.Count; i++)
        {
            var expected = Math.Round((double) dataset[i].Item1, 4);
            var actual = Math.Round(results[i], 4);
            Console.WriteLine($"actual: {actual} expected: {expected}");
        }

        return results;
    }
    
    public double UseNeuron(int[] dataset)
    {
        var res = CalculateValue(dataset);
        return res;
    }

    public double CalculateValue(int[] inputs)
    {
        var sum = (inputs[0] * Weights[0] + inputs[1] * Weights[1]) * inputs[2] * Weights[2];

        Output = Sigmoid(sum);
        return Output;
        
        return (GetWeighted(inputs[0], Weights[0]) + GetWeighted(inputs[1], Weights[1])) *
               GetWeighted(inputs[2], Weights[2]);
    }

    public double Output { get; set; }

    private double GetWeighted(int input, double weight)
    {
        return input * weight;
    }

    public double Delta { get; set; }
}