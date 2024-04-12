using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Pdd_Models;
using Pdd_Models.Models;
using System.ComponentModel;
using System.Data;
using System.Reflection;
using System.Xml.Serialization;

namespace APIOffice.Controllers.pddApi
{
    [ApiController]
    [Description("快速sql")]
    [Route("danger/Sql")]
    public class quickSqlController : Controller
    {
        [Route("list")]
        public string list([FromForm] string sql)
        {

            BackTable backData = new BackTable();
            try
            {
                DataTable dt = SQLHelper.GetDataTable1(sql);
                backData.Code = 0;
                backData.Data = dt;
            }
            catch(Exception ex)
            {
                backData.Code = 100;
                backData.Mess=ex.Message;
            }
            var json = JsonConvert.SerializeObject(backData);
            return json;
        }

        [Route("Upd")]
        public string Upd([FromForm] string sql)
        {
            BackMsg backData = new BackMsg();
            try
            {
                backData.Mess = SQLHelper.ExecuteCommand1(sql);

                backData.Code = 0;
            }
            catch (Exception ex)
            {
                backData.Code = 100;
                backData.Mess = ex.Message;
            }
            var json = JsonConvert.SerializeObject(backData);
            return json;
        }


    }
}
