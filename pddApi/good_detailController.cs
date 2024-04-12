using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Pdd_Models;
using Pdd_Models.Models;
using System.ComponentModel;
using System.Dynamic;
using System.Reflection;
namespace APIOffice.Controllers.pddApi
{
    [ApiController]
    [Description("修改商品")]
    [Route("goods/detail")]
    public class good_detailController : Controller
    {
        [Route("update")]
        public string update([FromBody] List<RequstGoodEditModel> BodyList)
        {
            List<BackMsg> results = new List<BackMsg>();
            foreach (var item in BodyList)
            {
                BackMsg backMsg = new BackMsg();
                if (item.goods_id == 0)
                {
                    backMsg.Code = 1001;
                    backMsg.Mess = "参数错误缺少参数。";
                    results.Add(backMsg);
                    continue;
                }
                Goods_detailModel detaiModel = getmodel(item.goods_id,item.Malls.mall_token);

                switch (item.ApiType)
                {
                    case 0:
                        detaiModel.goods_name= item.goods_name; 
                        break;
                    case 1:
                        detaiModel.tiny_name = item.tiny_name;
                        break;
                    case 2:
                        if (item.shipment_limit_second == 0)
                        {
                            detaiModel.shipment_limit_second = 86400;
                            detaiModel.delivery_one_day = 1;
                        }
                        else 
                            detaiModel.shipment_limit_second= item.shipment_limit_second;
                        break;
                    case 3:
                        detaiModel.sku_list = item.sku_list;
                        break;
                    case 4:
                        detaiModel.cat_id = item.cat_id;
                        break;
                    case 5:
                        detaiModel.cost_template_id = item.cost_template_id;
                        break;
                    case 6:
                        detaiModel.two_pieces_discount = item.two_pieces_discount;
                        break;
                    case 7:
                        detaiModel.buy_limit = item.buy_limit;
                        break;
                    case 8:
                        detaiModel.order_limit = item.order_limit;
                        break;
                    case 9:
                        detaiModel.is_folt=item.is_folt;
                        break;
                    case 10:
                        detaiModel.bad_fruit_claim = item.bad_fruit_claim;
                        break;
                    case 11:
                        detaiModel.goods_desc= item.goods_desc;
                        break;
                    case 12:// 商品轮播图
                        detaiModel.carousel_gallery_list = item.carousel_gallery;
                        break;
                    case 13:// 商品详情图
                        detaiModel.detail_gallery_list = item.detail_gallery;
                        break;
                    case 14:// 
                        detaiModel.sku_list=item.sku_list;
                        detaiModel.outer_goods_id = item.outer_goods_id;
                        break;
                    default:
                        break;
                }
                
                //数据正常请求接口
                Dictionary<string, string> parameters = new Dictionary<string, string>
                {
                    { "type", "pdd.goods.information.update" },
                    { "client_id", apiHelp.client_id },
                    { "access_token", item.Malls.mall_token },
                    { "timestamp", ((DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000/1000).ToString() },
                    { "data_type", "JSON" },
                    //{ "goods_id", item.goods_id.ToString() },

                };

                parameters.Add("carousel_gallery", JsonConvert.SerializeObject(detaiModel.carousel_gallery_list).ToString());
                parameters.Add("detail_gallery", JsonConvert.SerializeObject(detaiModel.detail_gallery_list).ToString());

                foreach (PropertyInfo property in detaiModel.GetType().GetProperties())
                {
                    object value = property.GetValue(detaiModel);

                    List<string> Names = new List<string>() { "oversea_goods", "goods_travel_attr", "goods_trade_attr", "goods_properties", "elec_goods_attributes", "carousel_video", "carousel_gallery_list", "detail_gallery_list" };
                    if (Names.Contains(property.Name))
                        continue;
                    
                    if (property.Name == "sku_list")
                    {
                        var settings = new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore
                        };

                        foreach (var sku in detaiModel.sku_list)
                        {
                            List<long> specIds= new List<long>();
                            foreach (var spe in sku.spec)
                                specIds.Add(spe.spec_id);
                            sku.spec_id_list=JsonConvert.SerializeObject(specIds.ToArray()).ToString();
                        }

                        value = JsonConvert.SerializeObject(detaiModel.sku_list, settings);
                    }

                    if (value != null && value.ToString() != "") // 检查值是否为null  
                    {
                        //某些参数类型变化
                        List<string> TypeChanged = new List<string>() { "is_customs", "is_folt", "is_pre_sale", "is_refundable",  "second_hand" };
                        if (TypeChanged.Contains(property.Name))
                            parameters.Add(property.Name, value.ToString() == "1" ? "true" : "false");
                        else
                            parameters.Add(property.Name, value.ToString());
                    }
                }
                backMsg = apiHelp.SendPddApi(parameters);
                //apiHelp.setError("修改成功:" + JsonConvert.SerializeObject(backMsg));
                //修改成功后提交溯源接口

                if (backMsg.Code == 0)
                {
                    detaiModel.access_token= item.Malls.mall_token;
                    string backMsg1 = apiHelp.postApi("http://112.124.0.204:2356/jky/uploadTraceability", new Dictionary<string, string>(), 1, detaiModel);
                    //apiHelp.setError("溯源失败:" + backMsg1+JsonConvert.SerializeObject(detaiModel));
                    JToken jToken = JsonConvert.DeserializeObject<JToken>(backMsg1);
                    if (jToken["code"].ToString() != "200")
                    {
                        backMsg.Code = MyConvert.ToInt(jToken["code"].ToString());
                        backMsg.Mess = "溯源失败！" + jToken["msg"].ToString();
                        apiHelp.setError("溯源失败:"+backMsg1);
                    }
                }
                results.Add(backMsg);
            }

            var json = JsonConvert.SerializeObject(results);
            return json;
        }

        [Route("List")]
        public string List([FromBody] List<RequstGoodDetail> Goods)
        {
            BackMsg results = new BackMsg();
            List<Goods_detailModel> models = new List<Goods_detailModel>(Goods.Count);


            Parallel.For(0, Goods.Count, i =>
            {
                try
                {
                    Goods_detailModel model = getmodel(Goods[i].Goods, Goods[i].mall.mall_token);
                    model.mall = Goods[i].mall;
                    models[i]= model;
                }
                catch (Exception ex)
                {
                }
            });
            foreach (var item in Goods)
            {
                Goods_detailModel model = getmodel(item.Goods, item.mall.mall_token);
                model.mall = item.mall;
                models.Add(model);
            }
            results.Mess=JsonConvert.SerializeObject(models);
            var json = JsonConvert.SerializeObject(results);
            return json;
        }

        public Goods_detailModel getmodel(long goodsID,string access_token)
        {
            Goods_detailModel model = new Goods_detailModel();
            //根据商品id获取商品明细
            Dictionary<string, string> parameters = new Dictionary<string, string>
                {
                    { "type", "pdd.goods.detail.get" },
                    { "client_id", apiHelp.client_id },
                    { "access_token", access_token },
                    { "timestamp", ((DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000/1000).ToString() },
                    { "data_type", "JSON" },
                    { "goods_id", goodsID.ToString() },

                };
            BackMsg backMsg = apiHelp.SendPddApi(parameters);
            if (backMsg.Code == 0)
            {
                JToken token = JsonConvert.DeserializeObject<JToken>(backMsg.Mess);
                var json = JsonConvert.SerializeObject(token["goods_detail_get_response"]);
                model = JsonConvert.DeserializeObject<Goods_detailModel>(json);
            }

            return model;
        }

       


        
    }
}
