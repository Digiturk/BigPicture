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
            
            var dlls = this._Repository.GetAllNodes<Dll>("Dll", new { Name = project.AssemblyName });
            if(dlls.Count == 0)
            {
                var dllId = this._Repository.CreateNode("Dll", new { Name = project.AssemblyName });
                this._Repository.CreateRelationship(project.Id, dllId, "EXPORTS");
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
                var dll = new Dll();
                dll.Name = reference.Include.Split(',')[0];


                var dlls = this._Repository.GetAllNodes<Dll>("Dll", new { Name = dll.Name });
                if (dlls.Count == 0)
                {
                    dll.Id = this._Repository.CreateNode("Dll", dll);
                }
                else
                {
                    dll = dlls[0];
                }
                
                this._Repository.CreateRelationship(project.Id, dll.Id, "REFERENCES");
            }                        
        }

        private void ProcessCompileItems(Project project, List<ProjectItemElement> compileItems)
        {
            foreach (var item in compileItems)
            {
                //var dll = new Dll();
                //dll.Name = reference.Include.Split(',')[0];


                //var dlls = this._Repository.GetAllNodes<Nodes.File>("Dll", new { Name = dll.Name });
                //if (dlls.Count == 0)
                //{
                //    dll.Id = this._Repository.CreateNode("Dll", dll);
                //}
                //else
                //{
                //    dll = dlls[0];
                //}

                //this._Repository.CreateRelationship(project.Id, dll.Id, "REFERENCES");
            }
        }
    }
}
