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
    [RoutePrefix("Api/ProLine")]
    public class ProLineController : BaseApiController
    {
        [HttpPost]
        [Route("AddProLine")]
        public async Task<AjaxResult> AddProLine(Hard_BaseDict obj)
        {
            string SerialNo = Guid.NewGuid().ToString();

            if (obj == null)
            {
                return AjaxResult.Cancel("参数不容许为空");
            }
            string DictNo = obj.DictNo;
            var BaseData = BLLSession.IHard_BaseDictBLL.Query(u => u.DictNo.Equals(DictNo)).FirstOrDefault();
            if (BaseData != null)
            {
                return AjaxResult.Cancel("该产品线编号已存在！");
            }


            Hard_BaseDict basedict = new Hard_BaseDict();
            basedict.SerialNo = SerialNo;
            basedict.DictType = "产品线";
            basedict.DictNo = obj.DictNo;
            basedict.DictName = obj.DictName;
            basedict.DictSort = obj.DictSort;
            basedict.Memo = obj.Memo;
            basedict.IsEnabled = obj.IsEnabled;
            basedict.BelongArea = "Product";
            basedict.DictParam = obj.DictParam;
            basedict.CreateTime = DateTime.Now;
            basedict.CreateUser = CurUser.userId;


            int result = await BLLSession.IHard_BaseDictBLL.AddEntityAsync(basedict);
            if (result >= 0)
            {
                return AjaxResult.OK("保存成功", basedict);
            }
            else
            {
                return AjaxResult.Cancel("保存失败");
            }
        }


        /// <summary>
        /// 编辑记录信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("EditProLine/{id}")]
        public async Task<AjaxResult> EditProLine(string id, dynamic obj)
        {

            if (string.IsNullOrEmpty(id))
            {
                return AjaxResult.Cancel("该单据不存在");
            }
            string DictNo = obj.DictNo;
            var BaseData = BLLSession.IHard_BaseDictBLL.Query(u => u.DictNo.Equals(DictNo) & u.SerialNo != id).FirstOrDefault();
            if (BaseData != null)
            {
                return AjaxResult.Cancel("该产品线编号已存在！");
            }

            var BaseInfo = BLLSession.IHard_BaseDictBLL.Query(u => u.SerialNo.Equals(id)).FirstOrDefault();
            BaseInfo.DictNo = obj.DictNo;
            BaseInfo.DictName = obj.DictName;
            BaseInfo.DictSort = obj.DictSort;
            BaseInfo.Memo = obj.Memo;
            BaseInfo.IsEnabled = obj.IsEnabled;
            BaseInfo.DictParam = obj.DictParam;
            BaseInfo.LastTime = DateTime.Now;
            BaseInfo.LastUser = CurUser.userId;

            int result = await BLLSession.IHard_BaseDictBLL.UpdateEntityAsync(BaseInfo);
            if (result >= 0)
            {
                return AjaxResult.OK("保存成功", BaseInfo);
            }
            else
            {
                return AjaxResult.Cancel("保存失败");
            }


        }

        [HttpGet]
        [Route("GetProLine")]
        public async Task<List<dynamic>> GetProLine()
        {
            var data = BLLSession.IHard_BaseDictBLL.Query(u => u.DictType.Equals("产品线"));
       
            return await data.ToListAsync<dynamic>();
        }
    }
}
