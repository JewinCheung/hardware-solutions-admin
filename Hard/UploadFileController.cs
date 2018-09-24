using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Pmis.Model;
using System.Web.Http;
using Pmis.Web.Helper;
using System.Data;
using System.Data.Entity;
using System.Net.Http;
using System.Web;
using System.IO;
using System.Collections;

namespace Pmis.Web.Api
{

    public class UploadFileController : ApiController
    {
        public class Resource
        {
            public string Id { get; set; }

            public string Type { get; set; }
        }
        [HttpPost]
        public async Task<AjaxResult> UploadFile()
        {
            List<Resource> resources = new List<Resource>();
            // multipart/form-data
            // 采用MultipartMemoryStreamProvider
            var provider = new MultipartMemoryStreamProvider();

            //读取文件数据
            await Request.Content.ReadAsMultipartAsync(provider);
            var FileUrl = "";
            foreach (var item in provider.Contents)
            {
                // 判断是否是文件
                if (item.Headers.ContentDisposition.FileName != null)
                {
                    //获取到流
                    var ms = item.ReadAsStreamAsync().Result;
                    //进行流操作
                    using (var br = new BinaryReader(ms))
                    {
                        if (ms.Length <= 0)
                            break;
                        //读取文件内容到内存中
                        var data = br.ReadBytes((int)ms.Length);
     
                        //Info
                        FileInfo info = new FileInfo(item.Headers.ContentDisposition.FileName.Replace("\"", ""));
                        Resource resource = new Resource() { Id = DateTime.Now.ToString("yyyyMMddHHmmssffff") };
                        //文件类型
                        var  FileType = info.Extension;

           
                        //文件存储地址
                        string rootPath = "D:\\HardImg";
                        var dirTempPath = rootPath + "\\" + DateTime.Now.ToString("yyyyMMdd") + "\\";
                        string dirPath = Path.Combine(dirTempPath);
                        string FileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + FileType;

                        //Write
                        try
                        {
                         
                            if (!Directory.Exists(dirPath))
                            {
                                Directory.CreateDirectory(dirPath);
                            }

                            File.WriteAllBytes(Path.Combine(dirPath, FileName), data);

                             FileUrl = ContextHelper.Current.GetDocFile(Path.Combine(dirPath, FileName));

                        }
                        catch (Exception exception)
                        {
                            return AjaxResult.Cancel(exception.Message.ToString());
                        }
                    }
                }
            }


            return AjaxResult.OK("上传成功", FileUrl);



















            //string rootPath = "D:\\HardImg";
            //var dirTempPath = rootPath + "\\" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + "\\";
            ////检查文档目录是否存在,如果不存在目录,就新增
            //if (!Directory.Exists(dirTempPath))
            //{
            //    Directory.CreateDirectory(dirTempPath);
            //}
            //// 设置上传目录 
            //var provider = new MyMultipartFormDataStreamProvider(dirTempPath);
            //Dictionary<string, string> dictParam = new Dictionary<string, string>();
            //AjaxResult result = null;

            //if (!Request.Content.IsMimeMultipartContent("form-data"))
            //    return AjaxResult.Cancel("没有要上传的文档", dictParam);
            //var task = await Request.Content.ReadAsMultipartAsync(provider).
            //    ContinueWith<AjaxResult>(o =>
            //    {
            //        //1：读取文件传递的参数
            //        foreach (var key in provider.FormData.AllKeys)
            //        {//接收FormData  
            //            dictParam.Add(key, provider.FormData[key]);
            //        }
            //        var files = provider.FileData;
            //        if (files.Count() == 0)
            //        {
            //            return AjaxResult.OK("没有文件", dictParam);
            //        }
            //        var file = files[0];
            //        // 2：设置最大文件大小

            //        // 4：获取文件名称
            //        string orfilename = file.Headers.ContentDisposition.FileName.TrimStart('"').TrimEnd('"');
            //         var fileinfo = new FileInfo(file.LocalFileName);
            //        var info = new
            //        {
            //            orfilename = orfilename,
            //            LocalFileName = file.LocalFileName
            //        };

            //        if (fileinfo.Length <= 0)
            //        {
            //            result = AjaxResult.Cancel("请选择上传文件");
            //        }
            //        else
            //        {

            //            result = AjaxResult.OK("上传成功", info);
            //        }
            //        return result;
            //    });

            //return task;

        }
    }
}
