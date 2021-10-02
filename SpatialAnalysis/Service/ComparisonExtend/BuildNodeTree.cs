using SpatialAnalysis.Entity;
using SpatialAnalysis.Mapper;
using System.Collections.Generic;
using System.IO;

namespace SpatialAnalysis.Service.ComparisonExtend
{
    internal class BuildNodeTree
    {
        public static DirNode[] RootChildrenNodes()
        {
            DriveInfo[] drives = DriveInfo.GetDrives();
            return null;
        }
        /// <summary>
        /// 建立该节点的子节点
        /// </summary>
        /// <param name="node">文件夹节点</param>
        public static DirNode[] BuildChildrenNodes(DirNode node)
        {
            //获取对应节点下的文件夹列表
            RecordBean[] oldBeans;
            if (node.OldBean.IncidentId == 0)
                oldBeans = new RecordBean[0];
            else
                oldBeans = RecordMapper.GetBeansByPid(node.OldBean.Id, node.OldBean.IncidentId);
            RecordBean[] newBeans;
            if (node.NewBean.IncidentId == 0)
                newBeans = new RecordBean[0];
            else
                newBeans = RecordMapper.GetBeansByPid(node.NewBean.Id, node.NewBean.IncidentId);
            List<DirNode> dirNodes = new List<DirNode>();
            //基于oldBeans建立节点
            foreach (RecordBean oldBean in oldBeans)
            {
                RecordBean newBean = GetOneByPath(oldBean.Path, newBeans);
                RecordBean tOldBean = GetTargectBean(oldBean);
                RecordBean tNewBean = GetTargectBean(newBean);
                DirNode dirNode = new DirNode()
                {
                    Name = tOldBean.Name,
                    Path = tOldBean.Path,
                    OldBean = tOldBean,
                };
                //设置类型
                if (tNewBean != null)
                {
                    dirNode.NewBean = tNewBean;
                    if (tOldBean.Equals(tNewBean))
                        dirNode.Type = DirNodeType.Unchanged;
                    else
                        dirNode.Type = DirNodeType.Changed;
                }
                else
                    dirNode.Type = DirNodeType.Deleted;
                dirNodes.Add(dirNode);
            }
            //新纪录中新增的部分
            foreach (RecordBean newBean in newBeans)
            {
                if (newBean == null)
                    continue;
                RecordBean tNewBean = GetTargectBean(newBean);
                DirNode dirNode = new DirNode()
                {
                    Name = tNewBean.Name,
                    Path = tNewBean.Path,
                    NewBean = tNewBean,
                    Type = DirNodeType.Added,
                };
                dirNodes.Add(dirNode);
            }
            //添加标签
            TagBean nullTag = new TagBean() { Color = "#FFFFFF" };
            foreach (DirNode dirNode in dirNodes)
            {
                TagBean tagBean = TagSupport.GetTagByPath(dirNode.Path, out bool isThis);
                if (tagBean == null)
                {
                    dirNode.Tag = nullTag;
                    continue;
                }
                dirNode.IsRootTag = isThis;
                dirNode.Tag = tagBean;
            }
            return dirNodes.ToArray();
        }
        //基于oldBean筛选newBenan
        private static RecordBean GetOneByPath(string path, RecordBean[] beans)
        {
            RecordBean newBean = null;
            for (int i = 0; i < beans.Length; i++)
            {
                if (beans[i] == null)
                    continue;
                if (path == beans[i].Path)
                {
                    newBean = beans[i];
                    beans[i] = null;
                }
            }
            return newBean;
        }
        enum BeanType {Old, New}
        private static DirNode BuildNode(RecordBean bean, BeanType beanType)
        {
            if (bean.IncidentId != 0)
                bean = RecordMapper.GetOneById(bean.TargetId, bean.IncidentId);
            DirNode node = new DirNode()
            {
                Name = bean.Name,
                Path = bean.Path
            };
            switch (beanType)
            {
                case BeanType.Old: node.OldBean = bean; break;
                case BeanType.New: node.NewBean = bean; break;
            }
            return node;
        }
        private static RecordBean GetTargectBean(RecordBean bean)
        {
            if (bean.IncidentId != 0)
                bean = RecordMapper.GetOneById(bean.TargetId, bean.IncidentId);
            return bean;
        }
    }
}
