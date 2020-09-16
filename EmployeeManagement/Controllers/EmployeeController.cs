using EmployeeManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;

namespace EmployeeManagement.Controllers
{
    public class EmployeeController : Controller
    {
        // GET: Employee
        //public ActionResult Index()
        //{
        //    return View();
        //}
        string restUrl = "http://dummy.restapiexample.com/";
        public async Task<ActionResult> Index()
        {
            AllEmployees empInfo = new AllEmployees();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(restUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage res = await client.GetAsync("api/v1/employees");
                if (res.IsSuccessStatusCode)
                {
                    var empRes = res.Content.ReadAsStringAsync().Result;
                    empInfo = JsonConvert.DeserializeObject<AllEmployees>(empRes);
                    //empInfo.Add(new Models.Employee() { age = "1" });
                    int[] ages = (from x in empInfo.data
                                  select Convert.ToInt32(x.employee_age)).ToArray();
                    ViewBag.AverageAge = ages.Average();
                    TempData["AverageAge"] = ViewBag.AverageAge;

                    double[] salary = (from x in empInfo.data
                                       select Convert.ToDouble(x.employee_salary)).ToArray();
                    ViewBag.AverageSalary = salary.Average();
                    TempData["AverageSalary"] = ViewBag.AverageSalary;
                }
            }
            return View(empInfo.data);
        }

        public async Task<ActionResult> Details(int? id)
        {
            if (id != null)
            {
                Employee emp = new Employee();
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(restUrl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage res = await client.GetAsync("api/v1/employee/" + id);
                    if (res.IsSuccessStatusCode)
                    {
                        var empRes = res.Content.ReadAsStringAsync().Result;
                        //   empInfo = JsonConvert.DeserializeObject<AllEmployees>(empRes);
                        var details = JObject.Parse(empRes);
                        if (details["status"].ToString() == "success")
                        {
                            emp = JsonConvert.DeserializeObject<Employee>(details["data"].ToString());
                        }

                        ViewBag.AverageAge = TempData["AverageAge"];
                        ViewBag.AverageSalary = TempData["AverageSalary"];
                        ViewBag.Message = details["message"];
                    }
                }
                return View(emp);
            }
            return View();

        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(Employee emp)
        {

            string data = "{ \"name\":\"" + emp.employee_name + "\",\"salary\":\"" + emp.employee_salary + "\",\"age\":\"" + emp.employee_age + "\"}";

            StringContent httpContent = new StringContent(data, Encoding.UTF8, "text/json");
            using (var client = new HttpClient())
            { 
                var content = new StringContent(data, Encoding.UTF8, "application/json");
                using (var response = await client.PostAsync("http://dummy.restapiexample.com/api/v1/create", content))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        using (var responseContent = response.Content)
                        {
                            string result = await responseContent.ReadAsStringAsync();
                            var details = JObject.Parse(result);

                            emp = JsonConvert.DeserializeObject<Employee>(details["data"].ToString());
                             
                            ViewBag.EmpId = emp.id;
                        }
                    }

                }
            }
            emp.employee_name = "";
            emp.employee_age = "";
            emp.employee_salary = "";
            emp.profile_image = "";
            //   ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");

            return View(emp);
        }

        public async Task<ActionResult> Edit(int? id)
        {
            if (id != null)
            {
                Employee emp = new Employee();
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(restUrl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage res = await client.GetAsync("api/v1/employee/" + id);
                    if (res.IsSuccessStatusCode)
                    {
                        var empRes = res.Content.ReadAsStringAsync().Result;
                        //   empInfo = JsonConvert.DeserializeObject<AllEmployees>(empRes);
                        var details = JObject.Parse(empRes);
                        if (details["status"].ToString() == "success")
                        {
                            emp = JsonConvert.DeserializeObject<Employee>(details["data"].ToString());
                        } 
                    }
                }
                return View(emp);
            }
            return View(); 
        }

        [HttpPost]
        public async Task<ActionResult> Edit(Employee emp)
        {
            string data = "{ \"name\":\"" + emp.employee_name + "\",\"salary\":\"" + emp.employee_salary + "\",\"age\":\"" + emp.employee_age + "\"}";

            StringContent httpContent = new StringContent(data, Encoding.UTF8, "text/json");
            using (var client = new HttpClient())
            {
                var content = new StringContent(data, Encoding.UTF8, "application/json");
                using (var response = await client.PutAsync("http://dummy.restapiexample.com/api/v1/update/" + emp.id, content))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        using (var responseContent = response.Content)
                        {
                            string result = await responseContent.ReadAsStringAsync();
                            var details = JObject.Parse(result);

                            emp = JsonConvert.DeserializeObject<Employee>(details["data"].ToString());

                            ViewBag.EmpId = emp.id;
                        }
                    }

                }
            }
             
            return View(emp);
            
        }

        public async Task<ActionResult> Delete(int? id)
        {
            if (id != null)
            {
                Employee emp = new Employee();
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(restUrl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage res = await client.GetAsync("api/v1/employee/" + id);
                    if (res.IsSuccessStatusCode)
                    {
                        var empRes = res.Content.ReadAsStringAsync().Result; 
                        var details = JObject.Parse(empRes);
                        if (details["status"].ToString() == "success")
                        {
                            emp = JsonConvert.DeserializeObject<Employee>(details["data"].ToString());
                        } 
                    }
                }
                return View(emp);
            }
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Delete(Employee emp)
        {
            string data = "{ \"name\":\"" + emp.employee_name + "\",\"salary\":\"" + emp.employee_salary + "\",\"age\":\"" + emp.employee_age + "\"}";

            StringContent httpContent = new StringContent(data, Encoding.UTF8, "text/json");
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(restUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage res = await client.DeleteAsync("api/v1/delete/" + emp.id);
                if (res.IsSuccessStatusCode)
                {
                    var empRes = res.Content.ReadAsStringAsync().Result;
                    var details = JObject.Parse(empRes);
                    if (details["status"].ToString() == "success")
                    {
                        // emp = JsonConvert.DeserializeObject<Employee>(details["data"].ToString());
                        ViewBag.Message = details["message"];
                    }
                }
            }

            return View(emp);

        }
    }
}