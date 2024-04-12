using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.ComponentModel;

namespace APIOffice.Controllers.pddApi
{
    [ApiController]
    [Description("获取token")]
    [Route("auth/token")]
    public class GetTokenController : Controller
    {
        [Route("get")]
        public string get([FromForm]string code)
        {

            BackMsg backMsg = new BackMsg();
            BackData backData = new BackData();
            //数据正常请求接口
            Dictionary<string, string> parameters = new Dictionary<string, string>
                {
                { "type", "pdd.pop.auth.token.create" },
                { "client_id", apiHelp.client_id },
                { "timestamp", ((DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000/1000).ToString() },
                { "data_type", "JSON" },  
                { "code",code},
            };

            backMsg = apiHelp.SendPddApi(parameters);
            //数据处理(成功时)
            if (backMsg.Code == 0)
            {
                JToken jToken = JsonConvert.DeserializeObject<JToken>(backMsg.Mess);
                backMsg.Mess = jToken["pop_auth_token_create_response"].ToString();
            }
            var json = JsonConvert.SerializeObject(backMsg);
            return json;
        }

        [Route("try")]
        public string Try([FromForm] IFormCollection file)
        {
            return file.Files[0].FileName;
        }
    }
}
