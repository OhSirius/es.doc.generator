﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BG.Domain.Infrastructure.Tests.Database.LocalDB {
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
    internal class LocalDBScripts {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal LocalDBScripts() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("BG.Domain.Infrastructure.Tests.Database.LocalDB.LocalDBScripts", typeof(LocalDBScripts).Assembly);
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
        ///   Looks up a localized string similar to CREATE DATABASE [AuthDBTest]
        ///COLLATE Cyrillic_General_CI_AS;  .
        /// </summary>
        internal static string AuthDB {
            get {
                return ResourceManager.GetString("AuthDB", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SET IDENTITY_INSERT dbo.BGApplication ON
        ///INSERT INTO dbo.BGApplication ([Id], [Name]) VALUES 
        ///  (1, N&apos;DocumentsGenerator&apos;)
        ///SET IDENTITY_INSERT dbo.BGApplication OFF
        ///
        ///SET IDENTITY_INSERT dbo.BGLicenseType ON
        ///INSERT INTO dbo.BGLicenseType ([Id], [Name], [Application]) VALUES 
        ///  (1, N&apos;Lite&apos;, 1),
        ///  (2, N&apos;Basis&apos;, 1),
        ///  (3, N&apos;Pro&apos;, 1)
        ///SET IDENTITY_INSERT dbo.BGLicenseType OFF.
        /// </summary>
        internal static string AuthDBListsInsert {
            get {
                return ResourceManager.GetString("AuthDBListsInsert", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to DBCC CHECKIDENT (&apos;[BGApplication]&apos;, RESEED, 0);
        ///DBCC CHECKIDENT (&apos;[BGLicenseType]&apos;, RESEED, 0);
        ///.
        /// </summary>
        internal static string AuthDBReseed {
            get {
                return ResourceManager.GetString("AuthDBReseed", resourceCulture);
            }
        }
    }
}
