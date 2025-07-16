
namespace SecureSaver.src.Models
{
    public record class Base
    {
        public required string InputPath { get; init; }
        public string? OutputPath { get; init; }
        public int SaltCount { get; init; } = 500_000;
        public bool Verbose { get; init; } = false;
        public required string Operation { get; init; }
    }
}