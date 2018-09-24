using Pmis.Common;
using Pmis.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
namespace Pmis.Web.Api
{
    [RoutePrefix("Api/MaterialItem")]
    public class MaterialItemController : BaseApiController
    {
        [HttpPost]
        [Route("SaveItem")]
        public async Task<AjaxResult> SaveItem(Hard_MaterialItem obj)
        {
            var SerialNo = obj.SerialNo;
            var MatTypeNo = obj.MatTypeNo;
            var SubItemNo= obj.SubItemNo;
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
                entity.ConfigDesc = obj.ConfigDesc;
                entity.WarrantyPeriod = obj.WarrantyPeriod;
                entity.warrantyType = obj.warrantyType;
                entity.WarrantyCost = obj.WarrantyCost;
                entity.AttachFiles = obj.AttachFiles;
                entity.IsEnergy = obj.IsEnergy;
                entity.IsStop = obj.IsStop;
                entity.PurchasePrice = obj.PurchasePrice;
                entity.Offer = obj.Offer;
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
                entity.ConfigDesc = obj.ConfigDesc;
                entity.WarrantyPeriod = obj.WarrantyPeriod;
                entity.warrantyType = obj.warrantyType;
                entity.WarrantyCost = obj.WarrantyCost;
                entity.AttachFiles = obj.AttachFiles;
                entity.IsEnergy = obj.IsEnergy;
                entity.IsStop = obj.IsStop;
                entity.PurchasePrice = obj.PurchasePrice;
                entity.Offer = obj.Offer;
                entity.LastTime = DateTime.Now;
                entity.LastUser = CurUser.userId;
                isOk = await BLLSession.IHard_MaterialItemBLL.UpdateEntityAsync(entity);
                if (isOk < 0)
                    return AjaxResult.Cancel("修改失败！");
            }

            return AjaxResult.OK("保存成功！", entity);
        }




        [HttpGet]
        [Route("GetItem")]
        public async Task<AjaxResult> GetItemData([FromUri]PageResult page)
        {
            //获取供应商的代码账号
            var data =  from a in BLLSession.IHard_MaterialItemBLL.Query(u =>true)
                             join b in BLLSession.IHard_MaterialTypeBLL.Query( u => true)
                             on a.MatTypeNo equals b.MatTypeNo
                             join c in BLLSession.IHard_MaterialTypeBLL.Query(u => true)
                            on a.SubItemNo equals c.MatTypeNo
                             orderby a.CreateTime descending
                             select new
                             {
                               a.SerialNo,
                               a.MatTypeNo,
                               b.MatTypeName,
                               a.SubItemNo,
                              SubType=c.MatTypeName,
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
                               a.CreateTime,
                             };
            //查询条件
            //data = supplyList.WhereIf(!string.IsNullOrEmpty(page.QueryKey), u => u.PoNo.Contains(page.QueryKey)); new LinqPage(page.PageIndex, page.PageSize, page.Order, page.Sort)
            return AjaxResult.ToJSON( new { total = data.Count(), rows =await data.PageResult(GetLinqPage(page.PageIndex, page.PageSize, "CreateTime", "desc")).ToListAsync<dynamic>()});

        }
    }
}
