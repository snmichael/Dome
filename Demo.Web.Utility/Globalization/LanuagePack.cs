using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using Demo.Web.Utility.Globalization.Models;

namespace Demo.Web.Utility.Globalization
{
    public class LanuagePack
    {
        private static IList<InstallationLanguage> _availableLanguages;

        public static IList<InstallationLanguage> GetAvailableLanguages()
        {
            if (_availableLanguages == null)
            {
                LoadLanguages();
            }
            return _availableLanguages;
        }

        public static void LoadLanguages()
        {
            _availableLanguages = new List<InstallationLanguage>();
            foreach (
                var filePath in
                    Directory.EnumerateFiles(System.Web.HttpContext.Current.Server.MapPath("~/Localization/Language/"),
                        "*.xml"))
            {
                var xmlDocument = new XmlDocument();
                xmlDocument.Load(filePath);


                //get language code
                var languageCode = "";
                //we file name format: installation.{languagecode}.xml
                var r = new Regex("(.*?)" + Regex.Escape(".xml"));
                var matches = r.Matches(Path.GetFileName(filePath));
                foreach (Match match in matches)
                    languageCode = match.Groups[1].Value;

                //get language friendly name
                var languageName = xmlDocument.SelectSingleNode(@"//Language").Attributes["Name"].InnerText.Trim();

                //is default
                var isDefaultAttribute = xmlDocument.SelectSingleNode(@"//Language").Attributes["IsDefault"];
                var isDefault = isDefaultAttribute != null
                    ? Convert.ToBoolean(isDefaultAttribute.InnerText.Trim())
                    : false;

                //is default
                var isRightToLeftAttribute = xmlDocument.SelectSingleNode(@"//Language").Attributes["IsRightToLeft"];
                var isRightToLeft = isRightToLeftAttribute != null
                    ? Convert.ToBoolean(isRightToLeftAttribute.InnerText.Trim())
                    : false;

                //create language
                var language = new InstallationLanguage()
                {
                    Code = languageCode,
                    Name = languageName,
                    IsDefault = isDefault,
                    IsRightToLeft = isRightToLeft,
                };
                //load resources
                foreach (XmlNode resNode in xmlDocument.SelectNodes(@"//Language/LocaleResource"))
                {
                    var resNameAttribute = resNode.Attributes["Name"];
                    var resValueNode = resNode.SelectSingleNode("Value");

                    if (resNameAttribute == null)
                        throw new Exception("All installation resources must have an attribute Name=\"Value\".");
                    var resourceName = resNameAttribute.Value.Trim();
                    if (string.IsNullOrEmpty(resourceName))
                        throw new Exception("All installation resource attributes 'Name' must have a value.'");

                    if (resValueNode == null)
                        throw new Exception("All installation resources must have an element \"Value\".");
                    var resourceValue = resValueNode.InnerText.Trim();

                    language.Resources.Add(new InstallationLocaleResource()
                    {
                        Name = resourceName,
                        Value = resourceValue
                    });
                }

                _availableLanguages.Add(language);
                _availableLanguages = _availableLanguages.OrderBy(l => l.Name).ToList();

            }
        }
    }
}