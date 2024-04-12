using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Pdd_Models;
using System.ComponentModel;
using static System.Net.Mime.MediaTypeNames;

namespace APIOffice.Controllers.pddApi
{
    [ApiController]
    [Description("拼多多修改商品上下架")]
    [Route("goods/sale/status")]
    public class good_sale_statusController : Controller
    {
        [Route("set")]
        public string Set([FromBody] List<requsetSaleBody> BodyList)
        {
            List<BackMsg> results = new BackMsg[BodyList.Count].ToList();
            //foreach (var item in BodyList)
            //{
            //    BackMsg backMsg = new BackMsg();
            //    if (item.goods_id == 0 || item.is_onsale == -1 )
            //    {
            //        backMsg.Code = 1001;
            //        backMsg.Mess = "参数错误缺少参数。";
            //        results.Add(backMsg);
            //        continue;
            //    }
            //    //数据正常请求接口
            //    Dictionary<string, string> parameters = new Dictionary<string, string>
            //    {
            //        { "type", "pdd.goods.sale.status.set" },
            //        { "client_id", apiHelp.client_id },
            //        { "access_token", item.Malls.mall_token },
            //        { "timestamp", ((DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000/1000).ToString() },
            //        { "data_type", "JSON" },
            //        { "goods_id", item.goods_id.ToString() },
            //        { "is_onsale", item.is_onsale.ToString() },

            //    };
            //    backMsg = apiHelp.SendPddApi(parameters);
            //    results.Add(backMsg);
            //}
            Parallel.For(0, BodyList.Count, i =>
            {
                BackMsg backMsg = new BackMsg();
                if (BodyList[i].goods_id == 0 || BodyList[i].is_onsale == -1)
                {
                    backMsg.Code = 1001;
                    backMsg.Mess = "参数错误缺少参数。";
                    results[i]=backMsg;
                }
                else
                {
                    //数据正常请求接口
                    Dictionary<string, string> parameters = new Dictionary<string, string>
                    {
                        { "type", "pdd.goods.sale.status.set" },
                        { "client_id", apiHelp.client_id },
                        { "access_token", BodyList[i].Malls.mall_token },
                        { "timestamp", ((DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000/1000).ToString() },
                        { "data_type", "JSON" },
                        { "goods_id", BodyList[i].goods_id.ToString() },
                        { "is_onsale", BodyList[i].is_onsale.ToString() },

                    };
                    backMsg = apiHelp.SendPddApi(parameters);
                    results[i] = backMsg;
                }

            });
            var json = JsonConvert.SerializeObject(results);
            return json;
        }



    }

}
