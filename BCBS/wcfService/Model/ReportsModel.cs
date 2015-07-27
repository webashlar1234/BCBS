using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wcfService.Model
{
    class ReportsModel
    {
    }

    public class MonthYear
    {
        public string Month { get; set; }
        public string Year { get; set; }
    }
    public class Balance
    {
        public string Avialablebalance { get; set; }
    }
    public class PlanRevenueExpense
    {
        public string ProjectCode { get; set; }
        public string Revenue { get; set; }
        public string Expense { get; set; }
    }
    public class AccuralReport
    {
        public string ProjectCode { get; set; }
        public string CustomerType { get; set; }
        public string ServiceName { get; set; }
        public string Revenue { get; set; }
        public string Expense { get; set; }
        public string Estimate { get; set; }
        public string FromDate { get; set; }
        public string CustomerName { get; set; }
    }
}
