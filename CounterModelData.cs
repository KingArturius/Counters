using SQLite;

namespace Counter
{
    public class CounterModelData
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public int Value { get; set; }
        public int InitialValue { get; set; }
        public string CounterColor { get; set; } = "transparent";
    }
}
