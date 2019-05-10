using BigPicture.Core.Resolver;
using BigPicture.Resolver.CSharp.Nodes;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Build.Construction;
using BigPicture.Core.Repository;
using System.IO;
using System.Linq;

namespace BigPicture.Resolver.CSharp.Resolvers
{
    public class ProjectResolver : IResolver<Project>
    {
        private IRepository _Repository { get; set; }

        public ProjectResolver(IRepository repository)
        {
            this._Repository = repository;
        }

        public void Resolve(Project project)
        {            
            var csproj = Microsoft.Build.Construction.ProjectRootElement.Open(project.AbsolutePath);

            this.UpdateProject(project, csproj);

            var allItems = csproj.Items.ToList();

            this.ProcessReferenceItems(project, allItems.FindAll(a => a.ItemType == "Reference"));
            this.ProcessCompileItems(project, allItems.FindAll(a => a.ItemType == "Compile"));   
        }

        private void UpdateProject(Project project, ProjectRootElement csproj)
        {
            var props = csproj.Properties.ToList();

            project.OutputType = this.GetPropertyValue("OutputType", props);
            project.AssemblyName = this.GetPropertyValue("AssemblyName", props);
            project.TargetFrameworkVersion = this.GetPropertyValue("TargetFrameworkVersion", props);

            this._Repository.UpdateNode(project);
            
            var assemblies = this._Repository.GetAllNodes<Assembly>("Assembly", new { Name = project.AssemblyName });
            if(assemblies.Count == 0)
            {
                var assemblyId = this._Repository.CreateNode("Assembly", new { Name = project.AssemblyName });
                this._Repository.CreateRelationship(project.Id, assemblyId, "EXPORTS");
            }
        }

        public String GetPropertyValue(String name, List<ProjectPropertyElement> props)
        {
            var prop = props.Find(a => a.Name == name);
            if(prop == null)
            {
                return String.Empty;
            }

            return prop.Value;
        }

        private void ProcessReferenceItems(Project project, List<ProjectItemElement> references)
        {
            foreach(var reference in references)
            {
                var assembly = new Assembly();
                assembly.Name = reference.Include.Split(',')[0];

                var assemblies = this._Repository.GetAllNodes<Assembly>("Assembly", new { Name = assembly.Name });
                if (assemblies.Count == 0)
                {
                    assembly.Id = this._Repository.CreateNode("Assembly", assembly);
                }
                else
                {
                    assembly = assemblies[0];
                }
                
                this._Repository.CreateRelationship(project.Id, assembly.Id, "REFERENCES");
            }                        
        }

        private void ProcessCompileItems(Project project, List<ProjectItemElement> compileItems)
        {
            foreach (var item in compileItems)
            {
                var compileItem = new CompileItem();
                compileItem.Path = item.Include;
                compileItem.Name = Path.GetFileName(compileItem.Path);
                compileItem.AbsolutePath = Path.Combine(Path.GetDirectoryName(project.AbsolutePath), compileItem.Path);

                foreach(var child in item.Children)
                {
                    if (child.ElementName == "SubType")
                    {
                        compileItem.SubType = ((ProjectMetadataElement)child).Value;
                    }

                    if (child.ElementName == "AutoGen")
                    {
                        compileItem.AutoGen = ((ProjectMetadataElement)child).Value;
                    }

                    if (child.ElementName == "DesignTime")
                    {
                        compileItem.DesignTime = ((ProjectMetadataElement)child).Value;
                    }
                }

                compileItem.Id = this._Repository.CreateNode("CompileItem", compileItem);
                this._Repository.CreateRelationship(project.Id, compileItem.Id, "COMPILES");
            }
        }
    }
}
