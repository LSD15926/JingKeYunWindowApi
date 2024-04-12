using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Reflection;
using Pdd_Models;

namespace APIOffice.Controllers.pddApi
{
    [ApiController]
    [Description("拼多多修改SKU价格")]
    [Route("goods/sale/status")]
    public class good_sku_priceController : Controller
    {
        [Route("update")]
        public string Update([FromBody] List<requsetSkuPriceModel> BodyList)
        {
            List<BackMsg> results = new List<BackMsg>();
            foreach (var item in BodyList)
            {
                BackMsg backMsg = new BackMsg();
                if (item.goods_id == 0 )
                {
                    backMsg.Code = 1001;
                    backMsg.Mess = "参数错误缺少参数。";
                    results.Add(backMsg);
                    continue;
                }
                //数据正常请求接口
                Dictionary<string, string> parameters = new Dictionary<string, string>
                {
                    { "type", "pdd.goods.sku.price.update" },
                    { "client_id", apiHelp.client_id },
                    { "access_token", item.mall.mall_token },
                    { "timestamp", ((DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000/1000).ToString() },
                    { "data_type", "JSON" },
                    //{ "goods_id", item.goods_id.ToString() },

                };
                foreach (PropertyInfo property in item.GetType().GetProperties())
                {
                    object value = property.GetValue(item);
                    if (property.Name== "sku_price_list")
                    {
                        var settings = new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore
                        };

                        value = JsonConvert.SerializeObject(item.sku_price_list, settings);
                    }
                    if (value != null) // 检查值是否为null  
                    {
                        parameters.Add(property.Name, value.ToString());
                    }
                }
                backMsg = apiHelp.SendPddApi(parameters);
                results.Add(backMsg);
            }

            var json = JsonConvert.SerializeObject(results);
            return json;
        }

       
    }

}
