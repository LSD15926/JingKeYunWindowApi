using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Pdd_Models;
using Pdd_Models.Models;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;

namespace APIOffice.Controllers.pddApi
{
    [ApiController]
    [Description("获取商品列表")]
    [Route("goods/list")]
    public class good_listController : Controller
    {
        [Route("get")]
        public string get([FromBody] requestGoodList BodyList)
        {
            BackData backData = new BackData();
            BackMsg backMsg = new BackMsg();

            Dictionary<string, string> parameters = new Dictionary<string, string>
                {
                    { "type", "pdd.goods.list.get" },
                    { "client_id", apiHelp.client_id },
                    { "access_token", BodyList.Malls.mall_token },
                    { "timestamp", ((DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000/1000).ToString() },
                    { "data_type", "JSON" },
                };
            foreach (PropertyInfo property in BodyList.GetType().GetProperties())
            {
                object value = property.GetValue(BodyList);

                List<string> Names = new List<string>() { "Malls" };
                if (Names.Contains(property.Name))
                    continue;
                if (value != null && value.ToString() != "") // 检查值是否为null  
                {
                    parameters.Add(property.Name, value.ToString());
                }
            }
            backMsg = apiHelp.SendPddApi(parameters);
            List<object> AllGood = new List<object>();
            //数据处理(成功时)
            if (backMsg.Code == 0)
            {
                JToken jToken = JsonConvert.DeserializeObject<JToken>(backMsg.Mess);
                var json1 = jToken["goods_list_get_response"]["goods_list"].ToString();
                List<GoodListResponse> pageGood = JsonConvert.DeserializeObject<List<GoodListResponse>>(json1);
                AllGood.AddRange(pageGood);
                //获取总数
                int max = MyConvert.ToInt(jToken["goods_list_get_response"]["total_count"]);
                backData.Code = 0;
                backData.Mess = max.ToString();
                backData.Data = AllGood;

            }
            else
            {
                backData.Code=backMsg.Code;
                backData.Mess=backMsg.Mess;
            }
            
            var json = JsonConvert.SerializeObject(backData);
            return json;
        }


        [Route("try")]
        public string Try()
        {
            return "Hello World!";
        }
    }

    public class pagetotal
    {
        /// <summary>
        /// 总个数
        /// </summary>
        public int total { get; set; } = 0;

        /// <summary>
        /// 向下取整页数
        /// </summary>
        public int minPage { get; set; } = 0;
        /// <summary>
        /// 向上取证页数
        /// </summary>
        public int maxPage { get; set; }=0;
    }


}
