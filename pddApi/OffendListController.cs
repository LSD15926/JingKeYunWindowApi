using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Pdd_Models.Models;
using System.ComponentModel;
using System.Data;

namespace APIOffice.Controllers.pddApi
{
    [ApiController]
    [Description("获取违规词品牌词")]
    [Route("Offend/list")]
    public class OffendListController : Controller
    {
        [Route("get")]
        public string get()
        {

            BackData backData = new BackData();
            try
            {
                DataTable dt = SQLHelper.GetDataTable1("select * from OffendWord ");
                List<object> list = new List<object>();
                if (dt != null)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {

                        list.Add(dt.Rows[i]["Offend_Word"].ToString());
                    }
                }
                backData.Code = 0;
                backData.Data = list;
            }
            catch(Exception ex)
            {
                backData.Code = 100;
                backData.Mess=ex.Message;
            }
            var json = JsonConvert.SerializeObject(backData);
            return json;
        }
    }
}
