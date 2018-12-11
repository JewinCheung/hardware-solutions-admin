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
        [Route("AddSolutions")]
        public async Task<AjaxResult> AddSolutions(string id, List<Hard_GroupByProLine> obj)
        {
            string ProLineNo = id;

            //删除旧数据
            List<Hard_GroupByProLine> delList = BLLSession.IHard_GroupByProLineBLL.Query(u => u.ProLineNo.Equals(id)).ToList();

            BLLSession.DbSession.Hard_GroupByProLineRepository.DeleteEntities(delList);
            //新增申请明细
            List<Hard_GroupByProLine> applyDetails = obj;
            applyDetails.ForEach(u =>
            {
                u.SerialNo = Guid.NewGuid().ToString();
                u.CreateTime = DateTime.Now;
                u.CreateUser = CurUser.userId;

            });

            await BLLSession.DbSession.Hard_GroupByProLineRepository.AddEntitiesAsync(applyDetails);


            var result =  BLLSession.DbSession.SaveChanges() > 0 ? "" : "数据库操作失败";

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
