using Front_end.Models;
using Front_end.Models.GlobalSetting;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime;
using System.Text.Json;

namespace Front_end.Controllers
{
    public class LoginController : Controller
    {
        private readonly IOptions<SettingsModel> _setOptions;
        private readonly IHttpClientFactory _factory;
        public LoginController(IOptions<SettingsModel> setOptions, IHttpClientFactory factory)
        {
            _setOptions = setOptions;
            _factory = factory;
        }
        public IActionResult Login()
        {
            ViewBag.Title = "MCF";
            return View();
        }

        public async Task<IActionResult> PostLogin([FromBody]MsUser userModel)
        {
            var badResponse = new HttpResponseMessage();
            string? configAppsBackEnd = _setOptions.Value.urlBackendIpHttps + "/api/Login";
            var ipHttpsOption = configAppsBackEnd;

            // Check if a matching key was found
            if (ipHttpsOption != null)
            {
                
                var url = ipHttpsOption;

                var jsondata = System.Text.Json.JsonSerializer.Serialize(userModel);


                var content = new StringContent(jsondata, System.Text.Encoding.UTF8, "application/json");

                var client = _factory.CreateClient();
                
                var response = await client.PostAsync(url, content);

                

                // Optionally, handle the response here
                if (response.IsSuccessStatusCode)
                {
                    var outputResponse = System.Text.Json.JsonSerializer.DeserializeAsync<MsUser>(response.Content.ReadAsStreamAsync().Result);
                    try
                    {
                        var resultData = await outputResponse;

                        HttpContext.Session.SetString("Nama", resultData!.UserName!);
                        HttpContext.Session.SetInt32("is_active", resultData!.IsActive ? 1 : 0);
                    }
                    catch (Exception)
                    {
                        return StatusCode(((int)HttpStatusCode.InternalServerError));
                    }
                    
                    //set cookies
                    CookieOptions option = new CookieOptions();
                    option.Expires = DateTime.Now.AddMilliseconds(10);
                    Response.Cookies.Append("session-cookies", "loogedin", option);

                    
                    return Json("Oke lah");
                }
                else
                {
                    badResponse = response;
                }
            }
            int errStatusCode = int.Parse(badResponse.StatusCode.ToString(), System.Globalization.CultureInfo.InvariantCulture);
            return StatusCode(errStatusCode);
        }

    }
}
