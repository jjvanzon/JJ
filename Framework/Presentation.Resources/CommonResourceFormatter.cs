﻿namespace JJ.Framework.Presentation.Resources
{
    public static class CommonResourceFormatter
    {
        public static string Add => CommonResources.Add;
        public static string AreYouSureYouWishToDelete_WithTypeName_AndName(string objectTypeDisplayName, string objectName) => string.Format(CommonResources.AreYouSureYouWishToDelete_WithTypeName_AndName, objectTypeDisplayName, objectName);
        public static string Cancel => CommonResources.Cancel;
        public static string CannotDelete_WithTypeName_AndName(string objectTypeDisplayName, string objectName) => string.Format(CommonResources.CannotDelete_WithTypeName_AndName, objectTypeDisplayName, objectName);
        public static string Close => CommonResources.Close;
        public static string Close_WithName(string name) => string.Format(CommonResources.Close_WithName, name);
        public static string Delete => CommonResources.Delete;
        public static string Delete_WithName(string name) => string.Format(CommonResources.Delete_WithName, name);
        public static string Details_WithName(string name) => string.Format(CommonResources.Details_WithName, name);
        public static string Edit => CommonResources.Edit;
        public static string Edit_WithName(string name) => string.Format(CommonResources.Edit_WithName, name);
        public static string False => CommonResources.False;
        public static string FilePath => CommonResources.FilePath;
        public static string ID => CommonResources.ID;
        public static string IsDeleted_WithName(string name) => string.Format(CommonResources.IsDeleted_WithName, name);
        public static string Item => CommonResources.Item;
        public static string Language => CommonResources.Language;
        public static string LogIn => CommonResources.LogIn;
        public static string LogOut => CommonResources.LogOut;
        public static string Menu => CommonResources.Menu;
        public static string Messages => CommonResources.Messages;
        public static string Name => CommonResources.Name;
        public static string New => CommonResources.New;
        public static string No => CommonResources.No;
        public static string None => CommonResources.None;
        public static string NoObject_WithName(string name) => string.Format(CommonResources.NoObject_WithName, name);
        public static string NotAuthorized => CommonResources.NotAuthorized;
        public static string NotFound_WithName(string name) => string.Format(CommonResources.NotFound_WithName, name);
        public static string NotFound_WithName_AndID(string name, object id) => string.Format(CommonResources.NotFound_WithName_AndID, name, id);
        public static string ObjectCount_WithNamePlural(string namePlural) => string.Format(CommonResources.ObjectCount_WithNamePlural, namePlural);
        public static string OK => CommonResources.OK;
        public static string Properties => CommonResources.Properties;
        public static string Properties_WithName(string name) => string.Format(CommonResources.Properties_WithName, name);
        public static string Remove => CommonResources.Remove;
        public static string Save => CommonResources.Save;
        public static string Save_WithName(string name) => string.Format(CommonResources.Save_WithName, name);
        public static string Search => CommonResources.Search;
        public static string True => CommonResources.True;
        public static string Yes => CommonResources.Yes;
    }
}