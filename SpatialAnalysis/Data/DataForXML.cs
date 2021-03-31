using SpatialAnalysis.Service.AddRecordPatter;

namespace SpatialAnalysis.Data
{
    class DataForXML
    {
        public static void AddPostfix()
        {
            FileCount.FilePostfix(FileCount.FileType.file, new string[] { ".pdf", ".doc", ".xls", "ppt", ".docx", ".xlsx", ".pptx", ".wps", ".et", "pds" });
            FileCount.FilePostfix(FileCount.FileType.picture, new string[] { ".gif", ".jpg", ".jpeg", ".pne" });
            FileCount.FilePostfix(FileCount.FileType.video, new string[] { ".avi", ".mp4", ".mkv", ".webm", ".flv", ".mov", ".wmv", ".3gp", ".3g2", ".mpg" });
            FileCount.FilePostfix(FileCount.FileType.project, new string[] { ".psd", ".ai", ".prproj", ".fla" });
            FileCount.FilePostfix(FileCount.FileType.dll, new string[] { ".exe", ".dll", ".jar", ".pyd" , ".js"});
            FileCount.FilePostfix(FileCount.FileType.txt, new string[] { ".txt", ".chm", ".html", ".json", ".xml", ".config", "ini", ".css" });
            FileCount.FilePostfix(FileCount.FileType.data, new string[] { ".db", ".dat", ".zip", ".rar" });
        }
    }
}
