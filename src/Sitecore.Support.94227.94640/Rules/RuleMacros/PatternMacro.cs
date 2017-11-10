namespace Sitecore.Support.Rules.RuleMacros
{
    using Sitecore.Data.Items;
    using Sitecore.Diagnostics;
    using Sitecore.Shell.Applications.Dialogs.ItemLister;
    using Sitecore.Text;
    using Sitecore.Web.UI.Sheer;
    using System;
    using System.Linq;
    using System.Xml.Linq;
    using Sitecore.Data.Templates;
    using Sitecore.Data.Managers;
    using Sitecore.Rules.RuleMacros;
    using Analytics.Data.Items;

    public class PatternMacro : IRuleMacro
    {
        private const string PatternTemplateID = "{4A6A7E36-2481-438F-A9BA-0453ECC638FA}";

        private const string PatternsForlderTemplateID = "{0771B0A2-5BCF-4F87-91DE-13474618B6BF}";

        public void Execute(XElement element, string name, UrlString parameters, string value)
        {
            Assert.ArgumentNotNull(element, "element");
            Assert.ArgumentNotNull(name, "name");
            Assert.ArgumentNotNull(parameters, "parameters");
            Assert.ArgumentNotNull(value, "value");
            SelectItemOptions selectItemOptions = new SelectItemOptions();
            string value2 = XElement.Parse(element.ToString()).FirstAttribute.Value;
            if (!string.IsNullOrEmpty(value2))
            {
                Item item = Client.ContentDatabase.GetItem(value2);
                if (item != null)
                {
                    selectItemOptions.FilterItem = item;
                }
            }
            selectItemOptions.ShowRoot = false;
            selectItemOptions.IncludeTemplatesForSelection = SelectItemOptions.GetTemplateList(new string[]
            {
                "{4A6A7E36-2481-438F-A9BA-0453ECC638FA}"
            });
            Item item2 = null;
            Item item3 = Client.ContentDatabase.GetItem(Sitecore.ItemIDs.Analytics.Profiles);
            Assert.IsNotNull(item3, "profile root");
            XAttribute xAttribute = element.Attribute("ProfileName");
            if (xAttribute != null && !string.IsNullOrEmpty(xAttribute.Value))
            {
                item2 = new ItemRecords<ProfileItem>(item3, new Func<Item, ProfileItem>(ProfileItem.Create), ProfileItem.TemplateID, true)[xAttribute.Value];
                if (item2 != null)
                {
                    foreach (Item item5 in item2.Children)
                    {
                        Template template = TemplateManager.GetTemplate(item5);
                        if (template != null && template.InheritsFrom("{0771B0A2-5BCF-4F87-91DE-13474618B6BF}"))
                        {
                            item2 = item5;
                            break;
                        }
                    }
                }
            }
            if (item2 == null)
            {
                if (MainUtil.GetBool(parameters["selectprofilefirst"], false))
                {
                    SheerResponse.Alert("Select profile first.", new string[0]);
                    return;
                }
                item2 = item3;
            }
            Item item4 = null;
            if (!string.IsNullOrEmpty(value))
            {
                item4 = Client.ContentDatabase.GetItem(value);
            }
            selectItemOptions.Root = item2;
            selectItemOptions.SelectedItem = (item4 ?? ((selectItemOptions.Root != null) ? selectItemOptions.Root.Children.FirstOrDefault<Item>() : null));
            selectItemOptions.Title = "Select Pattern";
            selectItemOptions.Text = "Select the pattern to use in this rule.";
            selectItemOptions.Icon = "Custom/24x24/pattern.png";
            string value3 = parameters["resulttype"];
            if (!string.IsNullOrEmpty(value3))
            {
                selectItemOptions.ResultType = (SelectItemOptions.DialogResultType)System.Enum.Parse(typeof(SelectItemOptions.DialogResultType), value3);
            }
            SheerResponse.ShowModalDialog(selectItemOptions.ToUrlString().ToString(), "1200px", "700px", string.Empty, true);
        }
    }
}
