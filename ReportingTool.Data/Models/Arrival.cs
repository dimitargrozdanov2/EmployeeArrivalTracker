#nullable disable

namespace ReportingTool.Data.Models
{
    public partial class Arrival
    {
        public int Id { get; set; }

        public int EmployeeId { get; set; }

        public string When { get; set; }
    }
}
