using BigPicture.Core.Config;
using BigPicture.Core.Repository;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BigPicture.Repository.ElasticSearch
{
    public class ElasticRepository : ICodeRepository
    {
        private const String CODE_BLOCK_INDEX = "code";

        private static readonly ConnectionSettings ConnSettings = new ConnectionSettings(new Uri(CommonConfig.Instance.CodeRepository))
            .DefaultMappingFor<CodeBlock>(i => i                
                .IndexName(CODE_BLOCK_INDEX)
                .IdProperty(p => p.Id)
            )
            //.EnableDebugMode()
            .PrettyJson()
            .RequestTimeout(TimeSpan.FromMinutes(2));
        private readonly ElasticClient _ElasticClient = new ElasticClient(ConnSettings);

        public ElasticRepository()
        {            
            if(this._ElasticClient.Indices.Exists(CODE_BLOCK_INDEX).Exists == false)
            {
                this._ElasticClient.Indices.Create(CODE_BLOCK_INDEX);                
                this._ElasticClient.Map<CodeBlock>(m => m.Index(CODE_BLOCK_INDEX));
            }
        }

        public string CreateCodeBlock(CodeBlock codeBlock)
        {
            var response = this._ElasticClient.IndexDocument(codeBlock);
            if (response.IsValid == false)
            {
                throw new Exception("CodeBlock could not be indexed");             
            }

            return response.Id;
        }

        public CodeBlock GetCodeBlock(String id)
        {
            var r = this.CreateCodeBlock(new CodeBlock() { Id = "asd", Name = "Deneme" });

            var response = this._ElasticClient.Get<CodeBlock>(id);

            if(response.Found == false)
            {
                return null;
            }

            return response.Source;
        }

        public void RemoveAllCodeBlocks()
        {
            if (this._ElasticClient.Indices.Exists(CODE_BLOCK_INDEX).Exists)
            {
                this._ElasticClient.Indices.Delete(CODE_BLOCK_INDEX);

                this._ElasticClient.Indices.Create(CODE_BLOCK_INDEX);
                this._ElasticClient.Map<CodeBlock>(m => m.Index(CODE_BLOCK_INDEX));
            }
            
        }
    }
}
