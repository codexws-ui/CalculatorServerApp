using ServerApp.Models;

namespace ServerApp.Services
{
    public class CalculatorService
    {
        public string Calculate(Calculator calculator)
        {
            string result = string.Empty;

            try
            {
                if (calculator.Operation == OperationType.Concatenation)
                {
                    result = calculator.ValueA + calculator.ValueB;
                }
                else if (calculator.Operation == OperationType.AddDays)
                {
                    DateTime date = DateTime.Parse(calculator.ValueA);
                    int days = int.Parse(calculator.ValueB);
                    result = date.AddDays(days).ToString("yyyy-MM-dd");
                }
                else
                {
                    double a = double.Parse(calculator.ValueA);
                    double b = double.Parse(calculator.ValueB);

                    switch (calculator.Operation)
                    {
                        case OperationType.Addition:
                            result = (a + b).ToString();
                            break;
                        case OperationType.Subtraction:
                            result = (a - b).ToString();
                            break;
                        case OperationType.Multiplication:
                            result = (a * b).ToString();
                            break;
                        case OperationType.Division:
                            result = (a / b).ToString();
                            break;
                        case OperationType.Modulus:
                            result = (a % b).ToString();
                            break;
                        case OperationType.Percent:
                            result = (a / b * 100).ToString() + "%";
                            break;
                        case OperationType.Exponentiation:
                            result = Math.Pow(a, b).ToString();
                            break;
                        case OperationType.NRoot:
                            switch (b)
                            {
                                case 2:
                                    result = Math.Sqrt(a).ToString();
                                    break;
                                case 3:
                                    result = Math.Cbrt(a).ToString();
                                    break;
                                default:
                                    result = Math.Pow(a, 1 / b).ToString();
                                    break;
                            }
                            break;
                        case OperationType.Random:
                            result = new Random().Next((int)a, (int)b).ToString();
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                result = $"ERROR - An error occurred during calculation: {ex.Message}";
            }

            return result;
        }
    }
}
