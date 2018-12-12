using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Pmis.Model;
using System.Web.Http;
using Pmis.Web.Helper;
using System.Data;
using System.Data.Entity;


namespace Pmis.Web.Api
{
    [RoutePrefix("Api/Solutions")]
    public class SolutionsManageController : BaseApiController
    {
        /// <summary>
        /// 根基产品线获取对应分配硬件列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GroupByProLine/{id}")]
        public async Task<List<dynamic>> GroupByProLine(string id)
        {
            var ProLineNo = id;

            var data = BLLSession.IHard_GroupByProLineBLL.Query(u => u.ProLineNo.Equals(ProLineNo));

            return await data.ToListAsync<dynamic>();
        }

        /// <summary>
        /// 添加方案
        /// </summary>
        /// <param name="id">产品线</param>
        /// <param name="obj">硬件类别</param>
        /// <returns></returns>
        [HttpPost]
        [Route("AddSolutions/{id}")]
        public async Task<AjaxResult> AddSolutions(string id, List<Hard_GroupByProLine> obj)
        {
            string ProLineNo = id;

            //删除旧数据
            List<Hard_GroupByProLine> delList = BLLSession.IHard_GroupByProLineBLL.Query(u => u.ProLineNo.Equals(id)).ToList();

            await BLLSession.DbSession.Hard_GroupByProLineRepository.DelEntitiesAsync(delList);
            //生成新数据
            List<Hard_GroupByProLine> applyDetails = obj;
            applyDetails.ForEach(u =>
            {
                u.SerialNo = Guid.NewGuid().ToString();
                u.CreateTime = DateTime.Now;
                u.CreateUser = CurUser.userId;

            });

            await BLLSession.DbSession.Hard_GroupByProLineRepository.AddEntitiesAsync(applyDetails);


            var result = BLLSession.DbSession.SaveChanges() > 0 ? "" : "数据库操作失败";

            if (string.IsNullOrEmpty(result))
            {
                return AjaxResult.OK("保存成功", obj);
            }
            else
            {
                return AjaxResult.Cancel("保存失败");
            }
        }

        /// <summary>
        /// 获取产品线下硬件类别下硬件列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetItemGroupByProLine")]
        public async Task<List<dynamic>> GetItemGroupByProLine([FromUri]string ProLineNo, [FromUri]string SubItemNo)
        {
            //var SubItemNo = ProLineNo;
            //var SubItemNo = SubItemNo;
            var GroupByItme = BLLSession.IHard_GroupByItmeBLL.Query(u => u.ProLineNo.Equals(ProLineNo));
            var data = from a in BLLSession.IHard_GroupByProLineBLL.Query(u => u.ProLineNo.Equals(ProLineNo) && u.GroupNo.Equals(SubItemNo))
                       join b in BLLSession.IHard_MaterialItemBLL.Query(u => u.IsStop == 0)
                       on a.GroupNo equals b.SubItemNo
                       where !GroupByItme.Any(u => u.ItemNo == b.ItemNo)
                       select new
                       {
                           a.SerialNo,
                           TypeNo = b.SerialNo,
                           b.MatTypeNo,
                           b.SubItemNo,
                           SubType = a.GroupType,
                           b.Brand,
                           b.ItemModel,
                           b.ConfigDesc,
                           b.AttachFiles,
                           b.WarrantyPeriod,
                           b.warrantyType,
                           b.WarrantyCost,
                           b.PurchasePrice,
                           b.Offer,
                           b.IsStop,
                           b.IsEnergy,
                           b.LastTime,
                           b.DeliveryDay,
                           b.CreateTime,
                       };



            return await data.ToListAsync<dynamic>();
        }


        /// <summary>
        /// 添加排除的硬件
        /// </summary>
        /// <param name="id">产品线</param>
        /// <param name="obj">硬件类别</param>
        /// <returns></returns>
        [HttpPost]
        [Route("AddNotGroupByItem/{id}")]
        public async Task<AjaxResult> AddNotGroupByItem(string id, List<Hard_GroupByItme> obj)
        {
            string ProLineNo = id;

            //删除旧数据
            List<Hard_GroupByItme> delList = BLLSession.IHard_GroupByItmeBLL.Query(u => u.ProLineNo.Equals(id)).ToList();

            await BLLSession.DbSession.Hard_GroupByItmeRepository.DelEntitiesAsync(delList);
            //生成新数据
            List<Hard_GroupByItme> applyDetails = obj;
            applyDetails.ForEach(u =>
            {
                u.SerialNo = Guid.NewGuid().ToString();
                u.CreateTime = DateTime.Now;
                u.CreateUser = CurUser.userId;

            });

            await BLLSession.DbSession.Hard_GroupByItmeRepository.AddEntitiesAsync(applyDetails);


            var result = BLLSession.DbSession.SaveChanges() > 0 ? "" : "数据库操作失败";

            if (string.IsNullOrEmpty(result))
            {
                return AjaxResult.OK("保存成功", obj);
            }
            else
            {
                return AjaxResult.Cancel("保存失败");
            }
        }

    }
}
