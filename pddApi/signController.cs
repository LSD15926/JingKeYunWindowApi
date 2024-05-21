using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Pdd_Models;
using Pdd_Models.Models;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;

namespace APIOffice.Controllers.pddApi
{
    [ApiController]
    [Description("生成sign")]
    [Route("sys/sign")]
    public class signController : Controller
    {
        [Route("get")]
        public string get([FromBody] string parJson)
        {
            Dictionary<string, string> parameters = JsonConvert.DeserializeObject<Dictionary<string, string>>(parJson);
            return PinduoduoSignHelper.GenerateSign(parameters);
        }
    }
}
