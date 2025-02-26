using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Shapes;
using WpfMvvm.Infrastructure.Converters.Base;

namespace WpfMvvm.Infrastructure.Converters
{
    public class MouseOverToTemplateConverter : Converter
    {
        private const string __templateCertificate = "PopupCertificateTemplate";
        private const string __templateIsEncrypted = "PopupIsEncryptedTemplate";
        private const string __templateIsExportable = "PopupIsExportedTemplate";
        private const string __templateIsDeleted = "PopupIsDeletedTemplate";

        private static int _callCounterFromElement = 0;
        private static DependencyObject _lastElement;
        private static string _lastTag;

        public override object Convert(object value, Type t, object p, CultureInfo c)
        {
            var elementUnderMouse = Mouse.DirectlyOver as DependencyObject;

            if (IsBadType(elementUnderMouse) || HasElementAlreadyCalledMe(elementUnderMouse))
                return null;

            var popupChild = GetPopupChild(elementUnderMouse);
            if (popupChild == null)
                return null;

            var currentTag = GetCurrentIconTag(elementUnderMouse);
            if (string.Equals(_lastTag, currentTag, StringComparison.OrdinalIgnoreCase))
                return null;

            _lastTag = currentTag;
            ChangeTemplate(popupChild);

            return null;
        }

        private static bool IsBadType(DependencyObject element) =>
           element is not (Border or Path or Canvas);

        private static bool HasElementAlreadyCalledMe(DependencyObject callElement)
        {
            if (_lastElement != callElement)
            {
                _lastElement = callElement;
                _callCounterFromElement = 0;
                _lastTag = string.Empty;
            }
            _callCounterFromElement++;
            return _callCounterFromElement != 1;
        }

        private static ContentPresenter GetPopupChild(DependencyObject element)
        {
            var panelWithIcons = RelativeParent<StackPanel>(element);
            if (panelWithIcons == null)
                return null;
            var popup = RelativeChild<Popup>(panelWithIcons);
            return (popup == null || popup.Child == null)
                ? null
                : RelativeChild<ContentPresenter>(popup.Child);
        }

        private static string GetCurrentIconTag(DependencyObject element)
        {
            var iconControl = RelativeParent<ContentControl>(element);
            return (iconControl == null || iconControl.Tag == null)
                ? null
                : iconControl.Tag.ToString();
        }

        private static void ChangeTemplate(ContentPresenter child)
        {
            var templateName = GetTemplateName(_lastTag);
            child.ContentTemplate = string.IsNullOrWhiteSpace(templateName)
                ? null
                : GetMainListView().FindResource(templateName) as DataTemplate;
        }

        private static string GetTemplateName(string iconIndex) =>
            iconIndex switch
            {
                "0" => __templateCertificate,
                "1" => __templateIsEncrypted,
                "2" => __templateIsExportable,
                "3" => __templateIsDeleted,
                _ => string.Empty
            };
    }
}
