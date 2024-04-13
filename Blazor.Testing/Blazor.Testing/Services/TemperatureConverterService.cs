namespace Blazor.Testing.Services;

public class TemperatureConverterService : ITemperatureConverterService
{
    public int ConvertTemperatureToF(int celsiusTemp) => 32 + (int)(celsiusTemp / 0.5556);
}