﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace JJ.Business.Synthesizer.Resources {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Messages {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Messages() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("JJ.Business.Synthesizer.Resources.Messages", typeof(Messages).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Cannot change the amount of inlets, because inlet {0} is still filled in..
        /// </summary>
        internal static string CannotChangeInletCountBecauseOneIsStillFilledIn {
            get {
                return ResourceManager.GetString("CannotChangeInletCountBecauseOneIsStillFilledIn", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Cannot change the amount of outlets, because outlet {0} is still in use..
        /// </summary>
        internal static string CannotChangeOutletCountBecauseOneIsStillFilledIn {
            get {
                return ResourceManager.GetString("CannotChangeOutletCountBecauseOneIsStillFilledIn", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Cannot delete item, because there are things still linking to it..
        /// </summary>
        internal static string CannotDeleteBecauseHasReferences {
            get {
                return ResourceManager.GetString("CannotDeleteBecauseHasReferences", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Curve &apos;{0}&apos; cannot be deleted, because it is being used by operators..
        /// </summary>
        internal static string CannotDeleteCurveBecauseHasOperators {
            get {
                return ResourceManager.GetString("CannotDeleteCurveBecauseHasOperators", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Sample &apos;{0}&apos; cannot be deleted, because it is being used by operators..
        /// </summary>
        internal static string CannotDeleteSampleBecauseHasOperators {
            get {
                return ResourceManager.GetString("CannotDeleteSampleBecauseHasOperators", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Channel count does not match speaker setup..
        /// </summary>
        internal static string ChannelCountDoesNotMatchSpeakerSetup {
            get {
                return ResourceManager.GetString("ChannelCountDoesNotMatchSpeakerSetup", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Channel number does not match speaker setup..
        /// </summary>
        internal static string ChannelIndexNumberDoesNotMatchSpeakerSetup {
            get {
                return ResourceManager.GetString("ChannelIndexNumberDoesNotMatchSpeakerSetup", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Document &apos;{0}&apos; is dependent on document &apos;{1}&apos;..
        /// </summary>
        internal static string DocumentIsDependentOnDocument {
            get {
                return ResourceManager.GetString("DocumentIsDependentOnDocument", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Inlet numbers are not unique..
        /// </summary>
        internal static string InletListIndexesAreNotUnique {
            get {
                return ResourceManager.GetString("InletListIndexesAreNotUnique", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Inlet names are not unique..
        /// </summary>
        internal static string InletNamesAreNotUnique {
            get {
                return ResourceManager.GetString("InletNamesAreNotUnique", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Inlet not found in the underlying patch. Inlet name: &apos;{0}&apos;, inlet type: &apos;{1}&apos;, inlet number: &apos;{2}&apos;..
        /// </summary>
        internal static string InletNotFoundInUnderlyingPatch {
            get {
                return ResourceManager.GetString("InletNotFoundInUnderlyingPatch", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0} operator named &apos;{1}&apos; does not have {2} filled in..
        /// </summary>
        internal static string InletNotSet {
            get {
                return ResourceManager.GetString("InletNotSet", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0} of inlet does not match with underlying patch. Inlet name: &apos;{1}&apos;, inlet type: &apos;{2}&apos;, inlet number: &apos;{3}&apos;..
        /// </summary>
        internal static string InletPropertyDoesNotMatchWithUnderlyingPatch {
            get {
                return ResourceManager.GetString("InletPropertyDoesNotMatchWithUnderlyingPatch", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0} must be a power of 2..
        /// </summary>
        internal static string MustBePowerOf2 {
            get {
                return ResourceManager.GetString("MustBePowerOf2", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Either a name or a dimension must be filled in..
        /// </summary>
        internal static string NameOrDimensionMustBeFilledIn {
            get {
                return ResourceManager.GetString("NameOrDimensionMustBeFilledIn", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0} names are not unique. Duplicate names: {1}..
        /// </summary>
        internal static string NamesNotUnique_WithEntityTypeNameAndNames {
            get {
                return ResourceManager.GetString("NamesNotUnique_WithEntityTypeNameAndNames", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0} named &apos;{1}&apos; not found..
        /// </summary>
        internal static string NotFound_WithTypeName_AndName {
            get {
                return ResourceManager.GetString("NotFound_WithTypeName_AndName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0} with ID &apos;{1}&apos; not found in the list of {2}..
        /// </summary>
        internal static string NotFoundInList_WithItemName_ID_AndListName {
            get {
                return ResourceManager.GetString("NotFoundInList_WithItemName_ID_AndListName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0} &apos;{1}&apos; is not unique..
        /// </summary>
        internal static string NotUnique_WithPropertyName_AndValue {
            get {
                return ResourceManager.GetString("NotUnique_WithPropertyName_AndValue", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Number named &apos;{0}&apos; is 0..
        /// </summary>
        internal static string NumberIs0WithName {
            get {
                return ResourceManager.GetString("NumberIs0WithName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Operator &apos;{0}&apos; has no inlet filled in..
        /// </summary>
        internal static string OperatorHasNoInletFilledIn_WithOperatorName {
            get {
                return ResourceManager.GetString("OperatorHasNoInletFilledIn_WithOperatorName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Operator &apos;{0}&apos; has no inlets filled in..
        /// </summary>
        internal static string OperatorHasNoInletsFilledIn_WithOperatorName {
            get {
                return ResourceManager.GetString("OperatorHasNoInletsFilledIn_WithOperatorName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Operator &apos;{0}&apos; has no items filled in..
        /// </summary>
        internal static string OperatorHasNoItemsFilledIn_WithOperatorName {
            get {
                return ResourceManager.GetString("OperatorHasNoItemsFilledIn_WithOperatorName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Operator named &apos;{0}&apos; has a circular reference..
        /// </summary>
        internal static string OperatorIsCircularWithName {
            get {
                return ResourceManager.GetString("OperatorIsCircularWithName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The patch of operator &apos;{0}&apos; is filled in, but it is not the expected patch &apos;{1}&apos;..
        /// </summary>
        internal static string OperatorPatchIsNotTheExpectedPatch {
            get {
                return ResourceManager.GetString("OperatorPatchIsNotTheExpectedPatch", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Outlet list indexes are not unique..
        /// </summary>
        internal static string OutletListIndexesAreNotUnique {
            get {
                return ResourceManager.GetString("OutletListIndexesAreNotUnique", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Outlet names are not unique..
        /// </summary>
        internal static string OutletNamesAreNotUnique {
            get {
                return ResourceManager.GetString("OutletNamesAreNotUnique", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Outlet not found in the underlying patch. Outlet name: &apos;{0}&apos;, outlet type: &apos;{1}&apos;, outlet number: &apos;{2}&apos;..
        /// </summary>
        internal static string OutletNotFoundInUnderlyingPatch {
            get {
                return ResourceManager.GetString("OutletNotFoundInUnderlyingPatch", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0} of outlet does not match with underlying patch. Outlet name: &apos;{1}&apos;, outlet type: &apos;{2}&apos;, outlet number: &apos;{3}&apos;..
        /// </summary>
        internal static string OutletPropertyDoesNotMatchWithUnderlyingPatch {
            get {
                return ResourceManager.GetString("OutletPropertyDoesNotMatchWithUnderlyingPatch", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Sample &apos;{0}&apos; has no data..
        /// </summary>
        internal static string SampleCount0 {
            get {
                return ResourceManager.GetString("SampleCount0", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Sample named &apos;{0}&apos; is not active..
        /// </summary>
        internal static string SampleNotActive {
            get {
                return ResourceManager.GetString("SampleNotActive", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Sample &apos;{0}&apos; is not loaded..
        /// </summary>
        internal static string SampleNotLoaded {
            get {
                return ResourceManager.GetString("SampleNotLoaded", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Scale names are not unique..
        /// </summary>
        internal static string ScaleNamesNotUnique {
            get {
                return ResourceManager.GetString("ScaleNamesNotUnique", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to There is a circular dependency with its underlying patch..
        /// </summary>
        internal static string UnderlyingPatchIsCircular {
            get {
                return ResourceManager.GetString("UnderlyingPatchIsCircular", resourceCulture);
            }
        }
    }
}
