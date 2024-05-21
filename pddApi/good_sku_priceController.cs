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
            List<BackMsg> results = new BackMsg[BodyList.Count].ToList();
            Parallel.For(0, BodyList.Count, i =>
            {
                BackMsg backMsg = new BackMsg();
                //数据正常请求接口
                Dictionary<string, string> parameters = new Dictionary<string, string>
                {
                    { "type", "pdd.goods.sku.price.update" },
                    { "client_id", apiHelp.client_id },
                    { "access_token", BodyList[i].mall.mall_token },
                    { "timestamp", ((DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000/1000).ToString() },
                    { "data_type", "JSON" },
                    //{ "goods_id", item.goods_id.ToString() },

                };
                foreach (PropertyInfo property in BodyList[i].GetType().GetProperties())
                {
                    object value = property.GetValue(BodyList[i]);
                    if (property.Name == "sku_price_list")
                    {
                        var settings = new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore
                        };

                        value = JsonConvert.SerializeObject(BodyList[i].sku_price_list, settings);
                    }
                    if (value != null) // 检查值是否为null  
                    {
                        parameters.Add(property.Name, value.ToString());
                    }
                }
                backMsg = apiHelp.SendPddApi(parameters);
                results[i] = backMsg;
            });

            var json = JsonConvert.SerializeObject(results);
            return json;
        }


    }

}
