using Microsoft.Maui.Controls;
using System;

namespace Counter
{
    public partial class CounterModel : ContentView
    {
        private int _value;
        private int _initialValue;
        private string _color;
        private MainPage _mainPage;

        public int Id { get; set; }

        public string Name
        {
            get => NameLabel.Text;
            set => NameLabel.Text = value;
        }

        public int Value
        {
            get => _value;
            set
            {
                if (_value != value)
                {
                    _value = value;
                    ValueLabel.Text = _value.ToString();
                    ValueChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        public int InitialValue
        {
            get => _initialValue;
            set => _initialValue = value;
        }

        public string CounterColor
        {
            get => _color;
            set
            {
                _color = value;
                UpdateColor();
            }
        }

        public event EventHandler ValueChanged;

        public CounterModel(MainPage mainPage, int initialValue, string color)
        {
            InitializeComponent();
            MinusButton.Clicked += OnMinusClicked;
            PlusButton.Clicked += OnPlusClicked;
            ResetButton.Clicked += OnResetClicked;
            DeleteButton.Clicked += OnDeleteClicked;
            _mainPage = mainPage;
            _initialValue = initialValue;
            Value = initialValue;
            CounterColor = color;
        }

        private void OnPlusClicked(object sender, EventArgs e)
        {
            Value++;
        }

        private void OnMinusClicked(object sender, EventArgs e)
        {
            Value--;
        }

        private void OnResetClicked(object sender, EventArgs e)
        {
            Value = _initialValue;
        }

        private void OnDeleteClicked(object sender, EventArgs e)
        {
            _mainPage.RemoveCounter(this);
        }

        private void UpdateColor()
        {
            switch (_color.ToLower())
            {
                case "red":
                    CounterLayout.BackgroundColor = Colors.Red;
                    break;
                case "green":
                    CounterLayout.BackgroundColor = Colors.Green;
                    break;
                case "blue":
                    CounterLayout.BackgroundColor = Colors.Aqua;
                    break;
                case "yellow":
                    CounterLayout.BackgroundColor = Colors.Yellow;
                    break;
                case "orange":
                    CounterLayout.BackgroundColor = Colors.Orange;
                    break;
                default:
                    CounterLayout.BackgroundColor = Colors.Transparent;
                    break;
            }
        }
    }
}
