using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.ComponentModel;
using System.Text;
using static jingkeyun.Class.Config;
using static System.Net.Mime.MediaTypeNames;

namespace APIOffice.Controllers.pddApi
{
    [ApiController]
    [Description("版本更新")]
    [Route("api/Update")]
    public class UpdateController : Controller
    {
        [Route("Version")]
        public string Version()
        {
            //获取服务器版本
            try
            {
                return OperateIniFile.ReadIniData("version", "key", "", "C:\\Update\\Config.ini");
            }
            catch (Exception ex)
            {
                return "";

            }
        }

        [Route("DownUrl")]
        public string DownUrl()
        {
            //获取服务器版本
            try
            {
                string filename = OperateIniFile.ReadIniData("File", "Zip", "", "C:\\Update\\Version.ini");
                return filename;
            }
            catch (Exception ex)
            {
                return "";

            }
        }

    }
}
