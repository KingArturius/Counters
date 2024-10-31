using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Maui.Controls;

namespace Counter
{
    public partial class MainPage : ContentPage
    {
        private SQLiteConnection database;
        private List<CounterModelData> counters = new();

        public MainPage()
        {
            InitializeComponent();
            InitializeDatabase();
            LoadCountersFromDatabase();
        }

        private void InitializeDatabase()
        {
            string dbPath = Path.Combine(AppContext.BaseDirectory, "../../../../../counters.db3");
            database = new SQLiteConnection(dbPath);
            database.CreateTable<CounterModelData>();
        }

        private void LoadCountersFromDatabase()
        {
            counters = database.Table<CounterModelData>().ToList();
            CountersLayout.Children.Clear();
            foreach (var counterData in counters)
                AddCounterToUI(counterData);
        }

        private void AddCounterToUI(CounterModelData counterData)
        {
            var counterView = new CounterModel(this, counterData.InitialValue, counterData.CounterColor)
            {
                Id = counterData.Id,
                Name = counterData.Name,
                Value = counterData.Value,
                InitialValue = counterData.InitialValue,
                CounterColor = counterData.CounterColor
            };

            counterView.ValueChanged += (s, e) => SaveToDatabase(counterView);
            CountersLayout.Children.Add(counterView);
        }

        private CounterModelData ConvertToDataModel(CounterModel counterView) => new()
        {
            Id = counterView.Id,
            Name = counterView.Name,
            Value = counterView.Value,
            InitialValue = counterView.InitialValue,
            CounterColor = counterView.CounterColor
        };

        public void SaveToDatabase(CounterModel counterView)
        {
            var counterData = ConvertToDataModel(counterView);
            if (counterData.Id == 0)
            {
                database.Insert(counterData);
                counterView.Id = counterData.Id;
            }
            else
            {
                database.Update(counterData);
            }
        }

        public void RemoveCounter(CounterModel counterView)
        {
            CountersLayout.Children.Remove(counterView);
            var counterData = counters.FirstOrDefault(c => c.Id == counterView.Id);
            if (counterData != null)
            {
                database.Delete(counterData);
                counters.Remove(counterData);
            }

            UpdateCounterNames();
        }

        private void UpdateCounterNames()
        {
            for (int i = 0; i < CountersLayout.Children.Count; i++)
            {
                var counterView = CountersLayout.Children[i] as CounterModel;
                if (counterView != null)
                {
                    counterView.Name = $"Counter {i + 1}";
                    SaveToDatabase(counterView);
                }
            }
        }

        private async void OnAddCounterClicked(object sender, EventArgs e)
        {
            var addCounterPage = new AddCounterPage();
            addCounterPage.CounterCreated += (s, args) =>
            {
                var counterView = new CounterModel(this, args.initialValue, args.color)
                {
                    Name = $"Counter {CountersLayout.Children.Count + 1}",
                    Value = args.initialValue,
                    InitialValue = args.initialValue,
                    CounterColor = args.color
                };

                SaveToDatabase(counterView);
                AddCounterToUI(ConvertToDataModel(counterView));
            };
            await Navigation.PushModalAsync(addCounterPage);
        }
    }
}
