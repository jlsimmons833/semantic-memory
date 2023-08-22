﻿// Copyright (c) Microsoft. All rights reserved.

using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace Microsoft.SemanticMemory.Configuration;

public class SemanticMemoryConfig
{
    /// <summary>
    /// Settings for the upload of documents and memory creation/update.
    /// </summary>
    public class DataIngestionConfig
    {
        public class DistributedOrchestrationConfig
        {
            public string QueueType { get; set; } = string.Empty;
        }

        public string OrchestrationType { get; set; } = string.Empty;

        public DistributedOrchestrationConfig DistributedOrchestration { get; set; } = new();

        /// <summary>
        /// List of embedding types to generate during document ingestion.
        /// Using multiple types can help with migration from two different models, or for comparing models performance.
        /// </summary>
        public List<string> EmbeddingGeneratorTypes { get; set; } = new();

        /// <summary>
        /// List of vector storages where embeddings will be saved during ingestion.
        /// Multiple storages can help with data migrations and testing purposes.
        /// </summary>
        public List<string> VectorDbTypes { get; set; } = new();

        /// <summary>
        /// Default document ingestion pipeline steps.
        /// * extract: extract text from files
        /// * partition: spit the text in small chunks
        /// * gen_embeddings: generate embeddings for each chunk
        /// * save_embeddings: save the embeddings
        /// * summarize: use LLMs to summarize the document (this step can be slow, so it's meant to run after gen_embeddings/save_embeddings)
        /// * gen_embeddings: generate embeddings for new chunks (e.g. the summary)
        /// * save_embeddings: save new embeddings
        /// </summary>
        public List<string> DefaultSteps { get; set; } = new()
        {
            "extract",
            "partition",
            "gen_embeddings",
            "save_embeddings",
            "summarize",
            "gen_embeddings",
            "save_embeddings"
        };
    }

    /// <summary>
    /// Settings for search and memory read API.
    /// </summary>
    public class RetrievalConfig
    {
        /// <summary>
        /// The vector storage to search for relevant data used to generate answers
        /// </summary>
        public string VectorDbType { get; set; } = string.Empty;

        /// <summary>
        /// The embedding generator used for questions and searching for relevant data in the vector DB
        /// </summary>
        public string EmbeddingGeneratorType { get; set; } = string.Empty;
    }

    /// <summary>
    /// Semantic Memory Service settings.
    /// </summary>
    public ServiceConfig Service { get; set; } = new();

    /// <summary>
    /// Documents storage settings.
    /// </summary>
    public string ContentStorageType { get; set; } = string.Empty;

    /// <summary>
    /// The text generator used to generate synthetic data during ingestion
    /// and to generate answers during retrieval.
    /// </summary>
    public string TextGeneratorType { get; set; } = string.Empty;

    /// <summary>
    /// Settings for the upload of documents and memory creation/update.
    /// </summary>
    public DataIngestionConfig DataIngestion { get; set; } = new();

    /// <summary>
    /// Settings for search and memory read API.
    /// </summary>
    public RetrievalConfig Retrieval { get; set; } = new();

    /// <summary>
    /// Dependencies settings, e.g. credentials, endpoints, etc.
    /// </summary>
    public Dictionary<string, Dictionary<string, object>> Services { get; set; } = new();

    /// <summary>
    /// Fetch a service configuration from the "Services" node 
    /// </summary>
    /// <param name="cfg">Configuration instance</param>
    /// <param name="serviceName">Service name</param>
    /// <param name="root">Root node name of the Semantic Memory config</param>
    /// <typeparam name="T">Type of configuration to retrieve</typeparam>
    /// <returns>Instance of T configuration class</returns>
    public T GetServiceConfig<T>(IConfiguration cfg, string serviceName, string root = "SemanticMemory")
    {
        return cfg
            .GetSection(root)
            .GetSection("Services")
            .GetSection(serviceName)
            .Get<T>() ?? throw new ConfigurationException($"The {serviceName} configuration is NULL");
    }
}
