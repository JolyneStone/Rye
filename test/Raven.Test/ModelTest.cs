public class TempleteRazorPageView : Raven.CodeGenerator.Razor.EntityRazorPageView
{
#pragma warning disable 1998
    public async override global::System.Threading.Tasks.Task ExecuteAsync()
    {
        WriteLiteral("using System;\r\n\r\nnamespace ");
#nullable restore
#line 3 "C:\monster\Code\KiraNet\Raven\test\Raven.Test\bin\Debug\netcoreapp3.0\Templates\ModelObject.tp"
        Write(Model.NameSpace);

#line default
#line hidden
#nullable disable
        WriteLiteral("\r\n{\r\n    [Serializable]\r\n    public partial class ");
#nullable restore
#line 6 "C:\monster\Code\KiraNet\Raven\test\Raven.Test\bin\Debug\netcoreapp3.0\Templates\ModelObject.tp"
        Write(Model.Name);

#line default
#line hidden
#nullable disable
        WriteLiteral("\r\n    {\r\n");
#nullable restore
#line 8 "C:\monster\Code\KiraNet\Raven\test\Raven.Test\bin\Debug\netcoreapp3.0\Templates\ModelObject.tp"
        foreach (var property in Model.Properties)
        {


#line default
#line hidden
#nullable disable
#nullable restore
#line 10 "C:\monster\Code\KiraNet\Raven\test\Raven.Test\bin\Debug\netcoreapp3.0\Templates\ModelObject.tp"
            Write(Raw("\t\t/// <summary>"));

#line default
#line hidden
#nullable disable
#nullable restore
#line 11 "C:\monster\Code\KiraNet\Raven\test\Raven.Test\bin\Debug\netcoreapp3.0\Templates\ModelObject.tp"
            Write(Raw($"\t\t/// {property.Description}"));

#line default
#line hidden
#nullable disable
#nullable restore
#line 12 "C:\monster\Code\KiraNet\Raven\test\Raven.Test\bin\Debug\netcoreapp3.0\Templates\ModelObject.tp"
            Write(Raw("\t\t/// </summary>"));

#line default
#line hidden
#nullable disable
#nullable restore
#line 13 "C:\monster\Code\KiraNet\Raven\test\Raven.Test\bin\Debug\netcoreapp3.0\Templates\ModelObject.tp"
            Write(Raw($"\t\tpublic {property.Type} {property.Name} {{ get; set; }}{(string.IsNullOrEmpty(property.DefaultValue) ? "" : (" = ") + property.DefaultValue + ";")}"));

#line default
#line hidden
#nullable disable
#nullable restore
#line 13 "C:\monster\Code\KiraNet\Raven\test\Raven.Test\bin\Debug\netcoreapp3.0\Templates\ModelObject.tp"


        }

#line default
#line hidden
#nullable disable
        WriteLiteral("    }\r\n}");
    }
#pragma warning restore 1998
}
