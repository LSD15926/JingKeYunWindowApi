using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;


public class FileWC
{
    /// <summary>
    /// 打开文件并替换
    /// </summary>
    /// <returns></returns>
    public string OAC(string strPath, string str入れかえ)
    {
        string result = "1";
        try
        {
            string strFilePath = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, strPath);
            if (File.Exists(strFilePath))
            {

                string strContent = File.ReadAllText(strFilePath);
                strContent = Regex.Replace(strContent, "//取次ぐ", str入れかえ);
                File.WriteAllText(strFilePath, strContent);
            }
        }
        catch (Exception ex)
        {
            return "0" + ex;

        }
        return result;
    }
    /// <summary>
    /// 移动选择的模板文件到demo中
    /// </summary>
    /// <param name="srcFolder">源地址</param>
    /// <param name="destFolder">目标地址</param>
    public void moveFiles(string srcFolder, string destFolder)
    {
        //如果目录不存在，新建一个
        if (!Directory.Exists(destFolder))
        {
            Directory.CreateDirectory(destFolder);
        }
        DirectoryInfo directoryInfo = new DirectoryInfo(srcFolder);
        FileInfo[] files = directoryInfo.GetFiles();
        DirectoryInfo[] fileDirs = directoryInfo.GetDirectories();

        foreach (FileInfo file in files) // Directory.GetFiles(srcFolder)
        {

            file.CopyTo(Path.Combine(destFolder, file.Name),true);

        }
        foreach (DirectoryInfo fileDir in fileDirs)
        {
            moveFiles(fileDir.FullName, destFolder+"\\"+fileDir.Name);
        }
       
    }

    /// <summary>
    /// 移动单个文件
    /// </summary>
    /// <param name="srcFolder">源文件地址</param>
    /// <param name="destFolder">目标地址</param>
    public void moveFile(string srcFolder, string destFolder)
    {
        //如果目录不存在，新建一个
        if (!Directory.Exists(destFolder))
        {
            Directory.CreateDirectory(destFolder);
        }
        //DirectoryInfo directoryInfo = new DirectoryInfo(srcFolder);
        FileInfo file = new FileInfo(srcFolder);

        file.CopyTo(Path.Combine(destFolder, file.Name), true);


    }
    public void moveFile(string srcFolder, string destFolder, string filename)
    {
        //如果目录不存在，新建一个
        if (!Directory.Exists(destFolder))
        {
            Directory.CreateDirectory(destFolder);
        }
        //DirectoryInfo directoryInfo = new DirectoryInfo(srcFolder);
        FileInfo file = new FileInfo(srcFolder);

        file.CopyTo(Path.Combine(destFolder, filename + file.Extension), true);


    }
    /// <summary>
    /// 压缩文件
    /// </summary>
    /// <returns>返回压缩后的路径</returns>
    public string YaSuo(out bool bo, out TimeSpan times)
    {
        string rarurlPath = string.Empty;
        bo = false;
        //压缩文件
        string yasuoPathSave = "E:\\WCHOT\\Export\\" + DateTime.Now.Year + DateTime.Now.Month + DateTime.Now.Day + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second + ".rar";
        string yasuoPath = @"E:\WCHOT\demo_ljc";
        System.Diagnostics.Process pro = new System.Diagnostics.Process();
        pro.StartInfo.FileName = @"C:\Program Files\WinRAR\WinRAR.exe";//WinRAR所在路径
                                                                       //pro.StartInfo.Arguments = "a " + yasuoPathSave + " " + yasuoPath + " -r ";//dir是你的目录名
        pro.StartInfo.Arguments = string.Format("a {0} {1} -r", yasuoPathSave, yasuoPath);

        pro.Start();
        times = pro.TotalProcessorTime;
        bo = pro.WaitForExit(60000);//设定一分钟
        if (!bo)
            pro.Kill();
        pro.Close();
        pro.Dispose();
        rarurlPath = yasuoPathSave;
        return rarurlPath;
    }

    /// <summary>
    /// 通知浏览器下载文件
    /// </summary>
    /// <param name="filename"></param>
    //public void ResponseFile(string filename)
    //{

    //    FileInfo file = new FileInfo(filename);//创建一个文件对象
    //    System.Web.HttpContext.Current.Response.Clear();//清除所有缓存区的内容
    //    System.Web.HttpContext.Current.Response.Charset = "GB2312";//定义输出字符集
    //    System.Web.HttpContext.Current.Response.ContentEncoding = Encoding.Default;//输出内容的编码为默认编码
    //    System.Web.HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=" + file.Name);
    //    //添加头信息。为“文件下载/另存为”指定默认文件名称
    //    System.Web.HttpContext.Current.Response.AddHeader("Content-Length", file.Length.ToString());
    //    //添加头文件，指定文件的大小，让浏览器显示文件下载的速度
    //    System.Web.HttpContext.Current.Response.WriteFile(file.FullName);// 把文件流发送到客户端
    //    System.Web.HttpContext.Current.Response.End();
    //}

    /// <summary>
    /// 保存使用日志
    /// </summary>
    /// <param name="logName">函数名称</param>
    /// <param name="time">运行时间</param>
    /// <param name="User">角色</param>
    public void saveLog(string logName,string User,string time)
    {
        //TheWingedDragonofRa
        //打开文件，不存在则创建
        //string strFilePath = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory,@"E:\WCHOT\log\"+ DateTime.Now.Year + DateTime.Now.Month + DateTime.Now.Day + ".txt");
        //if (!File.Exists(strFilePath))
        //{
        //    File.Create(strFilePath);
        //}
        //FileStream fs= File.OpenWrite(strFilePath);
        string s = "\r\n时间：" + DateTime.Now+"\t方法：" + logName + "\t用户：" + User + "\t用时：" + time;
        //FileInfo myFile = new FileInfo(@"E:\WCHOT\log\" + DateTime.Now.Year + DateTime.Now.Month + DateTime.Now.Day + ".txt");
        //StreamWriter sw = myFile.CreateText();
        //sw.Write(s);
        //sw.Close();
        File.AppendAllText(@"D:\Log\" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".txt", s);
        //fs.Close();
    }
}

