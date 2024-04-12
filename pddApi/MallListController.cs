using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Pdd_Models.Models;
using System.ComponentModel;
using System.Data;
using System.Reflection;
using System.Xml.Serialization;

namespace APIOffice.Controllers.pddApi
{
    [ApiController]
    [Description("获取店铺列表")]
    [Route("mall/list")]
    public class MallListController : Controller
    {
        [Route("get")]
        public string get([FromForm] int UserId)
        {

            BackData backData = new BackData();
            try
            {
                DataTable dt = SQLHelper.GetDataTable1("select * from u_mall where mall_del=0 and user_id=" + UserId +" order by id desc");
                List<object> list = new List<object>();
                if (dt != null)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        Mallinfo model = new Mallinfo();
                        model.mall_desc = dt.Rows[i]["mall_desc"].ToString();
                        model.merchant_type = Convert.ToInt32(dt.Rows[i]["merchant_type"]);
                        model.mall_id = Convert.ToInt64(dt.Rows[i]["mall_id"]);
                        model.logo = dt.Rows[i]["logo"].ToString();
                        model.mall_name = dt.Rows[i]["mall_name"].ToString();
                        model.mall_character = Convert.ToInt32(dt.Rows[i]["mall_character"].ToString());

                        model.mall_token = dt.Rows[i]["mall_token"].ToString();
                        model.mall_token_expire = MyConvert.ToLong(dt.Rows[i]["mall_token_expire"]);
                        model.user_id=UserId;
                        model.id = MyConvert.ToInt(dt.Rows[i]["id"]);
                        model.mall_group= dt.Rows[i]["mall_group"].ToString();
                        list.Add(model);
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

        [Route("del")]
        public string del([FromForm] string ids)
        {

            BackMsg backData = new BackMsg();
            try
            {

                backData.Mess=SQLHelper.ExecuteCommand1("update u_mall set mall_del=1 where mall_id in( " + ids + " )");
                
                backData.Code = 0;
            }
            catch (Exception ex)
            {
                backData.Code = 100;
                backData.Mess = ex.Message;
            }
            var json = JsonConvert.SerializeObject(backData);
            return json;
        }

        [Route("upd")]
        public string upd([FromBody] Mallinfo model)
        {

            BackMsg backData = new BackMsg();
            try
            {
                DataTable dt = SQLHelper.GetDataTable1("select * from u_mall where user_id=" + model.user_id+ " and mall_id="+model.mall_id);
                if (dt.Rows.Count == 0)//不存在新增
                {
                    backData.Mess=SQLHelper.ExecuteCommand1(string.Format("insert into u_mall(user_id,logo,mall_desc,mall_id,mall_name,merchant_type,mall_character,mall_token,mall_token_expire) " +
                        " values({0},'{1}','{2}',{3},'{4}',{5},{6},'{7}',{8})",model.user_id,model.logo,model.mall_desc,model.mall_id,model.mall_name,model.merchant_type,model.mall_character
                        ,model.mall_token,model.mall_token_expire));
                }
                else
                {
                    backData.Mess = SQLHelper.ExecuteCommand1(string.Format("update u_mall set mall_token='{0}',mall_token_expire={1},mall_del=0,mall_name='{2}',mall_group='{5}' where user_id={3} and mall_id={4} "
                        , model.mall_token,model.mall_token_expire,model.mall_name,model.user_id,model.mall_id,model.mall_group));
                }

                backData.Code = 0;
            }
            catch (Exception ex)
            {
                backData.Code = 100;
                backData.Mess = ex.Message;
            }
            var json = JsonConvert.SerializeObject(backData);
            return json;
        }


        [Route("info")]
        public string Info([FromForm] string Token)
        {
            BackMsg backMsg = new BackMsg();
            //数据正常请求接口
            Dictionary<string, string> parameters = new Dictionary<string, string>
                {
                { "type", "pdd.mall.info.get" },
                { "client_id", apiHelp.client_id },
                { "access_token", Token },
                { "timestamp", ((DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000/1000).ToString() },
                { "data_type", "JSON" },
            };

            backMsg = apiHelp.SendPddApi(parameters);
            //数据处理(成功时)
            if (backMsg.Code == 0)
            {
                JToken jToken = JsonConvert.DeserializeObject<JToken>(backMsg.Mess);
                backMsg.Mess = jToken["mall_info_get_response"].ToString();
            }
            var json = JsonConvert.SerializeObject(backMsg);
            return json;
        }


    }
}
