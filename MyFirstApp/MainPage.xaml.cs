
using System.Globalization;

namespace MyFirstApp;

public partial class MainPage : ContentPage
{
    private double firstNumber = 0;
    private string currentOperator = "";
    private bool isFirstDigit = true;
    private string operationHistory = "";

    private double lastSecondNumber = 0;
    private string lastOperator = "";

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

        if (Screen.Text == "Error" || Screen.Text == "Undefined")
            ClearClicked(sender, e);

        if (isFirstDigit)
        {
            if (currentOperator == "")
            {
                firstNumber = 0;
                lastOperator = "";
                lastSecondNumber = 0;
                operationHistory = "";
                OperationHistory.Text = "";
            }

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
        if (Screen.Text == "Error" || Screen.Text == "Undefined")
            ClearClicked(sender, e);

        if (isFirstDigit)
        {
            if (currentOperator == "")
            {
                firstNumber = 0;
                lastOperator = "";
                lastSecondNumber = 0;
                operationHistory = "";
                OperationHistory.Text = "";
            }

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
        lastSecondNumber = 0;
        lastOperator = "";

        isFirstDigit = true;
    }

    private void PercentClicked(object? sender, EventArgs e)
    {
        if (isFirstDigit)
            return;

        if (double.TryParse(Screen.Text, NumberStyles.Float,
            CultureInfo.InvariantCulture, out double number))
        {
            number = Math.Round(number / 100, 10);

            Screen.Text = number.ToString("G10", CultureInfo.InvariantCulture);

            lastOperator = "";
            lastSecondNumber = 0;

            isFirstDigit = true;
        }
    }

    private void BackspaceClicked(object? sender, EventArgs e)
    {
        if (Screen.Text == "Error" || Screen.Text == "Undefined")
        {
            ClearClicked(sender, e);
            return;
        }

        if (isFirstDigit)
            return;

        if (Screen.Text.Length > 1)
        {
            Screen.Text = Screen.Text.Substring(0, Screen.Text.Length - 1);

            if (Screen.Text == "0")
            {
                isFirstDigit = true;
            }

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
        double result = currentOperator switch
        {
            "+" => first + second,
            "-" => first - second,
            "*" => first * second,
            "/" => first / second,
            _ => first
        };

        return Math.Round(result, 10);
    }

    private void ShowError(string message)
    {
        Screen.Text = message;
        OperationHistory.Text = "";

        firstNumber = 0;
        currentOperator = "";
        lastSecondNumber = 0;
        lastOperator = "";
        operationHistory = "";

        isFirstDigit = true;
    }

    private void Hesapla(string op)
    {
        if (!double.TryParse(Screen.Text, NumberStyles.Float,
            CultureInfo.InvariantCulture, out double currentNumber))
        {
            ShowError("Error");
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
                ShowError("Undefined");
                return;
            }

            string operatorSymbol = currentOperator switch
            {
                "*" => "×",
                "/" => "÷",
                _ => currentOperator
            };

            double result = CalculateResult(firstNumber, currentNumber);

            if (!double.IsFinite(result))
            {
                ShowError("Error");
                return;
            }

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

        if (currentOperator == "" && isFirstDigit && lastOperator == "")
            return;

        if (currentOperator != "" && isFirstDigit)
        {
            currentOperator = op;

            string newSymbol = op switch
            {
                "*" => "×",
                "/" => "÷",
                _ => op
            };

            operationHistory =
                firstNumber.ToString(CultureInfo.InvariantCulture)
                + " " + newSymbol;

            OperationHistory.Text = operationHistory;

            return;
        }

        lastOperator = "";
        lastSecondNumber = 0;

        if (currentOperator != "")
        {
            if (currentOperator == "/" && currentNumber == 0)
            {
                ShowError("Undefined");
                return;
            }

            firstNumber = CalculateResult(firstNumber, currentNumber);

            if (!double.IsFinite(firstNumber))
            {
                ShowError("Error");
                return;
            }

            Screen.Text = firstNumber.ToString(CultureInfo.InvariantCulture);
        }
        else
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
