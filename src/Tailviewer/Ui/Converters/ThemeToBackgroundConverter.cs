using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using Tailviewer.Settings;

namespace Tailviewer.Ui.Converters
{
	/// <summary>
	///     Converts a Theme enum value to the appropriate background brush.
	/// </summary>
	public sealed class ThemeToBackgroundConverter
		: IValueConverter
	{
		public static readonly ThemeToBackgroundConverter Instance = new ThemeToBackgroundConverter();
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is Theme theme)
			{
				return theme == Theme.Dark ? Brushes.Black : Brushes.White;
			}

			return Brushes.White; // Default fallback
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}