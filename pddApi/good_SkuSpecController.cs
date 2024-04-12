using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.ComponentModel;

namespace APIOffice.Controllers.pddApi
{
    [ApiController]
    [Description("获取specID")]
    [Route("goods/specID")]
    public class good_SkuSpecController : Controller
    {
        [Route("get")]
        public string get([FromForm]string Parent_Spec_Id, [FromForm] string Spec_Name, [FromForm] string access_token)
        {

            BackMsg backMsg = new BackMsg();
            BackData backData = new BackData();
            //数据正常请求接口
            Dictionary<string, string> parameters = new Dictionary<string, string>
                {
                { "type", "pdd.goods.spec.id.get" },
                { "client_id", apiHelp.client_id },
                { "access_token", access_token },
                { "timestamp", ((DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000/1000).ToString() },
                { "data_type", "JSON" },
                { "parent_spec_id",Parent_Spec_Id},
                {"spec_name",Spec_Name}
                };

            backMsg = apiHelp.SendPddApi(parameters);
            //数据处理(成功时)
            if (backMsg.Code == 0)
            {
                JToken jToken = JsonConvert.DeserializeObject<JToken>(backMsg.Mess);
                backMsg.Mess = jToken["goods_spec_id_get_response"].ToString();
            }
            var json = JsonConvert.SerializeObject(backMsg);
            return json;
        }
    }
}
