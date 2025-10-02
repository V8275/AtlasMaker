namespace ClassLibrary1
{
    public class MathOperations
    {
        public int Add(int a, int b)
        {
            return a + b;
        }

        public int Subtract(int a, int b)
        {
            return a - b;
        }

        public int Multiply(int a, int b)
        {
            return a * b;
        }

        public double Divide(int a, int b)
        {
            if (b == 0)
                throw new DivideByZeroException("Деление на ноль невозможно.");
            return (double)a / b;
        }

        internal bool IsEven(int number)
        {
            return number % 2 == 0;
        }

        internal double CalculateSquareRoot(double number)
        {
            if (number < 0)
                throw new ArgumentOutOfRangeException("Число должно быть неотрицательным.");
            return Math.Sqrt(number);
        }

        public double CalculateAverage(int[] numbers)
        {
            if (numbers == null || numbers.Length == 0)
                throw new ArgumentException("Массив не может быть пустым.");
            double sum = 0;
            foreach (var number in numbers)
            {
                sum += number;
            }
            return sum / numbers.Length;
        }

        public bool CheckIfEven(int number)
        {
            return IsEven(number); 
        }

        public double GetSquareRoot(double number)
        {
            return CalculateSquareRoot(number);
        }
    }
}
