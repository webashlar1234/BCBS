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
    public class ServiceController : Controller
    {
        //
        // GET: /ServiceTypes/

        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult New()
        {
            BCBSClient client = new BCBSClient();
            
            ServiceModel serviceType = new ServiceModel();
            string projectList = client.GetProjectList();
            if (!string.IsNullOrEmpty(projectList))
            {
                ViewBag.Projects = JsonConvert.DeserializeObject<List<ProjectModel>>(projectList).Select(x => new { x.Id, x.Name });
            }
            else
            {
                ViewBag.Projects = "";
            }
            //string servicelist = client.GetProjectList();
            //if (!string.IsNullOrEmpty(servicelist))
            //{
            //    List<ProjectModel> services = new List<ProjectModel>();
            //    ViewBag.Projects = JsonConvert.DeserializeObject<List<ProjectModel>>(servicelist).Select(x => new { x.Id, x.Name });
            //}
            //else
            //{
            //    ViewBag.Projects = "";
            //}
            return View(serviceType);
        }
        [HttpPost]
        public ActionResult New(ServiceModel serviceType)
        {
            if (ModelState.IsValid)
            {
                BCBSClient client = new BCBSClient();
                long Id = 0;

                Id = client.InsertServiceType(serviceType.Name, serviceType.ProjectId,serviceType.Status,serviceType.FeesType,serviceType.Budget,serviceType.Notes);

                if (Id > 0)
                {
                    //List<string> selectedFees = serviceType.FeesType.Split(',').ToList();
                    //foreach (string feestype in selectedFees)
                    //{
                    //    var fees = feestype.Split('=');
                    //    string type = fees[0].ToString();
                    //    double amount = Convert.ToDouble(fees[1].ToString());
                    //    long resultid = client.InsertServiceFeesType(Id, type, amount);
                    //    if (!(resultid > 0))
                    //    {
                    //        bool deleteservice = client.DeleteServiceTypeById(Id.ToString());
                    //        TempData["Error"] = "Service Adding failed..!";
                    //        return RedirectToAction("Index", "Service");
                    //    }
                    //}
                    TempData["Message"] = "Service Added successfully..!";
                }
                else
                {
                    TempData["Error"] = "Service Adding failed..!";
                }
                ModelState.Clear();
                return RedirectToAction("Index", "Service");
            }
            else
            {
                return View(serviceType);
            }
        }
        [HttpGet]
        public ActionResult Edit(long id)
        {
            BCBSClient client = new BCBSClient();
            ServiceModel serviceType = new ServiceModel();
            if (id > 0)
            {
                string serviceData = client.GetServiceTypeById(id);
                if (!string.IsNullOrEmpty(serviceData))
                {
                    serviceType = JsonConvert.DeserializeObject<ServiceModel>(serviceData);
                    //string feestype = client.GetServiceFeesTypeByServiceId(id);
                    //List<ServiceFeesTypeModel> servicefeestype = new List<ServiceFeesTypeModel>();
                    //servicefeestype = JsonConvert.DeserializeObject<List<ServiceFeesTypeModel>>(feestype);
                    //ViewBag.Fees = feestype;
                }
                else
                {
                    TempData["Error"] = "Requested service not available!!";
                    return RedirectToAction("Index", "Service");
                }
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
            //string servicelist = client.GetProjectList();
            //if (!string.IsNullOrEmpty(servicelist))
            //{
            //    List<ProjectModel> services = new List<ProjectModel>();
            //    ViewBag.Projects = JsonConvert.DeserializeObject<List<ProjectModel>>(servicelist).Select(x => new { x.Id, x.Name });
            //}
            //else
            //{
            //    ViewBag.Projects = null;
            //}
            return View(serviceType);
        }
        [HttpPost]
        public ActionResult Edit(ServiceModel serviceType)
        {
            if (ModelState.IsValid)
            {
                BCBSClient client = new BCBSClient();
                long Id = 0;
                Id = client.UpdateServiceTypeById(serviceType.Id, serviceType.Name,serviceType.ProjectId,serviceType.Status,serviceType.Notes);
                if (Id > 0)
                {
                    //if (!string.IsNullOrEmpty(serviceType.FeesType))
                    //{
                    //    //bool deletefees = client.DeleteServiceFeesTypeByServiceId(Id.ToString());
                    //    //List<string> selectedFees = serviceType.FeesType.Split(',').ToList();
                    //    foreach (string feestype in selectedFees)
                    //    {
                    //        var fees = feestype.Split('=');
                    //        string type = fees[0].ToString();
                    //        double amount = Convert.ToDouble(fees[1].ToString());
                    //        long resultid = client.InsertServiceFeesType(Id, type, amount);
                    //        if (!(resultid > 0))
                    //        {
                    //            TempData["Error"] = "Service Updating failed..!";
                    //            return RedirectToAction("Index", "Service");
                    //        }
                    //    }
                    //}
                    TempData["Message"] = "Service  Updated successfully..!";
                }
                else
                {
                    TempData["Error"] = "Service Update failed..!";
                }
                ModelState.Clear();
                return RedirectToAction("Index", "Service");
            }
            else
            {
                return View(serviceType);
            }
        }
        public JsonResult GetServiceList()
        {
            BCBSClient client = new BCBSClient();
            string result = client.GetServiceTypeList();
            List<ServiceTypeListModel> ServiceList = JsonConvert.DeserializeObject<List<ServiceTypeListModel>>(result);
            return Json(ServiceList, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetProjectList()
        {
            BCBSClient client = new BCBSClient();
            string result = client.GetProjectList();
            List<ProjectModel> ProjectList = JsonConvert.DeserializeObject<List<ProjectModel>>(result);
            var resultx = ProjectList.Select(x => new { x.Id, x.Name });
            return Json(resultx, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DeleteServiceById(string id)
        {
            bool isdeleted = false;
            if (!string.IsNullOrEmpty(id))
            {
                BCBSClient client = new BCBSClient();
                isdeleted = client.DeleteServiceTypeById(id);
            }
            return Json(isdeleted, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetContractFeesTypeByServiceId(long serviceId)
        {
            BCBSClient client = new BCBSClient();
            string result = client.GetContractFeesTypeByServiceId(serviceId);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}
