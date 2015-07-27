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
    public class ActivityController : Controller
    {
        //
        // GET: /Activity/

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult New()
        {
            BCBSClient client = new BCBSClient();
            string customerlist = client.GetcustomerList();
            string servicelist = client.GetServiceTypeList();

            if (!string.IsNullOrEmpty(customerlist))
            {
                ViewBag.Customers = JsonConvert.DeserializeObject<List<CustomerModel>>(customerlist).Select(x => new { x.Id, x.Name });
            }
            else
            {
                ViewBag.Customers = "";
            }
            if (!string.IsNullOrEmpty(servicelist))
            {
                ViewBag.Services = JsonConvert.DeserializeObject<List<ServiceModel>>(servicelist).Select(x => new { x.Id, x.Name });
            }
            else
            {
                ViewBag.Services = "";
            }
            string projectList = client.GetProjectList();
            if (!string.IsNullOrEmpty(projectList))
            {
                ViewBag.Projects = JsonConvert.DeserializeObject<List<ProjectModel>>(projectList).Select(x => new { x.Id, x.Name });
            }
            else
            {
                ViewBag.Projects = "";
            }

            string contractList = client.GetcontractList();
            if (!string.IsNullOrEmpty(contractList))
            {
                ViewBag.Contracts = JsonConvert.DeserializeObject<List<ContractModel>>(contractList).Select(x => new { x.Id, x.ContractCode });
            }
            else
            {
                ViewBag.Contracts = "";
            }
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
            ViewBag.Years = ContractList.GroupBy(cust => cust.Year).Select(grp => grp.First());
            return View();
        }

        public JsonResult ServiceByProjectID(string projectId)
        {
            string services = string.Empty;
            if (!string.IsNullOrEmpty(projectId))
            {
                BCBSClient client = new BCBSClient();
                services = client.GetServiceByProjectId(Convert.ToInt64(projectId));
            }
            return Json(services, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CustomerByServiceAndProjectId(string serviceId, string projectId)
        {
            string customers = string.Empty;
            if ((!string.IsNullOrEmpty(projectId)) && (!string.IsNullOrEmpty(serviceId)))
            {
                BCBSClient client = new BCBSClient();
                customers = client.GetCustomerByServiceAndProjectId(Convert.ToInt64(serviceId),Convert.ToInt64(projectId));
            }
            return Json(customers, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetContractForActivity(string customerId,string serviceID,string projectId,string fromMonth,string toMonth)
        {
            BCBSClient client = new BCBSClient();
            string result = client.GetContractDetailForActivity(Convert.ToInt64(projectId), Convert.ToInt64(serviceID), Convert.ToInt64(customerId),fromMonth,toMonth,DateTime.Now.Year.ToString());
            List<ContractInvoiceModel> ContractList = JsonConvert.DeserializeObject<List<ContractInvoiceModel>>(result);
            return Json(ContractList, JsonRequestBehavior.AllowGet);
        }



    }
}
