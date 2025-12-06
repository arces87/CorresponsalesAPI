/*
 * Corresponsales.Query.Api
 *
 * Auto-generated client for tag 'CuentasPorCobrar' based on existing API style.
 *
 * The version of the OpenAPI document: 1.0
 */

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Mime;
using Corresponsales.Query.Client;
using Corresponsales.Query.Model;

namespace Corresponsales.Query.Api
{
    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints for tag CuentasPorCobrar
    /// </summary>
    public interface ICuentasPorCobrarApiSync : IApiAccessor
    {
        #region Synchronous Operations
        DevuelveCuentasPorCobrarDeUnClienteResponse DevuelveCuentasPorCobrarDeUnCliente(DevuelveCuentasPorCobrarDeUnClienteRequest? devuelveCuentasPorCobrarDeUnClienteRequest = default(DevuelveCuentasPorCobrarDeUnClienteRequest?), int operationIndex = 0);
        
        ApiResponse<DevuelveCuentasPorCobrarDeUnClienteResponse> DevuelveCuentasPorCobrarDeUnClienteWithHttpInfo(DevuelveCuentasPorCobrarDeUnClienteRequest? devuelveCuentasPorCobrarDeUnClienteRequest = default(DevuelveCuentasPorCobrarDeUnClienteRequest?), int operationIndex = 0);
        #endregion Synchronous Operations
    }

    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints for tag CuentasPorCobrar
    /// </summary>
    public interface ICuentasPorCobrarApiAsync : IApiAccessor
    {
        #region Asynchronous Operations
        System.Threading.Tasks.Task<DevuelveCuentasPorCobrarDeUnClienteResponse> DevuelveCuentasPorCobrarDeUnClienteAsync(DevuelveCuentasPorCobrarDeUnClienteRequest? devuelveCuentasPorCobrarDeUnClienteRequest = default(DevuelveCuentasPorCobrarDeUnClienteRequest?), int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));
        
        System.Threading.Tasks.Task<ApiResponse<DevuelveCuentasPorCobrarDeUnClienteResponse>> DevuelveCuentasPorCobrarDeUnClienteWithHttpInfoAsync(DevuelveCuentasPorCobrarDeUnClienteRequest? devuelveCuentasPorCobrarDeUnClienteRequest = default(DevuelveCuentasPorCobrarDeUnClienteRequest?), int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));
        #endregion Asynchronous Operations
    }

    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public interface ICuentasPorCobrarApi : ICuentasPorCobrarApiSync, ICuentasPorCobrarApiAsync
    {

    }

    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public partial class CuentasPorCobrarApi : ICuentasPorCobrarApi
    {
        private Corresponsales.Query.Client.ExceptionFactory _exceptionFactory = (name, response) => null;

        /// <summary>
        /// Initializes a new instance of the <see cref="CuentasPorCobrarApi"/> class.
        /// </summary>
        public CuentasPorCobrarApi() : this((string)null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CuentasPorCobrarApi"/> class.
        /// </summary>
        public CuentasPorCobrarApi(string basePath)
        {
            this.Configuration = Corresponsales.Query.Client.Configuration.MergeConfigurations(
                Corresponsales.Query.Client.GlobalConfiguration.Instance,
                new Corresponsales.Query.Client.Configuration { BasePath = basePath }
            );
            this.Client = new Corresponsales.Query.Client.ApiClient(this.Configuration.BasePath);
            this.AsynchronousClient = new Corresponsales.Query.Client.ApiClient(this.Configuration.BasePath);
            this.ExceptionFactory = Corresponsales.Query.Client.Configuration.DefaultExceptionFactory;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CuentasPorCobrarApi"/> class
        /// using Configuration object
        /// </summary>
        /// <param name="configuration">An instance of Configuration</param>
        public CuentasPorCobrarApi(Corresponsales.Query.Client.Configuration configuration)
        {
            if (configuration == null) throw new ArgumentNullException("configuration");

            this.Configuration = Corresponsales.Query.Client.Configuration.MergeConfigurations(
                Corresponsales.Query.Client.GlobalConfiguration.Instance,
                configuration
            );
            this.Client = new Corresponsales.Query.Client.ApiClient(this.Configuration.BasePath);
            this.AsynchronousClient = new Corresponsales.Query.Client.ApiClient(this.Configuration.BasePath);
            this.ExceptionFactory = Corresponsales.Query.Client.Configuration.DefaultExceptionFactory;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CuentasPorCobrarApi"/> class
        /// using a Configuration object and client instance.
        /// </summary>
        /// <param name="client">The client interface for synchronous API access.</param>
        /// <param name="asyncClient">The client interface for asynchronous API access.</param>
        /// <param name="configuration">The configuration object.</param>
        public CuentasPorCobrarApi(Corresponsales.Query.Client.ISynchronousClient client, Corresponsales.Query.Client.IAsynchronousClient asyncClient, Corresponsales.Query.Client.IReadableConfiguration configuration)
        {
            if (client == null) throw new ArgumentNullException("client");
            if (asyncClient == null) throw new ArgumentNullException("asyncClient");
            if (configuration == null) throw new ArgumentNullException("configuration");

            this.Client = client;
            this.AsynchronousClient = asyncClient;
            this.Configuration = configuration;
            this.ExceptionFactory = Corresponsales.Query.Client.Configuration.DefaultExceptionFactory;
        }

        /// <summary>
        /// The client for accessing this underlying API asynchronously.
        /// </summary>
        public Corresponsales.Query.Client.IAsynchronousClient AsynchronousClient { get; set; }

        /// <summary>
        /// The client for accessing this underlying API synchronously.
        /// </summary>
        public Corresponsales.Query.Client.ISynchronousClient Client { get; set; }

        /// <summary>
        /// Gets the base path of the API client.
        /// </summary>
        public string GetBasePath()
        {
            return this.Configuration.BasePath;
        }

        /// <summary>
        /// Gets or sets the configuration object
        /// </summary>
        public Corresponsales.Query.Client.IReadableConfiguration Configuration { get; set; }

        /// <summary>
        /// Provides a factory method hook for the creation of exceptions.
        /// </summary>
        public Corresponsales.Query.Client.ExceptionFactory ExceptionFactory
        {
            get
            {
                if (_exceptionFactory != null && _exceptionFactory.GetInvocationList().Length > 1)
                {
                    throw new InvalidOperationException("Multicast delegate for ExceptionFactory is unsupported.");
                }
                return _exceptionFactory;
            }
            set { _exceptionFactory = value; }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="Corresponsales.Query.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="devuelveCuentasPorCobrarDeUnClienteRequest"> </param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns>DevuelveCuentasPorCobrarDeUnClienteResponse</returns>
        public DevuelveCuentasPorCobrarDeUnClienteResponse DevuelveCuentasPorCobrarDeUnCliente(DevuelveCuentasPorCobrarDeUnClienteRequest? devuelveCuentasPorCobrarDeUnClienteRequest = default(DevuelveCuentasPorCobrarDeUnClienteRequest?), int operationIndex = 0)
        {
            Corresponsales.Query.Client.ApiResponse<DevuelveCuentasPorCobrarDeUnClienteResponse> localVarResponse = DevuelveCuentasPorCobrarDeUnClienteWithHttpInfo(devuelveCuentasPorCobrarDeUnClienteRequest);
            return localVarResponse.Data;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="Corresponsales.Query.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="devuelveCuentasPorCobrarDeUnClienteRequest"> </param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns>ApiResponse of DevuelveCuentasPorCobrarDeUnClienteResponse</returns>
        public Corresponsales.Query.Client.ApiResponse<DevuelveCuentasPorCobrarDeUnClienteResponse> DevuelveCuentasPorCobrarDeUnClienteWithHttpInfo(DevuelveCuentasPorCobrarDeUnClienteRequest? devuelveCuentasPorCobrarDeUnClienteRequest = default(DevuelveCuentasPorCobrarDeUnClienteRequest?), int operationIndex = 0)
        {
            Corresponsales.Query.Client.RequestOptions localVarRequestOptions = new Corresponsales.Query.Client.RequestOptions();

            string[] _contentTypes = new string[] {
                "application/json",
                "text/json",
                "application/*+json"
            };

            // to determine the Accept header
            string[] _accepts = new string[] {
                "text/plain",
                "application/json",
                "text/json"
            };

            var localVarContentType = Corresponsales.Query.Client.ClientUtils.SelectHeaderContentType(_contentTypes);
            if (localVarContentType != null)
            {
                localVarRequestOptions.HeaderParameters.Add("Content-Type", localVarContentType);
            }

            var localVarAccept = Corresponsales.Query.Client.ClientUtils.SelectHeaderAccept(_accepts);
            if (localVarAccept != null)
            {
                localVarRequestOptions.HeaderParameters.Add("Accept", localVarAccept);
            }

            localVarRequestOptions.Data = devuelveCuentasPorCobrarDeUnClienteRequest;
            localVarRequestOptions.Operation = "CuentasPorCobrarApi.DevuelveCuentasPorCobrarDeUnCliente";
            localVarRequestOptions.OperationIndex = operationIndex;

            // authentication (Bearer) required
            if (!string.IsNullOrEmpty(this.Configuration.GetApiKeyWithPrefix("Authorization")))
            {
                localVarRequestOptions.HeaderParameters.Add("Authorization", this.Configuration.GetApiKeyWithPrefix("Authorization"));
            }

            // make the HTTP request
            var localVarResponse = this.Client.Post<DevuelveCuentasPorCobrarDeUnClienteResponse>("/CuentasPorCobrar/DevuelveCuentasPorCobrarDeUnCliente", localVarRequestOptions, this.Configuration);
            if (this.ExceptionFactory != null)
            {
                Exception _exception = this.ExceptionFactory("DevuelveCuentasPorCobrarDeUnCliente", localVarResponse);
                if (_exception != null)
                {
                    throw _exception;
                }
            }

            return localVarResponse;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="Corresponsales.Query.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="devuelveCuentasPorCobrarDeUnClienteRequest"> </param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of DevuelveCuentasPorCobrarDeUnClienteResponse</returns>
        public async System.Threading.Tasks.Task<DevuelveCuentasPorCobrarDeUnClienteResponse> DevuelveCuentasPorCobrarDeUnClienteAsync(DevuelveCuentasPorCobrarDeUnClienteRequest? devuelveCuentasPorCobrarDeUnClienteRequest = default(DevuelveCuentasPorCobrarDeUnClienteRequest?), int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            Corresponsales.Query.Client.ApiResponse<DevuelveCuentasPorCobrarDeUnClienteResponse> localVarResponse = await DevuelveCuentasPorCobrarDeUnClienteWithHttpInfoAsync(devuelveCuentasPorCobrarDeUnClienteRequest, operationIndex, cancellationToken).ConfigureAwait(false);
            return localVarResponse.Data;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="Corresponsales.Query.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="devuelveCuentasPorCobrarDeUnClienteRequest"> </param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse (DevuelveCuentasPorCobrarDeUnClienteResponse)</returns>
        public async System.Threading.Tasks.Task<Corresponsales.Query.Client.ApiResponse<DevuelveCuentasPorCobrarDeUnClienteResponse>> DevuelveCuentasPorCobrarDeUnClienteWithHttpInfoAsync(DevuelveCuentasPorCobrarDeUnClienteRequest? devuelveCuentasPorCobrarDeUnClienteRequest = default(DevuelveCuentasPorCobrarDeUnClienteRequest?), int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {

            Corresponsales.Query.Client.RequestOptions localVarRequestOptions = new Corresponsales.Query.Client.RequestOptions();

            string[] _contentTypes = new string[] {
                "application/json",
                "text/json",
                "application/*+json"
            };

            // to determine the Accept header
            string[] _accepts = new string[] {
                "text/plain",
                "application/json",
                "text/json"
            };

            var localVarContentType = Corresponsales.Query.Client.ClientUtils.SelectHeaderContentType(_contentTypes);
            if (localVarContentType != null)
            {
                localVarRequestOptions.HeaderParameters.Add("Content-Type", localVarContentType);
            }

            var localVarAccept = Corresponsales.Query.Client.ClientUtils.SelectHeaderAccept(_accepts);
            if (localVarAccept != null)
            {
                localVarRequestOptions.HeaderParameters.Add("Accept", localVarAccept);
            }

            localVarRequestOptions.Data = devuelveCuentasPorCobrarDeUnClienteRequest;
            localVarRequestOptions.Operation = "CuentasPorCobrarApi.DevuelveCuentasPorCobrarDeUnCliente";
            localVarRequestOptions.OperationIndex = operationIndex;

            // authentication (Bearer) required
            if (!string.IsNullOrEmpty(this.Configuration.GetApiKeyWithPrefix("Authorization")))
            {
                localVarRequestOptions.HeaderParameters.Add("Authorization", this.Configuration.GetApiKeyWithPrefix("Authorization"));
            }

            // make the HTTP request
            var localVarResponse = await this.AsynchronousClient.PostAsync<DevuelveCuentasPorCobrarDeUnClienteResponse>("/CuentasPorCobrar/DevuelveCuentasPorCobrarDeUnCliente", localVarRequestOptions, this.Configuration, cancellationToken).ConfigureAwait(false);

            if (this.ExceptionFactory != null)
            {
                Exception _exception = this.ExceptionFactory("DevuelveCuentasPorCobrarDeUnCliente", localVarResponse);
                if (_exception != null)
                {
                    throw _exception;
                }
            }

            return localVarResponse;
        }

    }
}
