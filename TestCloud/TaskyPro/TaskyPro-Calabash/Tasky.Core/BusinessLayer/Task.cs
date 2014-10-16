using Tasky.BL.Contracts;
using Tasky.DL.SQLite;

namespace Tasky.BL
{
    /// <summary>
    ///   Represents a Task.
    /// </summary>
    public class Task : IBusinessEntity
    {
        public string Name { get; set; }
        public string Notes { get; set; }
        // new property
        public bool Done { get; set; }
        [PrimaryKey]
        [AutoIncrement]
        public int ID { get; set; }
    }
}
