﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections;

namespace DDTV_WEB_Server.Controllers
{
    /// <summary>
    /// 获取已录制的文件总列表
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class File_GetAllFileList : ProcessingControllerBase.ApiControllerBase
    {   
        [HttpPost(Name = "File_GetAllFileList")]
        public ActionResult post([FromForm] string cmd)
        {
            return Content(MessageBase.Success(nameof(File_GetAllFileList), DDTV_Core.Tool.DownloadList.GetRecFileList()), "application/json");
        }   
    }
    /// <summary>
    /// 根据文件树结构返回已录制的文件总列表
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class File_GetFilePathList : ProcessingControllerBase.ApiControllerBase
    {
        [HttpPost(Name = "File_GetFilePathList")]
        public ActionResult post([FromForm] string cmd)
        {
            string Path = DDTV_Core.SystemAssembly.DownloadModule.Download.DownloadPath;

            List<FileNames> T0 = GetSystemAllPath.GetallDirectory(new List<FileNames>(),Path);
            return Content(MessageBase.Success(nameof(File_GetAllFileList), T0), "application/json");
        }
        public class FileNames
        {
            /// <summary>
            /// 文件名
            /// </summary>
            public string Name { get; set; }
            /// <summary>
            /// 文件类型
            /// </summary>
            public string FileType { get; set; }
            /// <summary>
            /// 文件大小(如果类型是文件夹则为0)
            /// </summary>
            public long Size { get; set; }
            /// <summary>
            /// 文件创建时间
            /// </summary>
            public DateTime DateTime { get; set; }
            /// <summary>
            /// 子文件夹
            /// </summary>
            public List<FileNames> children { get; set; }
        }
        //以上字段为树形控件中需要的属性
        public class GetSystemAllPath : Controller
        {
            //获得指定路径下所有文件名
            public static List<FileNames> getFileName(List<FileNames> list, string filepath)
            {
                DirectoryInfo root = new DirectoryInfo(filepath);
                foreach (FileInfo f in root.GetFiles())
                {
                    list.Add(new FileNames
                    {
                        Name = f.Name,
                        Size=f.Length,
                        DateTime=f.CreationTime,
                        FileType =f.Extension,
                    });
                }
                return list;
            }
            //获得指定路径下的所有子目录名
            // <param name="list">文件列表</param>
            // <param name="path">文件夹路径</param>
            public static List<FileNames> GetallDirectory(List<FileNames> list, string path)
            {
                DirectoryInfo root = new DirectoryInfo(path);
                var dirs = root.GetDirectories();
                if (dirs.Count() !=0)
                {
                    foreach (DirectoryInfo d in dirs)
                    {
                        list.Add(new FileNames
                        {
                            Name = d.Name,
                            FileType = d.Extension,
                            Size=0,
                            children = GetallDirectory(new List<FileNames>(), d.FullName)
                        });
                    }
                }
                list = getFileName(list, path);
                return list;
            }
        }
    }
    /// <summary>
    /// 分类获取已录制的文件总列表
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class File_GetTypeFileList : ProcessingControllerBase.ApiControllerBase
    {
        [HttpPost(Name = "File_GetTypeFileList")]
        public ActionResult post([FromForm] string cmd)
        {
            ArrayList arrayList = DDTV_Core.Tool.DownloadList.GetRecFileList();
           
            TypeFileList.FileList flvList = new() { Type="flv"};
            TypeFileList.FileList mp4List = new() { Type = "mp4" };
            TypeFileList.FileList xmlList = new() { Type = "xml" };
            TypeFileList.FileList csvList = new() { Type = "csv" };
            TypeFileList.FileList otherList = new() { Type = "other" };
            foreach (var item in arrayList)
            {
                string type = item.ToString().Split('.')[item.ToString().Split('.').Length - 1];
                switch(type)
                {
                    case "flv":
                        flvList.files.Add(item.ToString());
                        break;
                    case "mp4":
                        mp4List.files.Add(item.ToString());
                        break;
                    case "xml":
                        xmlList.files.Add(item.ToString());
                        break;
                    case "csv":
                        csvList.files.Add(item.ToString());
                        break;
                    default:
                        otherList.files.Add(item.ToString());
                        break;
                }
            }
            TypeFileList typeFileList = new();
            typeFileList.fileLists.Add(flvList);
            typeFileList.fileLists.Add(mp4List);
            typeFileList.fileLists.Add(xmlList);
            typeFileList.fileLists.Add(csvList);
            typeFileList.fileLists.Add(otherList);
            return Content(MessageBase.Success(nameof(File_GetTypeFileList), typeFileList), "application/json");
        }
    }
    public class TypeFileList
    {
        public List<FileList> fileLists =new List<FileList>();
        public class FileList
        {
            public string Type { set; get; }
            public List<string> files = new List<string>();
        }
    }
    [Route("api/[controller]")]
    [ApiController]
    public class File_GetFile : ProcessingControllerBase.ApiControllerBase
    {
        [HttpGet(Name = "File_GetFile")]
        public ActionResult get(string FileName)
        {
            var _ = DDTV_Core.Tool.DownloadList.GetRecFileList();
            if (_.Contains(FileName))
            {
                try
                {
                    string type = FileName.Split('.')[FileName.Split('.').Length - 1];
                    string Name = FileName.Split('/')[FileName.Split('/').Length - 1];


                    //byte[] bts = new byte[fs.Length];
                    //fs.Read(bts, 0, (int)fs.Length);
                    FileStream fs = null;
                    if (type == "flv" || type == "mp4" || type == "xml" || type == "csv")
                    {
                        fs = new FileStream(FileName, FileMode.Open);
                    }
                    switch (type)
                    {
                        case "flv":
                            return File(fs, "video/mpeg4", Name);
                        case "mp4":
                            return File(fs, "video/mpeg4", Name);
                        case "xml":
                            return File(fs, "application/xml", Name);
                        case "csv":
                            return File(fs, "text/plain", Name);
                        default:
                            return Content(MessageBase.Success(nameof(File_GetFile), "该文件不在支持列表内"), "application/json");

                    }
                }
                catch (Exception e)
                {
                    return Content(MessageBase.Success(nameof(File_GetFile), $"获取文件出现意料之外的错误！错误信息:{e}"), "application/json");
                }
            }
            else
            {
                return Content(MessageBase.Success(nameof(File_GetFile), "该文件不存在"), "application/json");
            }
        }
    }
}
