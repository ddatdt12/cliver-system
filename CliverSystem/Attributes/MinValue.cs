using System.ComponentModel.DataAnnotations;

namespace CliverSystem.Attributes
{
    public class IntRange : ValidationAttribute
    {
        private readonly int _minValue;
        private readonly int _maxValue;

        public IntRange(int min, int max)
        {
            _minValue = min;
            _maxValue = max;
        }

        public override bool IsValid(object? value)
        {
            if (value is not int)
            {
                return false;
            }
            return value == null || ((int)value >= _minValue && (int)value <= _maxValue);
        }
    }
}
