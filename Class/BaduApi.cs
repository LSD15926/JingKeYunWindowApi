using Nancy;
using Nancy.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Text;

namespace CJOffice.Class
{
    public class BaduApi
    {

        private string corpid = "wwc47c385d4675fbcb", corpsecret = "iZMv2bWMywuW87pQ37WYUWc8JJaJ3TPf2CkBacZ139k", access_token = "";

        private void GetToken()
        {
            string url = "https://qyapi.weixin.qq.com/cgi-bin/gettoken";
            string param = "corpid=" + corpid + "&corpsecret=" + corpsecret;
            string returnMsg = requestGet(url, param);
            JToken token = JObject.Parse(returnMsg);
            if (token["errcode"].ToString() == "0")
            {
                access_token = token["access_token"].ToString();
            }

        }

        public string SendMsg(string Msg)
        {
            string RtnMsg = "";
            GetToken();
            if (access_token == "")
            {
                return "";
            }
            string url = "https://qyapi.weixin.qq.com/cgi-bin/message/send?access_token="+access_token;
            RtnMsg = requestPostJ(url, Msg);


            return RtnMsg;
        }

        public string requestGet(string strUrl, string param)
        {
            Console.WriteLine("HTTP的GET请求:" + strUrl + "?" + param);

            HttpWebRequest httpWebRequest = WebRequest.Create(strUrl + "?" + param) as HttpWebRequest;
            httpWebRequest.Method = "GET";      //指定允许数据发送的请求的一个协议方法
            httpWebRequest.ContentType = "application/x-www-form-urlencoded";       //设置 ContentType 属性设置为适当的值
            WebResponse webResponse = httpWebRequest.GetResponse() as HttpWebResponse;        //发起请求,得到返回对象
            Stream dataStream = webResponse.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream, Encoding.UTF8);
            string returnStr = reader.ReadToEnd();
            // Clean up the streams and the response.
            reader.Close();
            webResponse.Close();
            return returnStr;
        }

        public string requestPost(string strUrl, string param)
        {
            Console.WriteLine("HTTP的POST请求:" + strUrl + ";数据:" + param);

            HttpWebRequest httpWebRequest = WebRequest.Create(strUrl) as HttpWebRequest;
            httpWebRequest.Method = "POST";      //指定允许数据发送的请求的一个协议方法
            httpWebRequest.ContentType = "application/x-www-form-urlencoded";       //设置 ContentType 属性设置为适当的值
            byte[] data = Encoding.UTF8.GetBytes(param);
            using (Stream stream = httpWebRequest.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);     //写入数据
            }
            WebResponse webResponse = httpWebRequest.GetResponse() as HttpWebResponse;        //发起请求,得到返回对象
            Stream dataStream = webResponse.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream, Encoding.UTF8);
            string returnStr = reader.ReadToEnd();
            // Clean up the streams and the response.
            reader.Close();
            webResponse.Close();
            return returnStr;
        }
        public string requestPostJson(string strUrl, string param)
        {
            Console.WriteLine("HTTP的POST请求:" + strUrl + ";数据:" + param);

            HttpWebRequest httpWebRequest = WebRequest.Create(strUrl) as HttpWebRequest;
            httpWebRequest.Method = "POST";      //指定允许数据发送的请求的一个协议方法
            httpWebRequest.ContentType = "application/json";      //设置 ContentType 属性设置为适当的值
            byte[] data = Encoding.UTF8.GetBytes(param);
            using (Stream stream = httpWebRequest.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);     //写入数据
            }
            WebResponse webResponse = httpWebRequest.GetResponse() as HttpWebResponse;        //发起请求,得到返回对象
            Stream dataStream = webResponse.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream, Encoding.UTF8);
            string returnStr = reader.ReadToEnd();
            // Clean up the streams and the response.
            reader.Close();
            webResponse.Close();
            return returnStr;
        }

        public  string requestPostJ(string url,string json)
        {
            //cj111|cj065|cj201|cj093
            var client = new HttpClient();
            var content = new StringContent("{\r\n   \"touser\" : \"cj111|cj065|cj201|cj093\",\r\n   \"msgtype\" : \"text\",\r\n   \"agentid\" : 1000023,\r\n   \"text\" : {\r\n       \"content\" : \"" + json+"\"\r\n   },\r\n   \"safe\":0,\r\n   \"enable_id_trans\": 0,\r\n   \"enable_duplicate_check\": 0,\r\n   \"duplicate_check_interval\": 1800\r\n}", null, "application/json");
            var response = client.PostAsync(url, content).Result;
            if (response.IsSuccessStatusCode)
            {
                string responseBody = response.Content.ReadAsStringAsync().Result;
                return responseBody;
            }
            return "";
        }
    }

}
