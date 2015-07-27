using BCBS.bsbcserviceref;
using BCBS.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BCBS.Controllers
{
    public class ReportsController : Controller
    {
        //
        // GET: /Reports/
        public ActionResult AccuralWorkSheet()
        {
            ViewBag.currentMonth = DateTime.Now.Month.ToString();
            ViewBag.currentYear = DateTime.Now.Year.ToString();
            ViewBag.Months = new List<SelectListItem> {
                new SelectListItem { Text = "January", Value = "1" },
                new SelectListItem { Text = "February", Value = "2" },
                new SelectListItem { Text = "March", Value = "3" }, 
                new SelectListItem{Text="April",Value="4"},
                new SelectListItem{Text="May",Value="5"},
                new SelectListItem{Text="June",Value="6"},
                new SelectListItem{Text="July",Value="7"},
                new SelectListItem{Text="August",Value="8"},
                new SelectListItem{Text="September",Value="9"},
                new SelectListItem{Text="October",Value="10"},
                new SelectListItem{Text="November",Value="11"},
                new SelectListItem{Text="December",Value="12"}
            };
            BCBSClient client = new BCBSClient();
            string years = client.GetAccuralReportMonthYear();
            List<MonthYear> ContractList = new List<MonthYear>();
            if (!string.IsNullOrEmpty(years))
            {
                ContractList = JsonConvert.DeserializeObject<List<MonthYear>>(years);
                int currentexist = ContractList.Where(x => x.Year == DateTime.Now.Year.ToString()).Count();
                if (!(currentexist > 0))
                {
                    MonthYear monthYear = new MonthYear();
                    monthYear.Year = DateTime.Now.Year.ToString();
                    monthYear.Month = "";
                    ContractList.Add(monthYear);
                }
            }
            else
            {
                ContractList = JsonConvert.DeserializeObject<List<MonthYear>>(years);
                MonthYear monthYear = new MonthYear();
                monthYear.Year = DateTime.Now.Year.ToString();
                monthYear.Month = "";
                ContractList.Add(monthYear);
            }
            ViewBag.Years = ContractList.Select(x => new MonthYear { Year = x.Year });
            return View();
        }
        public ActionResult AccuralWorkShhet()
        {
            ViewBag.currentMonth = DateTime.Now.Month.ToString();
            ViewBag.currentYear = DateTime.Now.Year.ToString();
            ViewBag.Months = new List<SelectListItem> {
                new SelectListItem { Text = "January", Value = "1" },
                new SelectListItem { Text = "February", Value = "2" },
                new SelectListItem { Text = "March", Value = "3" }, 
                new SelectListItem{Text="April",Value="4"},
                new SelectListItem{Text="May",Value="5"},
                new SelectListItem{Text="June",Value="6"},
                new SelectListItem{Text="July",Value="7"},
                new SelectListItem{Text="August",Value="8"},
                new SelectListItem{Text="September",Value="9"},
                new SelectListItem{Text="October",Value="10"},
                new SelectListItem{Text="November",Value="11"},
                new SelectListItem{Text="December",Value="12"}
            };
            BCBSClient client = new BCBSClient();
            string years = client.GetAccuralReportMonthYear();
            List<MonthYear> ContractList = new List<MonthYear>();
            if (!string.IsNullOrEmpty(years))
            {
                ContractList = JsonConvert.DeserializeObject<List<MonthYear>>(years);
                int currentexist = ContractList.Where(x => x.Year == DateTime.Now.Year.ToString()).Count();
                if (!(currentexist > 0))
                {
                    MonthYear monthYear = new MonthYear();
                    monthYear.Year = DateTime.Now.Year.ToString();
                    monthYear.Month = "";
                    ContractList.Add(monthYear);
                }
            }
            else
            {
                ContractList = JsonConvert.DeserializeObject<List<MonthYear>>(years);
                MonthYear monthYear = new MonthYear();
                monthYear.Year = DateTime.Now.Year.ToString();
                monthYear.Month = "";
                ContractList.Add(monthYear);
            }
            ViewBag.Years = ContractList.Select(x => new MonthYear { Year = x.Year });
            return View();
        }
        public JsonResult GetAccuralListbyMonthYear(string month, string year)
        {
            BCBSClient client = new BCBSClient();
            string result = client.GetAccuralReportByMonthYear(month, year);
            List<AccuralReportModel> AccuralList = JsonConvert.DeserializeObject<List<AccuralReportModel>>(result);
            return Json(AccuralList, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetAccuralListbyDate(string fromdate, string todate)
        {
            DateTime fromdt = Convert.ToDateTime(fromdate);
            fromdate = fromdt.ToString("yyyy-MM-dd");
            DateTime todt = Convert.ToDateTime(todate);
            todate = todt.ToString("yyyy-MM-dd");
            BCBSClient client = new BCBSClient();
            string result = client.GetAccuralReportByDate(fromdate, todate);
            List<AccuralReportModel> AccuralList = JsonConvert.DeserializeObject<List<AccuralReportModel>>(result);
            return Json(AccuralList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult RevenueExpense()
        {
            ViewBag.currentMonth = DateTime.Now.Month.ToString();
            ViewBag.currentYear = DateTime.Now.Year.ToString();
            ViewBag.Months = new List<SelectListItem> {
                new SelectListItem { Text = "January", Value = "1" },
                new SelectListItem { Text = "February", Value = "2" },
                new SelectListItem { Text = "March", Value = "3" }, 
                new SelectListItem{Text="April",Value="4"},
                new SelectListItem{Text="May",Value="5"},
                new SelectListItem{Text="June",Value="6"},
                new SelectListItem{Text="July",Value="7"},
                new SelectListItem{Text="August",Value="8"},
                new SelectListItem{Text="September",Value="9"},
                new SelectListItem{Text="October",Value="10"},
                new SelectListItem{Text="November",Value="11"},
                new SelectListItem{Text="December",Value="12"}
            };
            BCBSClient client = new BCBSClient();
            string years = client.GetAccuralReportMonthYear();
            List<MonthYear> ContractList = new List<MonthYear>();
            if (!string.IsNullOrEmpty(years))
            {
                ContractList = JsonConvert.DeserializeObject<List<MonthYear>>(years);
                int currentexist = ContractList.Where(x => x.Year == DateTime.Now.Year.ToString()).Count();
                if (!(currentexist > 0))
                {
                    MonthYear monthYear = new MonthYear();
                    monthYear.Year = DateTime.Now.Year.ToString();
                    monthYear.Month = "";
                    ContractList.Add(monthYear);
                }
            }
            else
            {
                ContractList = JsonConvert.DeserializeObject<List<MonthYear>>(years);
                MonthYear monthYear = new MonthYear();
                monthYear.Year = DateTime.Now.Year.ToString();
                monthYear.Month = "";
                ContractList.Add(monthYear);
            }
            ViewBag.Years = ContractList.Select(x => new MonthYear { Year = x.Year });
            return View();
        }

        public JsonResult GetProjectRevenueExpensebyMonthYear(string month, string year)
        {
            BCBSClient client = new BCBSClient();
            string result = client.GetProjectRevenueExpenseMonthYear(month, year);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetProjectRevenueExpensebyDate(string fromdate, string todate)
        {
            DateTime fromdt = Convert.ToDateTime(fromdate);
            fromdate = fromdt.ToString("yyyy-MM-dd");
            DateTime todt = Convert.ToDateTime(todate);
            todate = todt.ToString("yyyy-MM-dd");
            BCBSClient client = new BCBSClient();
            string result = client.GetProjectRevenueExpenseByDate(fromdate, todate);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetServiceRevenueExpensebyMonthYear(string month, string year)
        {
            BCBSClient client = new BCBSClient();
            string result = client.GetServiceRevenueExpenseMonthYear(month, year);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetServiceRevenueExpensebyDate(string fromdate, string todate)
        {
            DateTime fromdt = Convert.ToDateTime(fromdate);
            fromdate = fromdt.ToString("yyyy-MM-dd");
            DateTime todt = Convert.ToDateTime(todate);
            todate = todt.ToString("yyyy-MM-dd");
            BCBSClient client = new BCBSClient();
            string result = client.GetServiceRevenueExpenseByDate(fromdate, todate);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCustomerRevenueExpensebyMonthYear(string month, string year)
        {
            BCBSClient client = new BCBSClient();
            string result = client.GetPlanCustomerRevenueExpenseMonthYear(month, year);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetCustomerRevenueExpensebyDate(string fromdate, string todate)
        {
            DateTime fromdt = Convert.ToDateTime(fromdate);
            fromdate = fromdt.ToString("yyyy-MM-dd");
            DateTime todt = Convert.ToDateTime(todate);
            todate = todt.ToString("yyyy-MM-dd");
            BCBSClient client = new BCBSClient();
            string result = client.GetPlanCustomerRevenueExpenseByDate(fromdate, todate);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GrandBillingWorkSheet()
        {
            return View();
        }
    }
}
