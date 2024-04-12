using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Pdd_Models.Models;
using System.ComponentModel;

namespace APIOffice.Controllers.pddApi
{
    [ApiController]
    [Description("获取叶子类目")]
    [Route("goods/cats")]
    public class good_catsController : Controller
    {
        [Route("get")]
        public string get([FromBody] RequstCat requstCat)
        {

            BackMsg backMsg = new BackMsg();
            BackData backData = new BackData();
            //数据正常请求接口
            Dictionary<string, string> parameters = new Dictionary<string, string>
                {
                { "type", "pdd.goods.authorization.cats" },
                { "client_id", apiHelp.client_id },
                { "access_token", requstCat.mall.mall_token },
                { "timestamp", ((DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000/1000).ToString() },
                { "data_type", "JSON" },
                { "parent_cat_id",requstCat.Catid.ToString()},
            };

            backMsg = apiHelp.SendPddApi(parameters);
            //数据处理(成功时)
            if (backMsg.Code == 0)
            {
                JToken jToken = JsonConvert.DeserializeObject<JToken>(backMsg.Mess);
                backMsg.Mess = jToken["goods_auth_cats_get_response"]["goods_cats_list"].ToString();
            }
            var json = JsonConvert.SerializeObject(backMsg);
            return json;
        }
    }
}
