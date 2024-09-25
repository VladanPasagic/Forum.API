using Forum.Core.Services.Interfaces;

namespace Forum.Core.Services;

public class RandomCodeGeneratorService : IRandomCodeGeneratorService
{
    private readonly Random random;

    public RandomCodeGeneratorService()
    {
        random = new Random();
    }

    public string GenerateRandomNumericalCode(int length)
    {
        int[] numbers = new int[length];

        for (int i = 0; i < numbers.Length; i++)
        {
            numbers[i] = random.Next(1, 10);
        }
        return string.Join("", numbers);
    }
}
