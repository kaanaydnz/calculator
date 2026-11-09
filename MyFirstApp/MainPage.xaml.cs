
using System.Globalization;

namespace MyFirstApp;

public partial class MainPage : ContentPage
{
    private double firstNumber = 0;
    private string currentOperator = "";
    private bool isFirstDigit = true;
    private string operationHistory = "";

    public MainPage()
    {
        InitializeComponent();
        Screen.Text = "0";
        OperationHistory.Text = "";
    }

    private void DigitClicked(object? sender, EventArgs e)
    {
        Button? digit = sender as Button;

        if (digit == null)
            return;

        string value = digit.CommandParameter?.ToString() ?? "";

        if (isFirstDigit)
        {
            Screen.Text = value;
            isFirstDigit = false;
        }
        else
        {
            Screen.Text += value;
        }
    }

    private void DecimalClicked(object? sender, EventArgs e)
    {
        if (isFirstDigit)
        {
            Screen.Text = "0.";
            isFirstDigit = false;
        }
        else if (!Screen.Text.Contains("."))
        {
            Screen.Text += ".";
        }
    }

    private void ClearClicked(object? sender, EventArgs e)
    {
        Screen.Text = "0";
        OperationHistory.Text = "";
        operationHistory = "";
        firstNumber = 0;
        currentOperator = "";
        isFirstDigit = true;
    }

    private void PercentClicked(object? sender, EventArgs e)
    {
        if (double.TryParse(Screen.Text, NumberStyles.Float,
            CultureInfo.InvariantCulture, out double number))
        {
            number /= 100;
            Screen.Text = number.ToString(CultureInfo.InvariantCulture);
            isFirstDigit = false;
        }
    }

    private void BackspaceClicked(object? sender, EventArgs e)
    {
        if (Screen.Text == "Error" || Screen.Text == "Undefined")
        {
            Screen.Text = "0";
            isFirstDigit = true;
            return;
        }

        if (isFirstDigit)
            return;

        if (Screen.Text.Length > 1)
        {
            Screen.Text = Screen.Text.Substring(0, Screen.Text.Length - 1);

            if (Screen.Text == "-")
            {
                Screen.Text = "0";
                isFirstDigit = true;
            }
        }
        else
        {
            Screen.Text = "0";
            isFirstDigit = true;
        }
    }

    private void EqualOperator_OnClicked(object? sender, EventArgs e)
    {
        Hesapla("=");
    }

    private void SumOperator_OnClicked(object? sender, EventArgs e)
    {
        Hesapla("+");
    }

    private void MinusOperator_OnClicked(object? sender, EventArgs e)
    {
        Hesapla("-");
    }

    private void MultiplyOperator_OnClicked(object? sender, EventArgs e)
    {
        Hesapla("*");
    }

    private void DivideOperator_OnClicked(object? sender, EventArgs e)
    {
        Hesapla("/");
    }

    private double CalculateResult(double first, double second)
    {
        return currentOperator switch
        {
            "+" => first + second,
            "-" => first - second,
            "*" => first * second,
            "/" => first / second,
            _ => first
        };
    }

    private double lastSecondNumber = 0;
    private string lastOperator = "";

    private void Hesapla(string op)
    {
        if (!double.TryParse(Screen.Text, NumberStyles.Float,
            CultureInfo.InvariantCulture, out double currentNumber))
        {
            Screen.Text = "Error";
            return;
        }

        if (op == "=")
        {
            if (currentOperator != "" && !isFirstDigit)
            {
                lastSecondNumber = currentNumber;
                lastOperator = currentOperator;
            }
            else if (currentOperator == "" && lastOperator != "")
            {
                currentOperator = lastOperator;
                currentNumber = lastSecondNumber;
            }
            else
            {
                return;
            }

            if (currentOperator == "/" && currentNumber == 0)
            {
                Screen.Text = "Undefined";
                currentOperator = "";
                lastOperator = "";
                firstNumber = 0;
                isFirstDigit = true;
                operationHistory = "";
                OperationHistory.Text = "";
                return;
            }

            string operatorSymbol = currentOperator switch
            {
                "*" => "×",
                "/" => "÷",
                _ => currentOperator
            };

            double result = CalculateResult(firstNumber, currentNumber);

            OperationHistory.Text =
                firstNumber.ToString(CultureInfo.InvariantCulture)
                + " " + operatorSymbol + " "
                + currentNumber.ToString(CultureInfo.InvariantCulture)
                + " =";

            Screen.Text = result.ToString(CultureInfo.InvariantCulture);
            firstNumber = result;
            currentOperator = "";
            isFirstDigit = true;

            return;
        }

        lastOperator = "";
        lastSecondNumber = 0;

        if (currentOperator != "" && !isFirstDigit)
        {
            if (currentOperator == "/" && currentNumber == 0)
            {
                Screen.Text = "Undefined";
                currentOperator = "";
                firstNumber = 0;
                isFirstDigit = true;
                operationHistory = "";
                OperationHistory.Text = "";
                return;
            }

            firstNumber = CalculateResult(firstNumber, currentNumber);
            Screen.Text = firstNumber.ToString(CultureInfo.InvariantCulture);
        }
        else if (currentOperator == "")
        {
            firstNumber = currentNumber;
        }

        currentOperator = op;

        string symbol = op switch
        {
            "*" => "×",
            "/" => "÷",
            _ => op
        };

        operationHistory =
            firstNumber.ToString(CultureInfo.InvariantCulture)
            + " " + symbol;

        OperationHistory.Text = operationHistory;
        isFirstDigit = true;
    }
}