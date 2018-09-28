using Pmis.Common;
using Pmis.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace Pmis.Web.Api
{
    [RoutePrefix("Api/MaterialItem")]
    public class MaterialItemController : BaseApiController
    {
        /// <summary>
        /// 保存硬件信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("SaveItem")]
        public async Task<AjaxResult> SaveItem(Hard_MaterialItem obj)
        {
            var SerialNo = obj.SerialNo;
            var MatTypeNo = obj.MatTypeNo;
            var SubItemNo = obj.SubItemNo;
            int isOk;
            Hard_MaterialItem entity = new Hard_MaterialItem();
            //新增
            if (string.IsNullOrEmpty(SerialNo))
            {
                var p_matObj = BLLSession.IHard_MaterialTypeBLL.Query(u => u.MatTypeNo.Equals(SubItemNo)).FirstOrDefault();
                var c_matObj = BLLSession.IHard_MaterialItemBLL.Query(u => u.ItemNo.Equals(p_matObj.SerialNo)).OrderByDescending(u => u.MatTypeNo).FirstOrDefault();
                entity.SerialNo = Guid.NewGuid().ToString();
                entity.ItemNo = (c_matObj == null ? p_matObj.MatTypeNo + "0001" : (Convert.ToInt32(c_matObj.ItemNo) + 1).ToString());
                entity.MatTypeNo = obj.MatTypeNo;
                entity.SubItemNo = obj.SubItemNo;
                entity.Brand = obj.Brand;
                entity.ItemModel = obj.ItemModel;
                entity.WarrantyPeriod = obj.WarrantyPeriod;
                entity.warrantyType = obj.warrantyType;
                entity.WarrantyCost = obj.WarrantyCost;
                entity.AttachFiles = obj.AttachFiles;
                entity.IsEnergy = obj.IsEnergy;
                entity.IsStop = obj.IsStop;
                entity.PurchasePrice = obj.PurchasePrice;
                entity.Offer = obj.Offer;
                entity.DeliveryDay = obj.DeliveryDay;
                entity.CreateTime = DateTime.Now;
                entity.CreateUser = CurUser.userId;

                isOk = await BLLSession.IHard_MaterialItemBLL.AddEntityAsync(entity);
                if (isOk < 0)
                    return AjaxResult.Cancel("保存失败！");
            }
            //编辑
            else
            {
                entity = BLLSession.IHard_MaterialItemBLL.Query(u => u.SerialNo.Equals(SerialNo)).FirstOrDefault<Hard_MaterialItem>();

                entity.MatTypeNo = obj.MatTypeNo;
                entity.SubItemNo = obj.SubItemNo;
                entity.Brand = obj.Brand;
                entity.ItemModel = obj.ItemModel;
                entity.WarrantyPeriod = obj.WarrantyPeriod;
                entity.warrantyType = obj.warrantyType;
                entity.WarrantyCost = obj.WarrantyCost;
                entity.AttachFiles = obj.AttachFiles;
                entity.IsEnergy = obj.IsEnergy;
                entity.IsStop = obj.IsStop;
                entity.PurchasePrice = obj.PurchasePrice;
                entity.Offer = obj.Offer;
                entity.DeliveryDay = obj.DeliveryDay;
                entity.LastTime = DateTime.Now;
                entity.LastUser = CurUser.userId;
                isOk = await BLLSession.IHard_MaterialItemBLL.UpdateEntityAsync(entity);
                if (isOk < 0)
                    return AjaxResult.Cancel("修改失败！");
            }

            return AjaxResult.OK("保存成功！", entity);
        }

        [HttpPost]
        [Route("SetConfig")]
        public async Task<AjaxResult> SetItemConfig(dynamic obj)
        {
            string SerialNo = obj.SerialNo;
            string ConfigDesc = obj.ConfigDesc;
            int isOk;

            var entity = BLLSession.IHard_MaterialItemBLL.Query(u => u.SerialNo.Equals(SerialNo)).FirstOrDefault<Hard_MaterialItem>();

            if (entity == null)
            {
                return AjaxResult.Cancel("找不到对象！");
            }
            entity.ConfigDesc = obj.ConfigDesc;
            entity.LastTime = DateTime.Now;
            entity.LastUser = CurUser.userId;
            isOk = await BLLSession.IHard_MaterialItemBLL.UpdateEntityAsync(entity);
            if (isOk < 0)
            {
                return AjaxResult.Cancel("配置失败！");
            }
            return AjaxResult.OK("配置成功！", entity);
        }

        /// <summary>
        /// 查询-硬件列表
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetItem")]
        public async Task<AjaxResult> GetItemData([FromUri]PageResult page)
        {

            var data = from a in BLLSession.IHard_MaterialItemBLL.Query(u => true)
                       join b in BLLSession.IHard_MaterialTypeBLL.Query(u => true)
                       on a.MatTypeNo equals b.MatTypeNo
                       join c in BLLSession.IHard_MaterialTypeBLL.Query(u => true)
                      on a.SubItemNo equals c.MatTypeNo
                       orderby a.CreateTime descending
                       select new
                       {
                           a.SerialNo,
                           TypeNo = b.SerialNo,
                           a.MatTypeNo,
                           b.MatTypeName,
                           a.SubItemNo,
                           SubType = c.MatTypeName,
                           a.Brand,
                           a.ItemModel,
                           a.ConfigDesc,
                           a.AttachFiles,
                           a.WarrantyPeriod,
                           a.warrantyType,
                           a.WarrantyCost,
                           a.PurchasePrice,
                           a.Offer,
                           a.IsStop,
                           a.IsEnergy,
                           a.LastTime,
                           a.DeliveryDay,
                           a.CreateTime,
                       };
            if (!string.IsNullOrEmpty(page.QueryKey))
            {
                string[] list = new JavaScriptSerializer().Deserialize<string[]>(page.QueryKey);
                if (list.Length > 0) {
                    data = data.Where(u => list.Contains(u.MatTypeName) || list.Contains(u.SubType) || list.Contains(u.Brand) || list.Contains(u.ItemModel));
                }
                 
            }
          
            //查询条件
            //data = supplyList.WhereIf(!string.IsNullOrEmpty(page.QueryKey), u => u.PoNo.Contains(page.QueryKey)); new LinqPage(page.PageIndex, page.PageSize, page.Order, page.Sort)
            return AjaxResult.ToJSON(new { total = data.Count(), rows = await data.PageResult(GetLinqPage(page.PageIndex, page.PageSize, "CreateTime", "desc")).ToListAsync<dynamic>() });

        }







    }
}
