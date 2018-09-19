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
        public async Task<AjaxResult> AddProLine(dynamic obj)
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
        [Route("SaveRecord/{id}")]
        public async Task<AjaxResult> SaveRecord(string id, dynamic obj)
        {
            string resMsg = string.Empty;

            if (string.IsNullOrEmpty(id))
            {
                return AjaxResult.Cancel("该单据不存在");
            }
            var disp = await BLLSession.ICService_DispatchBLL.Query(u => u.SerialNo.Equals(id)).FirstOrDefaultAsync();
            var record = await BLLSession.ICService_RecordBLL.Query(u => u.SerialNo.Equals(disp.RecordNo)).FirstOrDefaultAsync();
            if (disp == null)
            {
                return AjaxResult.Cancel("该派工单不存在");
            }

            if (obj == null)
            {
                return AjaxResult.Cancel("传递的对象为空");
            }
            //更新主表记录
            record.Category = Convert.ToString(obj.Category);
            record.ContactUser = Convert.ToString(obj.ContactUser);
            record.HoptalId = Convert.ToInt32(obj.HospId);
            record.ProblemDesc = Convert.ToString(obj.ProblemDesc);
            record.Solution = Convert.ToString(obj.Solution);
            record.ProcessUser = CurUser.userId;
            record.ProductIds = Convert.ToString(obj.ProductIds);
            record.ReceiveTime = Convert.ToDateTime(obj.ReceiveTime);
            record.LastTime = DateTime.Now;
            record.LastUser = CurUser.userId;
            //更新派工单信息
            disp.CompleteTime = Convert.ToDateTime(obj.CompleteTime);
            disp.ExecMethod = Convert.ToString(obj.ExecMethod) == "现场处理" ? "CLFS0002" : "CLFS0001";
            disp.Solution = Convert.ToString(obj.Solution);
            disp.LastUser = CurUser.userId;
            disp.LastTime = DateTime.Now;

            //检查是否重新上传附件了,如果是就重新上传
            string serverId = Convert.ToString(obj.serverId);
            if (!string.IsNullOrEmpty(serverId))
            {
                //将附件保存到服务器
                Sys_Attachment attachEntity = await ContextHelper.Current.SaveImgFile("CService", serverId, record.SerialNo, CurUser);
                record.FileAttach = attachEntity.SerialNo;
            }

            BLLSession.DbSession.CService_DispatchRepository.UpdateEntity(disp);
            BLLSession.DbSession.CService_RecordRepository.UpdateEntity(record);

            try
            {
                resMsg = await BLLSession.DbSession.SaveChangesAsync() > 0 ? "更新成功" : "更新数据库失败";
                return AjaxResult.OK(resMsg);
            }
            catch (Exception ex)
            {
                Common.LogHelper.WriteLog("WebApi CService", "AddRecord", "操作人:" + CurUser.userId + "-" + ex.Message.ToString());
                resMsg = ex.Message.ToString();
                return AjaxResult.Cancel(resMsg);
            }

        }

        /// <summary>
        /// 获取个人的记录信息
        /// </summary>
        /// <param name="id">类别 0是带处理  4是流程中 2是完结</param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetRecords/{id}")]
        public async Task<List<Record>> GetRecords(int id)
        {
            int userId = CurUser.userId;
            var data = from a in BLLSession.ICService_DispatchBLL.Query(u => u.ProcessUser.Equals(userId) && u.FormType.Equals(id))
                       join b in BLLSession.ICService_RecordBLL.Query(u => true)
                       on a.RecordNo equals b.SerialNo
                       join d in BLLSession.ICService_HosptialBLL.Query(u => true)
                       on b.HoptalId equals d.HospId
                       orderby a.CreateTime descending
                       select new Record
                       {
                           HospName = d.HospName,
                           DispNo = a.SerialNo,
                           RecordNo = b.SerialNo,
                           ReceiveDate = b.ReceiveTime
                       };
            return await data.ToListAsync<Record>();
        }


        /// <summary>
        /// 获取个人的记录信息
        /// </summary>
        /// <param name="id">派工单单号</param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetRecordByNo/{id}")]
        public async Task<dynamic> GetRecordByNo(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return AjaxResult.Cancel("该单据不存在");
            }
            var entity = from a in BLLSession.ICService_DispatchBLL.Query(u => u.SerialNo.Equals(id))
                         join b in BLLSession.ICService_RecordBLL.Query(u => true)
                         on a.RecordNo equals b.SerialNo
                         join c in BLLSession.ICommon_HospViewBLL.Query(u => true)
                         on b.HoptalId equals c.HospId
                         join d in BLLSession.ISys_AttachmentBLL.Query(u => true)
                         on b.FileAttach equals d.SerialNo into leftJoin
                         from d in leftJoin.DefaultIfEmpty()
                         select new
                         {
                             a.ExecMethod,
                             a.CompleteTime,
                             a.FormType,
                             b.Category,
                             b.ContactTel,
                             b.ContactUser,
                             b.ReceiveTime,
                             b.Solution,
                             b.ProblemDesc,
                             b.ProductIds,
                             b.HoptalId,
                             c.HospName,
                             FileUrl = d == null ? "" : d.FilePath
                         };
            return await entity.FirstOrDefaultAsync();
        }

        /// <summary>
        /// 通过医院获取对应的联系人
        /// </summary>
        /// <param name="id">医院编号</param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetContactUsers/{id}")]
        public async Task<List<string>> GetContactUsers(int id)
        {
            var data = from a in BLLSession.ICService_ContractUserBLL.Query(u => u.HospId.Equals(id))
                       select a.ContractUser;
            return await data.ToListAsync<string>();
        }
    }
}
