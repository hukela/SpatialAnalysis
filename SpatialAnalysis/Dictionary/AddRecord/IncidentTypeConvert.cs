using SpatialAnalysis.Entity;
using System;
using System.Globalization;
using System.Windows.Data;

namespace SpatialAnalysis.Dictionary.AddRecord
{
    /// <summary>
    /// 为了绑定单选框的转换器
    /// </summary>
    public class IncidentTypeConvert : IValueConverter
    {
        //设置
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            IncidentType type = (IncidentType)value;
            //返回bool
            return type == (IncidentType)int.Parse(parameter.ToString());
        }
        //获取
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isChecked = (bool)value;
            if (isChecked)
                return (IncidentType)int.Parse(parameter.ToString());
            else
                return null;
        }
    }
}
