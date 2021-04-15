using SpatialAnalysis.Entity;
using SpatialAnalysis.Mapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using System.Windows.Controls;

namespace SpatialAnalysis.Service
{
    class ComparisonService
    {
        /// <summary>
        /// 获取事件选择列表
        /// </summary>
        public static Grid[] GetComboBoxResource()
        {
            DataTable table = IncidentMapper.GetSuccessIncident();
            int count = table.Rows.Count;
            Grid[] items = new Grid[count];
            for (int i = 0; i < count; i++)
            {
                TextBlock title = new TextBlock()
                {
                    Text = table.Rows[i]["title"] as string,
                    FontSize = 16,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Margin = new Thickness(8, 0, 0, 0),
                };
                TextBlock time = new TextBlock()
                {
                    Text = ((DateTime)table.Rows[i]["create_time"]).ToString("yy-MM-dd HH:mm"),
                    FontSize = 16,
                    HorizontalAlignment = HorizontalAlignment.Right,
                    Margin = new Thickness(0, 0, 8, 0),
                };
                Grid grid = new Grid()
                {
                    Uid = table.Rows[i]["title"].ToString()
                };
                grid.Children.Add(title);
                grid.Children.Add(time);
                items[i] = grid;
            }
            return items;
        }
        /// <summary>
        /// 获取两个事件中文件夹的子一级文件夹的比较
        /// </summary>
        /// <param name="oldIncidentId">以前的事件id</param>
        /// <param name="newIncidentId">后来的事件id</param>
        /// <param name="oldid">以前事件中的父级id</param>
        /// <param name="newId">后来事件中的父级id</param>
        public static DirNode[] GetDirNode(uint oldIncidentId, ulong oldid, uint newIncidentId, ulong newId)
        {
            RecordBean[] oldBeans = RecordMapper.GetBeansByPid(oldid, oldIncidentId);
            RecordBean[] newBeans = RecordMapper.GetBeansByPid(newId, newIncidentId);
            List<DirNode> dirNodes = new List<DirNode>();
            //遍历比较两个bean列表
            foreach (RecordBean oldBean in oldBeans)
            {
                RecordBean newBean = null;
                for (int i = 0; i < newBeans.Length; i++)
                {
                    if (oldBean.Path == newBeans[i].Path)
                    {
                        newBean = newBeans[i];
                        newBeans[i] = null;
                    }
                }
                DirNode dirNode = new DirNode()
                {
                    OldIncidentId = oldIncidentId,
                    OldId = oldBean.Id,
                    Name = oldBean.Name,
                };
                if (newBean != null)
                {
                    dirNode.NewIncidentId = newIncidentId;
                    dirNode.NewId = newBean.Id;
                    if (oldBean.Equals(newBean))
                        dirNode.Color = "#87CEFA";
                }
                else
                    dirNode.Color = "#FFC0CB";
            }
            foreach (RecordBean newBean in newBeans)
            {
            }
        }
    }
}
