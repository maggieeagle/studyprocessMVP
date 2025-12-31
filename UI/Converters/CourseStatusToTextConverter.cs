using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace UI.Converters
{
    public class CourseStatusToTextConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return value == null ? string.Empty : value.ToString() ?? string.Empty;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            var str = value?.ToString();
            if (string.IsNullOrEmpty(str) || str == string.Empty)
                return null;
            return Enum.Parse(typeof(Course.CourseStatus), str);
        }
    }
}
