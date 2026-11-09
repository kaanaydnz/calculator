
using System.Globalization;

namespace MyFirstApp;

public partial class MainPage : ContentPage
{
    private double firstNumber = 0;
    private string currentOperator = "";
    private bool isFirstDigit = true;

    public MainPage()
    {
        InitializeComponent();
        Screen.Text = "0";
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
        firstNumber = 0;
        currentOperator = "";
        isFirstDigit = true;
    }

    private void PercentClicked(object? sender, EventArgs e)
    {
    }

    private void BackspaceClicked(object? sender, EventArgs e)
    {
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

    private void Hesapla(string op)
    {
        if (!double.TryParse(Screen.Text, NumberStyles.Float,
            CultureInfo.InvariantCulture, out double currentNumber))
        {
            Screen.Text = "Error";
            return;
        }

        if (currentOperator == "")
        {
            firstNumber = currentNumber;
        }
        else
        {
            if (op == "=" || !isFirstDigit)
            {
                double secondNumber = currentNumber;

                if (currentOperator == "/" && secondNumber == 0)
                {
                    Screen.Text = "Undefined";
                    currentOperator = "";
                    firstNumber = 0;
                    isFirstDigit = true;
                    return;
                }

                double result = currentOperator switch
                {
                    "+" => firstNumber + secondNumber,
                    "-" => firstNumber - secondNumber,
                    "*" => firstNumber * secondNumber,
                    "/" => firstNumber / secondNumber,
                    _ => firstNumber
                };

                firstNumber = result;
                Screen.Text = result.ToString(CultureInfo.InvariantCulture);
            }
        }

        currentOperator = op == "=" ? "" : op;
        isFirstDigit = true;
    }
}
