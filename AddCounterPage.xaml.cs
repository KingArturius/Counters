using System;
using Microsoft.Maui.Controls;

namespace Counter
{
    public partial class AddCounterPage : ContentPage
    {
        public event EventHandler<(int initialValue, string color)> CounterCreated;

        public AddCounterPage()
        {
            InitializeComponent();
        }

        private void OnOKClicked(object sender, EventArgs e)
        {
            if (int.TryParse(InitialValueEntry.Text, out int initialValue) && ColorPicker.SelectedItem != null)
            {
                string selectedColor = ColorPicker.SelectedItem.ToString();
                CounterCreated?.Invoke(this, (initialValue, selectedColor));
                Navigation.PopModalAsync();
            }
        }

        private void OnCancelClicked(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }
    }
}
