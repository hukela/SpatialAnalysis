using SpatialAnalysis.Entity;
using SpatialAnalysis.Mapper;
using SpatialAnalysis.Utils;
using System.Threading;

namespace SpatialAnalysis.Service
{
    //提供标签支持
    class TagSupport
    {
        public static object Thrend { get; private set; }

        /// <summary>
        /// 设置或刷新标签标注的数据
        /// </summary>
        public static void SetTagSort()
        {
            Thread thrend = new Thread(SetTagSortAsyn);
            thrend.Start();
        }
        public static void SetTagSortAsyn()
        {
            DirTagBean[] beans = DirTagMapper.GetAll();
            //根据标注长度进行排序
            DirTagBean bean = null;
            int count = beans.Length;
            for (int r = 0; r < count; r++)
            {
                for (int i = 0; i < count - 1; i++)
                {
                    string a = beans[i].Path;
                    string b = beans[i + 1].Path;
                    if (a.Length < b.Length)
                    {
                        bean = beans[i];
                        beans[i] = beans[i + 1];
                        beans[i + 1] = bean;
                    }
                }
            }
            InternalStorage.Build(InternalStorage.Domain.tag);
            InternalStorage.Set(InternalStorage.Domain.tag, "tagSort", beans);
        }
        /// <summary>
        /// 当内存中没有数据时，放入数据
        /// </summary>
        public static void CheckTagSort()
        {
            object obj = InternalStorage.Get(InternalStorage.Domain.tag, "tagSort");
            if (obj == null)
                SetTagSort();
        }
        /// <summary>
        /// 通过路径获取标签bean
        /// </summary>
        /// <param name="isThis">该路径是否是标签所标注的路径</param>
        public static TagBean GetTagByPath(string path, out bool isThis)
        {
            DirTagBean[] dirTags = InternalStorage.Get(InternalStorage.Domain.tag, "tagSort") as DirTagBean[];
            uint tagId = 0;
            isThis = false;
            foreach (DirTagBean dirTag in dirTags)
            {
                if (dirTag.Path.IndexOf(path) != -1)
                {
                    tagId = dirTag.TagId;
                    if (dirTag.Path == path)
                        isThis = true;
                    break;
                }
            }
            if (tagId == 0)
                return null;
            else
                return TagMapper.GetOneById(tagId);
        }
    }
}
