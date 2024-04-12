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
    [Route("mall/group")]
    public class MallgroupController : Controller
    {
        [Route("list")]
        public string list([FromForm] int UserId)
        {

            BackData backData = new BackData();
            try
            {
                DataTable dt = SQLHelper.GetDataTable1("select * from u_mall_group where  group_user=" + UserId);
                List<object> list = new List<object>();
                if (dt != null)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        MallGroup model = new MallGroup();
                        model.group_id = MyConvert.ToInt(dt.Rows[i]["group_id"]);
                        model.group_name = dt.Rows[i]["group_name"].ToString();
                        model.group_user = MyConvert.ToInt(dt.Rows[i]["group_user"]);
                        model.group_notes = dt.Rows[i]["group_notes"].ToString();
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

        [Route("Add")]
        public string Add([FromBody] MallGroup model)
        {
            BackMsg backData = new BackMsg();
            try
            {
                System.Data.SqlClient.SqlParameter[] sqlParameter = new System.Data.SqlClient.SqlParameter[] {
                    new System.Data.SqlClient.SqlParameter("@OutMess", System.Data.SqlDbType.NVarChar,50),
                    new System.Data.SqlClient.SqlParameter("@OutCode", System.Data.SqlDbType.Int),
                    new System.Data.SqlClient.SqlParameter("@group_name", model.group_name),
                    new System.Data.SqlClient.SqlParameter("@group_notes", model.group_notes),
                    new System.Data.SqlClient.SqlParameter("@group_user", model.group_user),

                };
                sqlParameter[0].Direction = System.Data.ParameterDirection.Output;
                sqlParameter[1].Direction = System.Data.ParameterDirection.Output;
                SQLHelper.ExecuteDataSetProducts("[dbo].[MallGroupAdd]", sqlParameter);
                backData.Code = MyConvert.ToInt(sqlParameter[1].Value);
                backData.Mess = sqlParameter[0].Value.ToString();
            }
            catch (Exception ex)
            {
            }
            finally { }
            return JsonConvert.SerializeObject(backData);
        }

        [Route("Edit")]
        public string Edit([FromBody] MallGroup model)
        {

            BackMsg backData = new BackMsg();
            try
            {
                
                backData.Mess = SQLHelper.ExecuteCommand1(string.Format("update u_mall_group set group_name='{0}',group_notes='{1}' where group_id={2} "
                    , model.group_name, model.group_notes, model.group_id));

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


        [Route("Del")]
        public string Del([FromBody] MallGroup model)
        {
            BackMsg backData = new BackMsg();
            try
            {
                backData.Mess = SQLHelper.ExecuteCommand1(string.Format("delete u_mall_group  where group_id={0} "
                    ,model.group_id));

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


    }
}
