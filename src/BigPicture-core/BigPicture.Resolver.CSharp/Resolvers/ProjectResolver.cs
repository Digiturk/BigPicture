using BigPicture.Core.Resolver;
using BigPicture.Resolver.CSharp.Nodes;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Build.Construction;
using BigPicture.Core.Repository;

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
            Console.WriteLine("Node processing: " + project.Name);            

            var projectData = Microsoft.Build.Evaluation.Project.FromFile(project.AbsolutePath, new Microsoft.Build.Definition.ProjectOptions());

        }
    }
}
