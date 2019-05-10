using BigPicture.Core.Resolver;
using BigPicture.Resolver.CSharp.Nodes;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Build.Construction;
using BigPicture.Core.Repository;

namespace BigPicture.Resolver.CSharp.Resolvers
{
    public class SolutionResolver : IResolver<Solution>
    {
        private IRepository _Repository { get; set; }

        public SolutionResolver(IRepository repository)
        {
            this._Repository = repository;
        }

        public void Resolve(Solution solution)
        {
            var solutionFile = SolutionFile.Parse(solution.Path);

            var projectNodes = new List<Project>();
            foreach(var project in solutionFile.ProjectsInOrder)
            {
                var projectNode = new Project();
                projectNode.AbsolutePath = project.AbsolutePath;
                projectNode.Name = project.ProjectName;
                projectNode.ProjectGuid = project.ProjectGuid;
                projectNode.RelativePath = project.RelativePath;

                var list = this._Repository.GetAllNodes<Project>("Project", new { AbsolutePath = projectNode.AbsolutePath });
                if(list.Count == 0)
                {
                    projectNode.Id = this._Repository.CreateNode("Project", projectNode);
                }
                else
                {
                    projectNode = list[0];
                }
                
                this._Repository.CreateRelationship(solution.Id, projectNode.Id, "CONTAINS");

                projectNodes.Add(projectNode);
            }
            
            // Create relationship between projects after all nodes inserted to db
            foreach(var project in solutionFile.ProjectsInOrder)
            {
                var projectNode = projectNodes.Find(p => p.ProjectGuid == project.ProjectGuid);
                foreach(var dependency in project.Dependencies)
                {
                    var targetProjectNode = projectNodes.Find(p => p.ProjectGuid == dependency);

                    this._Repository.CreateRelationship(projectNode.Id, targetProjectNode.Id, "DEPENDS");
                }
            }
        }
    }
}
