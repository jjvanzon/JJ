﻿'------------------------------------------------------------------------------
' <auto-generated>
'     This code was generated by a tool.
'     Runtime Version:4.0.30319.18010
'
'     Changes to this file may cause incorrect behavior and will be lost if
'     the code is regenerated.
' </auto-generated>
'------------------------------------------------------------------------------

Option Strict On
Option Explicit On

Imports System

Namespace My.Resources
    
    'This class was auto-generated by the StronglyTypedResourceBuilder
    'class via a tool like ResGen or Visual Studio.
    'To add or remove a member, edit your .ResX file then rerun ResGen
    'with the /str option, or rebuild your VS project.
    '''<summary>
    '''  A strongly-typed resource class, for looking up localized strings, etc.
    '''</summary>
    <Global.System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0"),  _
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     Global.System.Runtime.CompilerServices.CompilerGeneratedAttribute()>  _
    Friend Class Resource
        
        Private Shared resourceMan As Global.System.Resources.ResourceManager
        
        Private Shared resourceCulture As Global.System.Globalization.CultureInfo
        
        <Global.System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")>  _
        Friend Sub New()
            MyBase.New
        End Sub
        
        '''<summary>
        '''  Returns the cached ResourceManager instance used by this class.
        '''</summary>
        <Global.System.ComponentModel.EditorBrowsableAttribute(Global.System.ComponentModel.EditorBrowsableState.Advanced)>  _
        Friend Shared ReadOnly Property ResourceManager() As Global.System.Resources.ResourceManager
            Get
                If Object.ReferenceEquals(resourceMan, Nothing) Then
                    Dim temp As Global.System.Resources.ResourceManager = New Global.System.Resources.ResourceManager("Microsoft.CodeAnalysis.VisualBasic.UnitTests.Resource", GetType(Resource).Assembly)
                    resourceMan = temp
                End If
                Return resourceMan
            End Get
        End Property
        
        '''<summary>
        '''  Overrides the current thread's CurrentUICulture property for all
        '''  resource lookups using this strongly typed resource class.
        '''</summary>
        <Global.System.ComponentModel.EditorBrowsableAttribute(Global.System.ComponentModel.EditorBrowsableState.Advanced)>  _
        Friend Shared Property Culture() As Global.System.Globalization.CultureInfo
            Get
                Return resourceCulture
            End Get
            Set
                resourceCulture = value
            End Set
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Option Infer On
        '''Option Explicit Off
        '''
        '''Imports System
        '''Imports System.Collections.Generic
        '''Imports System.Linq
        '''Imports System.Linq.Expressions
        '''Imports System.Text
        '''Imports M = System.Math
        '''Imports System.Collections
        '''Imports &lt;xmlns:ns=&quot;foo&quot;&gt;
        '''Imports &lt;xmlns=&quot;foo&quot;&gt;
        '''
        '''#Const line = 6
        '''#Const foo = True
        '''#If foo Then
        '''#Else
        '''#End If
        '''&apos; There is no equivalent to #undef in VB.NET:
        '''&apos;#undef foo
        '''&apos;#warning foo
        '''&apos;#error foo
        '''&apos; There is no equivalent to &apos;extern alias&apos; in VB:
        '''&apos;extern alias Foo;
        '''
        '''#If DEBUG OrEl [rest of string was truncated]&quot;;.
        '''</summary>
        Friend Shared ReadOnly Property VBAllInOne() As String
            Get
                Return ResourceManager.GetString("VBAllInOne", resourceCulture)
            End Get
        End Property
    End Class
End Namespace