using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Pdd_Models;
using System.ComponentModel;

namespace APIOffice.Controllers.pddApi
{
    [ApiController]
    [Description("拼多多修改商品库存")]
    [Route("goods/quantity")]
    public class good_quantityController : Controller
    {
        [Route("Update")]
        public string Update([FromBody] List<requestQuantity> BodyList)
        {
            List<BackMsg> results = new List<BackMsg>();
            foreach (var item in BodyList)
            {
                BackMsg backMsg = new BackMsg();
                if (item.goods_id == 0 || item.quantity == 0 || (item.sku_id == 0 && item.outer_id == ""))
                {
                    backMsg.Code = 1001;
                    backMsg.Mess = "参数错误缺少参数。";
                    results.Add(backMsg);
                    continue;
                }
                //数据正常请求接口
                Dictionary<string, string> parameters = new Dictionary<string, string>
                {
                    { "type", "pdd.goods.quantity.update" },
                    { "client_id", apiHelp.client_id },
                    { "access_token", item.Malls.mall_token },
                    { "timestamp", ((DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000/1000).ToString() },
                    { "data_type", "JSON" },
                    { "goods_id", item.goods_id.ToString() },
                    { "quantity", item.quantity.ToString() },
                    { "sku_id", item.sku_id.ToString() },
                    { "outer_id", item.outer_id.ToString() },
                    { "update_type", item.update_type.ToString() },
                };
                backMsg = apiHelp.SendPddApi(parameters);
                results.Add(backMsg);
            }

            var json = JsonConvert.SerializeObject(results);
            return json;
        }


    }


   

}
