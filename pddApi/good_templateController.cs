using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Pdd_Models;
using System.ComponentModel;
using System.Reflection;

namespace APIOffice.Controllers.pddApi
{
    [ApiController]
    [Description("获取运费模板")]
    [Route("goods/template")]
    public class good_templateController : Controller
    {
        [Route("get")]
        public string get([FromBody] TempRequset requset)
        {

            BackMsg backMsg = new BackMsg();
            BackData backData = new BackData();
            //数据正常请求接口
            Dictionary<string, string> parameters = new Dictionary<string, string>
                {
                { "type", "pdd.goods.logistics.template.get" },
                { "client_id", apiHelp.client_id },
                { "access_token", requset.mall.mall_token },
                { "timestamp", ((DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000/1000).ToString() },
                { "data_type", "JSON" },
                };

            foreach (PropertyInfo property in requset.GetType().GetProperties())
            {
                object value = property.GetValue(requset);
                if (value != null) // 检查值是否为null  
                {
                    parameters.Add(property.Name, value.ToString());
                }
            }

            backMsg = apiHelp.SendPddApi(parameters);
            //数据处理(成功时)
            if (backMsg.Code == 0)
            {
                JToken jToken = JsonConvert.DeserializeObject<JToken>(backMsg.Mess);
                backMsg.Mess = jToken["goods_logistics_template_get_response"]["logistics_template_list"].ToString();
            }
            var json = JsonConvert.SerializeObject(backMsg);
            return json;
        }


    }
    
}
