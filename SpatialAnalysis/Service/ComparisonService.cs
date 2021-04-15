using SpatialAnalysis.Entity;
using SpatialAnalysis.Mapper;
using SpatialAnalysis.Service.ComparisonExtend;
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
            Grid[] items = new Grid[count + 1];
            items[0] = new Grid() { Uid = "unll" };
            items[0].Children.Add(new TextBlock()
            {
                Text = "请选择事件",
                FontSize = 16,
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = new Thickness(8, 0, 0, 0),
            });
            for (int i = 1; i < count + 1; i++)
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
            DirNode[] nodes = BuildNodeTree.GetChildrenNodes(baseNode);
            foreach (DirNode node in nodes)
                node.Children = BuildNodeTree.GetChildrenNodes(node);
            return nodes;
        }
        /// <summary>
        /// 建立该节点中子节点的子节点
        /// </summary>
        public static void BuiledNodeChildren(ref DirNode baseNode)
        {
            foreach (DirNode dirNode in baseNode.Children)
                dirNode.Children = BuildNodeTree.GetChildrenNodes(dirNode);
        }
    }
}
