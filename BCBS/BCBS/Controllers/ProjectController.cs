using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BCBS.bsbcserviceref;
using BCBS.Models;
using Newtonsoft.Json;
namespace BCBS.Controllers
{
    public class ProjectController : Controller
    {
        //
        // GET: /Project/
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult New()
        {
            ProjectModel project = new ProjectModel();
            return View(project);
        }
        [HttpPost]
        public ActionResult New(ProjectModel project)
        {
            if (ModelState.IsValid)
            {
                BCBSClient client = new BCBSClient();
                long Id = 0;
                Id = client.InsertProject(project.Name, project.ChargeCode, project.HighLevelBudget.ToString(), project.Status, project.Description, project.RC, project.GLAccount);
                if (Id > 0)
                {
                    TempData["Message"] = "Project Added successfully..!";
                }
                else
                {
                    TempData["Error"] = "Project Adding failed..!";
                }
                ModelState.Clear();
                return RedirectToAction("Index", "Project");
            }
            else
            {
                return View(project);
            }
        }
        [HttpGet]
        public ActionResult Edit(long id)
        {
            ProjectModel project = new ProjectModel();
            if (id > 0)
            {
                BCBSClient client = new BCBSClient();
                string projectData = client.GetProjectById(id);
                if (!string.IsNullOrEmpty(projectData))
                {
                    project = JsonConvert.DeserializeObject<ProjectModel>(projectData);
                }
                else
                {
                    TempData["Error"] = "Requested project not available!!";
                    return RedirectToAction("Index", "Project");
                }
            }
            return View(project);
        }
        [HttpPost]
        public ActionResult Edit(ProjectModel project)
        {
            if (ModelState.IsValid)
            {
                BCBSClient client = new BCBSClient();
                long Id = 0;
                Id = client.UpdateProjectById(project.Id, project.Name, project.ChargeCode, project.HighLevelBudget.ToString(), project.Status, project.Description, project.RC, project.GLAccount);
                if (Id > 0)
                {
                    TempData["Message"] = "Project Updated successfully..!";
                }
                else
                {
                    TempData["Error"] = "Project Update failed..!";
                }
                ModelState.Clear();
                return RedirectToAction("Index", "Project");
            }
            else
            {
                return View(project);
            }
        }
        public JsonResult GetProjectList()
        {
            try
            {
                BCBSClient client = new BCBSClient();
                string result = client.GetProjectList();
                List<ProjectModel> ProjectList = JsonConvert.DeserializeObject<List<ProjectModel>>(result);
                return Json(ProjectList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult DeleteProjectById(string id)
        {
            bool isdeleted = false;
            if (!string.IsNullOrEmpty(id))
            {
                BCBSClient client = new BCBSClient();
                isdeleted = client.DeleteProjectById(id);
            }
            return Json(isdeleted, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetContractListbyProjectId(string id)
        {
            string result = string.Empty;
            BCBSClient client = new BCBSClient();
            List<ContractListModel> ContractList = new List<ContractListModel>();
            result = client.GetcontractListByProjectId(Convert.ToInt64(id));
            if (!string.IsNullOrEmpty(result))
            {
                ContractList = JsonConvert.DeserializeObject<List<ContractListModel>>(result).ToList();
            }
            return Json(ContractList, JsonRequestBehavior.AllowGet);
        }
        public JsonResult IsChargeCodeExist(string ChargeCode)
        {

            bool isexist = false;
            if (!string.IsNullOrEmpty(ChargeCode))
            {
                //if (!string.IsNullOrEmpty(existCode))
                //{
                //if (ChargeCode.ToLower() != existCode.ToLower())
                //{
                BCBSClient client = new BCBSClient();
                isexist = client.CheckIsChargeCodeExist(ChargeCode, "project");
                //    }
                //}
            }
            return Json(!isexist, JsonRequestBehavior.AllowGet);
        }
    }
}
