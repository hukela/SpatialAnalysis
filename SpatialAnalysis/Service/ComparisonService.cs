﻿using SpatialAnalysis.Entity;
using SpatialAnalysis.Mapper;
using SpatialAnalysis.MyWindow;
using SpatialAnalysis.Service.ComparisonExtend;
using SpatialAnalysis.Utils;
using System;
using System.Numerics;

namespace SpatialAnalysis.Service
{
internal static class ComparisonService
{
    /// <summary>
    /// 获取事件选择列表
    /// </summary>
    public static IncidentBean[] GetComboBoxResource()
    {
        IncidentBean[] beans = IncidentMapper.SelectSuccessIncidents();
        IncidentBean[] items = new IncidentBean[beans.Length + 1];
        items[0] = new IncidentBean
        {
            Id = 0,
            Title = "请选择事件",
        };
        for (int i = 1; i < items.Length; i++)
            items[i] = beans[i - 1];
        return items;
    }

    /// <summary>
    /// 获取根节点列表
    /// </summary>
    /// <param name="oldIncidentId">老事件</param>
    /// <param name="newIncidentId">新事件</param>
    public static DirNode[] GetRootNodes(uint oldIncidentId, uint newIncidentId)
    {
        DirNode baseNode = new DirNode()
        {
            OldIncidentId = oldIncidentId,
            OldId = 0,
            NewIncidentId = newIncidentId,
            NewId = 0,
        };
        BuildNodeTree.BuildChildrenNodes(baseNode);
        return baseNode.Children;
    }

    /// <summary>
    /// 建立该节点中子节点的子节点
    /// </summary>
    /// <returns>该节点数据是否被刷新</returns>
    public static bool BuildNodeChildren(DirNode dirNode)
    {
        bool update = dirNode.Children.Length != 0 && dirNode.Children[0] == null;
        //防止重复计算
        if (update)
            BuildNodeTree.BuildChildrenNodes(dirNode);
        return update;
    }

    /// <summary>
    /// 通过节点获取文件夹的比较信息
    /// </summary>
    public static ComparisonInfo GetInfoByNode(DirNode node)
    {
        //获取bean
        RecordBean oldBean = null;
        if (node.OldId != 0)
            oldBean = RecordMapper.SelectById(node.OldIncidentId, node.OldId);
        RecordBean newBean = null;
        if (node.NewId != 0)
            newBean = RecordMapper.SelectById(node.NewIncidentId, node.NewId);
        //设置标签
        ComparisonInfo info = new ComparisonInfo()
        { TagName = string.Concat("标签：", node.Tag.Name ?? "没有标签") };
        //设置主要比较的数据
        BigInteger oldSize, newSize, oldUsage, newUsage;
        if (oldBean != null)
        {
            info.Title = oldBean.Name;
            info.Location = oldBean.Location;
            info.CreateTime = "创建时间：" + oldBean.CreateTime.ToString("yy-MM-dd HH:mm:ss");
            info.OldFileCount = Convert.ToInt32(oldBean.FileCount);
            info.OldDirCount = Convert.ToInt32(oldBean.DirCount);
            info.OldExCode = oldBean.ExceptionCode;
            oldSize = oldBean.Size;
            oldUsage = oldBean.SpaceUsage;
        }
        else
        {
            oldSize = BigInteger.Zero;
            oldUsage = BigInteger.Zero;
        }
        if (newBean != null)
        {
            info.Title = newBean.Name;
            info.Location = newBean.Location;
            info.CreateTime = "创建时间：" + newBean.CreateTime.ToString("yy-MM-dd HH:mm:ss");
            info.NewFileCount = Convert.ToInt32(newBean.FileCount);
            info.NewDirCount = Convert.ToInt32(newBean.DirCount);
            info.NewExCode = newBean.ExceptionCode;
            newSize = newBean.Size;
            newUsage = newBean.SpaceUsage;
        }
        else
        {
            newSize = BigInteger.Zero;
            newUsage = BigInteger.Zero;
        }
        //单位换算
        info.OldSize = ConversionUtil.StorageFormat(oldSize, false);
        info.NewSize = ConversionUtil.StorageFormat(newSize, false);
        info.SizeChanged = ConversionUtil.StorageFormat(newSize - oldSize, true);
        info.OldUsage = ConversionUtil.StorageFormat(oldUsage, false);
        info.NewUsage = ConversionUtil.StorageFormat(newUsage, false);
        info.UsageChanged = ConversionUtil.StorageFormat(newUsage - oldUsage, true);
        //设置状态
        switch (node.Type)
        {
            case DirNodeType.Unchanged:
                info.Action = "文件夹未发生变化";
                break;
            case DirNodeType.Added:
                info.Action = "该文件夹是新增的文件夹";
                break;
            case DirNodeType.Changed:
                info.Action = "该文件夹发生了变化";
                break;
            case DirNodeType.Deleted:
                info.Action = "该文件夹已经被删除";
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        return info;
    }

    /// <summary>
    /// 修改或新添标签标注
    /// </summary>
    /// <param name="path">标注路径</param>
    /// <param name="isNew">是否是新建</param>
    public static void AddOrEditTag(string path, bool isNew)
    {
        SelectTagWindow window = new SelectTagWindow();
        bool? dialogResult = window.ShowDialog();
        if (dialogResult != true)
            return;
        TagBean tagBean = window.tagBean;
        if (tagBean.Id == 0)
            DirTagMapper.DeleteOneByPath(path);
        if (isNew)
            DirTagMapper.InsertOne(new DirTagBean()
            {
                Path = path,
                TagId = tagBean.Id,
            });
        else
            DirTagMapper.UpdateByPath(path, tagBean.Id);
        //刷新标签缓存
        DirTagCache.DeleteTagCache();
    }
} }
