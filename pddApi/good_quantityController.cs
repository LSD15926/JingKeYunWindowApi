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
            List<BackMsg> results = new BackMsg[BodyList.Count].ToList();
            Parallel.For(0, BodyList.Count, i =>
            {
                BackMsg backMsg = new BackMsg();
                //数据正常请求接口
                Dictionary<string, string> parameters = new Dictionary<string, string>
                {
                    { "type", "pdd.goods.quantity.update" },
                    { "client_id", apiHelp.client_id },
                    { "access_token", BodyList[i].Malls.mall_token },
                    { "timestamp", ((DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000/1000).ToString() },
                    { "data_type", "JSON" },
                    { "goods_id", BodyList[i].goods_id.ToString() },
                    { "quantity", BodyList[i].quantity.ToString() },
                    { "sku_id", BodyList[i].sku_id.ToString() },
                    { "outer_id", BodyList[i].outer_id.ToString() },
                    { "update_type", BodyList[i]    .update_type.ToString() },
                };
                backMsg = apiHelp.SendPddApi(parameters);
                results[i] = backMsg;
            });

            var json = JsonConvert.SerializeObject(results);
            return json;
        }


    }




}
