namespace Forum.Core.Services.Interfaces;

public interface IRandomCodeGeneratorService
{
    string GenerateRandomNumericalCode(int length);
}
