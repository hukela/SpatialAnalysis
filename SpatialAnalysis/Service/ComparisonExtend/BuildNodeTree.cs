﻿using System;
using SpatialAnalysis.Entity;
using SpatialAnalysis.Mapper;
using System.Collections.Generic;

namespace SpatialAnalysis.Service.ComparisonExtend
{
internal class BuildNodeTree
{

    /// <summary>
    /// 建立该节点的子节点
    /// </summary>
    /// <param name="node">文件夹节点</param>
    public static void BuildChildrenNodes(DirNode node)
    {
        //获取对应节点下的文件夹列表
        RecordBean[] oldBeans;
        if (node.OldIncidentId == 0)
            oldBeans = Array.Empty<RecordBean>();
        else
            oldBeans = RecordMapper.GetBeansByPid(node.OldId, node.OldIncidentId);
        RecordBean[] newBeans;
        if (node.NewIncidentId == 0)
            newBeans = Array.Empty<RecordBean>();
        else
            newBeans = RecordMapper.GetBeansByPid(node.NewId, node.NewIncidentId);
        List<DirNode> dirNodes = new List<DirNode>();
        //遍历两个文件夹列表
        foreach (RecordBean oldBean in oldBeans)
        {
            RecordBean newBean = null;
            //去除重复项
            for (int i = 0; i < newBeans.Length; i++)
            {
                if (newBeans[i] == null)
                    continue;
                if (oldBean.Path == newBeans[i].Path)
                {
                    newBean = newBeans[i];
                    newBeans[i] = null;
                }
            }
            //建立node
            DirNode dirNode = new DirNode()
            {
                Name = oldBean.Name,
                Path = oldBean.Path,
                // 放入一个空数组，告知页面该节点可以展开
                Children = oldBean.DirCount > 0 ? new DirNode[1] : null,
            };
            SetOldId(ref dirNode, oldBean, node.OldIncidentId);
            //设置类型
            if (newBean != null)
            {
                SetNewId(ref dirNode, newBean, node.NewIncidentId);
                if (oldBean.Equals(newBean))
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
            DirNode dirNode = new DirNode()
            {
                Name = newBean.Name,
                Path = newBean.Path,
                Children = newBean.DirCount > 0 ? new DirNode[1] : null,
                //新增的节点
                Type = DirNodeType.Added,
            };
            SetNewId(ref dirNode, newBean, node.NewIncidentId);
            dirNodes.Add(dirNode);
        }
        //添加标签
        TagBean nullTag = new TagBean() { Color = "#FFFFFF" };
        foreach (DirNode dirNode in dirNodes)
        {
            TagBean tagBean = TagCache.GetTagByPath(dirNode.Path, out bool isThis);
            if (tagBean == null)
            {
                dirNode.Tag = nullTag;
                continue;
            }
            dirNode.IsRootTag = isThis;
            dirNode.Tag = tagBean;
        }
        node.Children = dirNodes.ToArray();
    }
    //若有指针的话，则调整node的基础节点为指针所在的节点
    private static void SetOldId(ref DirNode node, RecordBean bean, uint incidentId)
    {
        if (bean.IncidentId != 0)
        {
            node.OldIncidentId = bean.IncidentId;
            node.OldId = bean.TargetId;
        }
        else
        {
            node.OldIncidentId = incidentId;
            node.OldId = bean.Id;
        }
    }
    //作用同上，调整新记录的指针
    private static void SetNewId(ref DirNode node, RecordBean bean, uint incidentId)
    {
        if (bean.IncidentId != 0)
        {
            node.NewIncidentId = bean.IncidentId;
            node.NewId = bean.TargetId;
        }
        else
        {
            node.NewIncidentId = incidentId;
            node.NewId = bean.Id;
        }
    }
} }
