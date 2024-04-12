using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Pdd_Models.Models;
using System.ComponentModel;
using System.Data;

namespace APIOffice.Controllers.pddApi
{
    [ApiController]
    [Description("错误日志")]
    [Route("sys/Error")]
    public class SysErrorController : Controller
    {
        
        [Route("Add")]
        public string Add([FromForm] string Error)
        {
            BackMsg backData = new BackMsg();

            try
            {
                string i= SQLHelper.ExecuteCommand1("insert into Sys_Error(msg) values ('" + Error + "')");

                if (MyConvert.ToInt(i) == 0)
                {
                    backData.Code = 200;
                    backData.Mess = i;
                }
            }
            catch (Exception ex)
            {
                backData.Code = 201;
                backData.Mess = ex.Message;
            }
            finally { }
            return JsonConvert.SerializeObject(backData);
        }

    }
}
