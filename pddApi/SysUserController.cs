using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Pdd_Models;
using Pdd_Models.Models;
using System.ComponentModel;
using System.Data;

namespace APIOffice.Controllers.pddApi
{
    [ApiController]
    [Description("登录")]
    [Route("sys/user")]
    public class SysUserController : Controller
    {
        [Route("login")]
        public string login([FromForm]string Name, [FromForm] string Psw)
        {
            BackData backData = new BackData();

            try
            {
                System.Data.SqlClient.SqlParameter[] sqlParameter = new System.Data.SqlClient.SqlParameter[] {
                    new System.Data.SqlClient.SqlParameter("@OutMess", System.Data.SqlDbType.NVarChar,50),
                    new System.Data.SqlClient.SqlParameter("@OutCode", System.Data.SqlDbType.Int),
                    new System.Data.SqlClient.SqlParameter("@UserName", Name),
                    new System.Data.SqlClient.SqlParameter("@Psw", Psw),
                };
                sqlParameter[0].Direction = System.Data.ParameterDirection.Output;
                sqlParameter[1].Direction = System.Data.ParameterDirection.Output;
                DataTable Dt = SQLHelper.ExecuteDataSetProducts("[dbo].[LoginUser]", sqlParameter).Tables[0];
                List<object> ts = new List<object>();
                if (Dt.Rows.Count > 0)
                {
                    LoginUser user = new LoginUser();
                    user.UserId = MyConvert.ToInt(Dt.Rows[0]["user_id"]);
                    user.user_name = Dt.Rows[0]["user_name"].ToString();
                    //user.user_psw = Dt.Rows[0]["user_psw"].ToString();
                    user.user_desc = Dt.Rows[0]["user_desc"].ToString();
                    user.user_expire = Dt.Rows[0]["user_expire"].ToString();
                    user.user_Phone = Dt.Rows[0]["user_Phone"].ToString();
                    ts.Add(user);
                    //if (DateTime.Now > MyConvert.StampToTime(user.user_expire))
                    //{
                    //    //已过期
                    //}
                    backData.Code=0;
                    backData.Data=ts;
                }
                else
                {
                    backData.Code = 101;
                    backData.Mess = "账号密码错误";
                }
                
            }
            catch(Exception ex) { 
                
            }
            finally { }
            return JsonConvert.SerializeObject(backData);
        }

        [Route("sign")]
        public string sign([FromBody] LoginUser model)
        {
            BackMsg backData = new BackMsg();

            try
            {
                System.Data.SqlClient.SqlParameter[] sqlParameter = new System.Data.SqlClient.SqlParameter[] {
                    new System.Data.SqlClient.SqlParameter("@OutMess", System.Data.SqlDbType.NVarChar,50),
                    new System.Data.SqlClient.SqlParameter("@OutCode", System.Data.SqlDbType.Int),
                    new System.Data.SqlClient.SqlParameter("@UserName", model.user_name),
                    new System.Data.SqlClient.SqlParameter("@Psw", model.user_psw),
                    new System.Data.SqlClient.SqlParameter("@Expire", model.user_expire),
                    new System.Data.SqlClient.SqlParameter("@Phone", model.user_Phone),
                };
                sqlParameter[0].Direction = System.Data.ParameterDirection.Output;
                sqlParameter[1].Direction = System.Data.ParameterDirection.Output;
                SQLHelper.ExecuteDataSetProducts("[dbo].[SignUser]", sqlParameter);
                backData.Code = MyConvert.ToInt(sqlParameter[1].Value);
                backData.Mess = sqlParameter[0].Value.ToString();

            }
            catch (Exception ex)
            {

            }
            finally { }
            return JsonConvert.SerializeObject(backData);
        }
        [Route("Bslogin")]
        public string Bslogin([FromBody] LoginUser model)
        {
            BackData backData = new BackData();
            try
            {
                System.Data.SqlClient.SqlParameter[] sqlParameter = new System.Data.SqlClient.SqlParameter[] {
                    new System.Data.SqlClient.SqlParameter("@OutMess", System.Data.SqlDbType.NVarChar,50),
                    new System.Data.SqlClient.SqlParameter("@OutCode", System.Data.SqlDbType.Int),
                    new System.Data.SqlClient.SqlParameter("@UserName", model.user_name),
                    new System.Data.SqlClient.SqlParameter("@Psw", model.user_psw),
                    new System.Data.SqlClient.SqlParameter("@Expire", model.user_expire),
                    new System.Data.SqlClient.SqlParameter("@Phone", model.user_Phone),
                };
                sqlParameter[0].Direction = System.Data.ParameterDirection.Output;
                sqlParameter[1].Direction = System.Data.ParameterDirection.Output;
                DataTable Dt = SQLHelper.ExecuteDataSetProducts("[dbo].[BS_LoginUser]", sqlParameter).Tables[0];
                List<object> ts = new List<object>();
                if (Dt.Rows.Count > 0)
                {
                    LoginUser user = new LoginUser();
                    user.UserId = MyConvert.ToInt(Dt.Rows[0]["user_id"]);
                    user.user_name = Dt.Rows[0]["user_name"].ToString();
                    user.user_desc = Dt.Rows[0]["user_desc"].ToString();
                    user.user_expire = Dt.Rows[0]["user_expire"].ToString();
                    user.user_Phone = Dt.Rows[0]["user_Phone"].ToString();
                    ts.Add(user);
                    backData.Code = 0;
                    backData.Data = ts;
                }
            }
            catch (Exception ex)
            {

            }
            finally { }
            return JsonConvert.SerializeObject(backData);
        }
    }
}
