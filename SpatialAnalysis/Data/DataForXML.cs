using SpatialAnalysis.Service.AddRecordPatter;

namespace SpatialAnalysis.Data
{
    class DataForXML
    {
        public static void AddPostfix()
        {
            FileCount.FilePostfix(FileCount.FileType.file, new string[] { ".pdf", ".doc", ".xls", "ppt", ".docx", ".xlsx", ".pptx", ".wps", ".et", "pds" });
            FileCount.FilePostfix(FileCount.FileType.picture, new string[] { ".gif", ".jpg", ".jpeg", ".png" });
            FileCount.FilePostfix(FileCount.FileType.video, new string[] { ".avi", ".mp3", ".mp4", ".mkv", ".webm", ".flv", ".mov", ".wmv", ".3gp", ".3g2", ".mpg" });
            FileCount.FilePostfix(FileCount.FileType.project, new string[] { ".psd", ".ai", ".prproj", ".fla" });
            FileCount.FilePostfix(FileCount.FileType.zip, new string[] { ".zip", ".rar", ".7z"});
            FileCount.FilePostfix(FileCount.FileType.dll, new string[] { ".exe", ".dll", ".jar", ".php", ".tmp", ".bin" });
            FileCount.FilePostfix(FileCount.FileType.txt, new string[] { ".txt", ".chm", ".html", ".js", ".json", ".xml", ".config", ".ini", ".css", ".manifest", ".java", ".pyd", ".pyc", ".py"});
            FileCount.FilePostfix(FileCount.FileType.data, new string[] { ".db", ".dat", ".wem" });
        }
    }
}
