namespace Sitecore.Support.Rules.RuleMacros
{
    using Sitecore.Data.Items;
    using Sitecore.Diagnostics;
    using Sitecore.Rules.RuleMacros;
    using Sitecore.Shell.Applications.Dialogs.ItemLister;
    using Sitecore.Text;
    using Sitecore.Web.UI.Sheer;
    using System;
    using System.Linq;
    using System.Xml.Linq;

    public class ProfileMacro : IRuleMacro
    {
        public void Execute(XElement element, string name, UrlString parameters, string value)
        {
            Assert.ArgumentNotNull(element, "element");
            Assert.ArgumentNotNull(name, "name");
            Assert.ArgumentNotNull(parameters, "parameters");
            Assert.ArgumentNotNull(value, "value");
            SelectItemOptions selectItemOptions = new SelectItemOptions();
            Item item = null;
            bool flag = !string.IsNullOrEmpty(value);
            if (flag)
            {
                item = Client.ContentDatabase.GetItem(value);
            }
            string value2 = XElement.Parse(element.ToString()).FirstAttribute.Value;
            bool flag2 = !string.IsNullOrEmpty(value2);
            if (flag2)
            {
                Item item2 = Client.ContentDatabase.GetItem(value2);
                bool flag3 = item2 != null;
                if (flag3)
                {
                    selectItemOptions.FilterItem = item2;
                }
            }
            selectItemOptions.Root = Client.ContentDatabase.GetItem(ItemIDs.Analytics.Profiles);
            selectItemOptions.SelectedItem = (item ?? ((selectItemOptions.Root != null) ? selectItemOptions.Root.Children.FirstOrDefault<Item>() : null));
            selectItemOptions.IncludeTemplatesForSelection = SelectItemOptions.GetTemplateList(new string[]
            {
                "{8E0C1738-3591-4C60-8151-54ABCC9807D1}"
            });
            selectItemOptions.IncludeTemplatesForDisplay = SelectItemOptions.GetTemplateList(new string[]
            {
                "{8E0C1738-3591-4C60-8151-54ABCC9807D1}",
                "{A87A00B1-E6DB-45AB-8B54-636FEC3B5523}"
            });
            selectItemOptions.Title = "Select Profile";
            selectItemOptions.Text = "Select the profile to use in this rule.";
            selectItemOptions.Icon = "Business/16x16/chart.png";
            selectItemOptions.ShowRoot = false;
            string value3 = parameters["resulttype"];
            bool flag4 = !string.IsNullOrEmpty(value3);
            if (flag4)
            {
                selectItemOptions.ResultType = (SelectItemOptions.DialogResultType)Enum.Parse(typeof(SelectItemOptions.DialogResultType), value3);
            }
            SheerResponse.ShowModalDialog(selectItemOptions.ToUrlString().ToString(), "1000px", "600px", "", true);
        }
    }
}
