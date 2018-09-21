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
    [RoutePrefix("Api/HardType")]
    public class HardTypeController : BaseApiController
    {
        /// <summary>
        /// 保存硬件类别信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("SaveHardType")]
        public async Task<AjaxResult> SaveHardType(Hard_MaterialType obj)
        {

            var SerialNo = obj.SerialNo;
            var ParentNo = obj.ParentNo;
            var MatTypeNo = obj.MatTypeNo;
            int isOk;
            Hard_MaterialType entity = new Hard_MaterialType();
            //新增
            if (string.IsNullOrEmpty(SerialNo))
            {
                var p_matObj = BLLSession.IHard_MaterialTypeBLL.Query(u => u.MatTypeNo.Equals(MatTypeNo)).FirstOrDefault();
                var c_matObj = BLLSession.IHard_MaterialTypeBLL.Query(u => u.ParentNo.Equals(p_matObj.SerialNo)).OrderByDescending(u => u.MatTypeNo).FirstOrDefault();
                entity.SerialNo = Guid.NewGuid().ToString();
                entity.MatTypeName = obj.MatTypeName;
                entity.MatTypeNo = (c_matObj == null ? p_matObj.MatTypeNo + "01" :
                       (p_matObj.MatTypeNo.Remove(1) == "0" ? "0" : "") + (Convert.ToInt32(c_matObj.MatTypeNo) + 1).ToString());
                entity.ParentNo = p_matObj.SerialNo;
                entity.MatTypeMemo = obj.MatTypeMemo;
                entity.CreateTime = DateTime.Now;
                entity.CreateUser = CurUser.userId;

                isOk = await BLLSession.IHard_MaterialTypeBLL.AddEntityAsync(entity);
                if (isOk < 0)
                    return AjaxResult.Cancel("保存失败！");
            }
            //编辑
            else
            {
                entity = BLLSession.IHard_MaterialTypeBLL.Query(u => u.MatTypeNo.Equals(MatTypeNo)).FirstOrDefault<Hard_MaterialType>();
                entity.MatTypeName = obj.MatTypeName;
                entity.MatTypeNo = obj.MatTypeNo;
                entity.ParentNo = obj.ParentNo;
                entity.MatTypeMemo = obj.MatTypeMemo;
                entity.LastTime = DateTime.Now;
                entity.LastUser = CurUser.userId;
                isOk = await BLLSession.IHard_MaterialTypeBLL.UpdateEntityAsync(entity);
                if (isOk < 0)
                    return AjaxResult.Cancel("修改失败！");
            }

            return AjaxResult.OK("保存成功！", entity);


        }

        /// <summary>
        /// 保存硬件配置信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("SaveTypeConfig")]
        public async Task<AjaxResult> SaveTypeConfig(Hard_MaterialConfig obj)
        {

            var SerialNo = obj.SerialNo;
            var ParentNo = obj.ParentNo;

            int isOk;
            Hard_MaterialConfig entity = new Hard_MaterialConfig();
            //新增
            if (string.IsNullOrEmpty(SerialNo))
            {

                entity.SerialNo = Guid.NewGuid().ToString();
                entity.ItmeName = obj.ItmeName;
                entity.ParentNo = obj.ParentNo;
                entity.IsEnabled = obj.IsEnabled;
                entity.Memo = obj.Memo;
                entity.CreateTime = DateTime.Now;
                entity.CreateUser = CurUser.userId;

                isOk = await BLLSession.IHard_MaterialConfigBLL.AddEntityAsync(entity);
                if (isOk < 0)
                    return AjaxResult.Cancel("保存失败！");
            }
            //编辑
            else
            {
                entity = BLLSession.IHard_MaterialConfigBLL.Query(u => u.SerialNo.Equals(SerialNo)).FirstOrDefault<Hard_MaterialConfig>();
                entity.ItmeName = obj.ItmeName;
                entity.ParentNo = obj.ParentNo;
                entity.IsEnabled = obj.IsEnabled;
                entity.Memo = obj.Memo;
                entity.LastTime = DateTime.Now;
                entity.LastUser = CurUser.userId;
                isOk = await BLLSession.IHard_MaterialConfigBLL.UpdateEntityAsync(entity);
                if (isOk < 0)
                    return AjaxResult.Cancel("修改失败！");
            }

            return AjaxResult.OK("保存成功！", entity);


        }


        [HttpGet]
        [Route("GetHardType/{id}")]
        public AjaxResult GetHardType(string id)
        {
            var matTypeList = BLLSession.IHard_MaterialTypeBLL.Query(u => u.ParentNo.Equals(id)).ToList();
            var data = from a in matTypeList
                       orderby a.MatTypeNo
                       select new
                       {
                           a.SerialNo,
                           a.MatTypeNo,
                           a.MatTypeName,
                           a.ParentNo,
                           a.MatTypeMemo,
                           smallType = GetSmallType(a.SerialNo)

                       };

            return AjaxResult.OK("", data);
            //return await data.ToListAsync<dynamic>();
        }
        private List<Hard_MaterialType> GetSmallType(string SerialNo)
        {
            var matTypeList = BLLSession.IHard_MaterialTypeBLL.Query(u => u.ParentNo.Equals(SerialNo)).ToList();
            var obj = (from a in matTypeList
                       orderby a.MatTypeNo
                       select new Hard_MaterialType
                       {
                           SerialNo = a.SerialNo,
                           MatTypeNo = a.MatTypeNo,
                           MatTypeName = a.MatTypeName,
                           ParentNo = a.ParentNo,
                           MatTypeMemo = a.MatTypeMemo
                       });
            return obj.ToList();
        }

        [HttpGet]
        [Route("GetTypeInfo/{id}")]
        public async Task<Hard_MaterialType> GetTypeInfo(string id)
        {

            var data = BLLSession.IHard_MaterialTypeBLL.Query(u => u.SerialNo.Equals(id));

            return await data.FirstOrDefaultAsync();
        }

        [HttpGet]
        [Route("GetConfigInfo/{id}")]
        public async Task<Hard_MaterialConfig> GetConfigInfo(string id)
        {
            var data = BLLSession.IHard_MaterialConfigBLL.Query(u => u.SerialNo.Equals(id));

            return await data.FirstOrDefaultAsync();
        }

        [HttpGet]
        [Route("GeTypeConfig/{id}")]
        public AjaxResult GeTypeConfig(string id)
        {
            var matTypeList = BLLSession.IHard_MaterialConfigBLL.Query(u => u.ParentNo.Equals(id)).ToList();
            var data = from a in matTypeList
                       orderby a.ItmeName
                       select new
                       {
                           a.SerialNo,
                           a.ItmeName,
                           a.ParentNo,
                           a.Memo,
                           a.IsEnabled,
                           ConfigItme = GetConfigItme(a.SerialNo)
                       };
            return AjaxResult.OK("", data);
        }

        private List<Hard_MaterialConfig> GetConfigItme(string SerialNo)
        {
            var matTypeList = BLLSession.IHard_MaterialConfigBLL.Query(u => u.ParentNo.Equals(SerialNo)).ToList();
            var obj = (from a in matTypeList
                       orderby a.ItmeName
                       select new Hard_MaterialConfig
                       {
                           SerialNo = a.SerialNo,
                           ItmeName = a.ItmeName,
                           ParentNo = a.ParentNo,
                           Memo = a.Memo,
                           IsEnabled = a.IsEnabled,
                       });
            return obj.ToList();
        }
    }
}
