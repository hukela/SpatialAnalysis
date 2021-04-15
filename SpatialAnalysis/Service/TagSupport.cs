using SpatialAnalysis.Entity;
using SpatialAnalysis.Mapper;
using SpatialAnalysis.Utils;

namespace SpatialAnalysis.Service
{
    //提供标签支持
    class TagSupport
    {

        /// <summary>
        /// 设置或刷新标签标注的数据
        /// </summary>
        public static void SetTagSort()
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
        /// 获取排序后的标注路径
        /// </summary>
        public static DirTagBean[] GetTagSort()
        {
            return InternalStorage.Get(InternalStorage.Domain.tag, "tagSort") as DirTagBean[];
        }
    }
}
