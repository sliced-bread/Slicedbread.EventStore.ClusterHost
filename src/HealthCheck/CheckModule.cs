using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using HealthCheck.Models;
using Nancy;
using Nancy.Json;

namespace HealthCheck
{
    public class CheckModule : NancyModule
    {
        public CheckModule()
        {
            Get["/", true] = (_, __) => this.RunCheck();
            Get["/cluster", true] = (_, __) => this.GetClusterJson();
            Get["/node", true] = (_, __) => this.GetNodeJson();
        }

        private async Task<dynamic> GetNodeJson()
        {
            var result = await GetEventStoreResponse();

            if (!result.IsSuccessStatusCode)
            {
                return (int)result.StatusCode;
            }

            var jsonBody = await result.Content.ReadAsStringAsync();
            var node = GetNode(jsonBody);
            if (node == null)
            {
                throw new InvalidOperationException("Unable to locate node with the same internal ip and port");
            }
            return Response.AsJson(node);
        }

        private static Member GetNode(string jsonBody)
        {
            var serialiser = new JavaScriptSerializer();
            var clusterStatus = serialiser.Deserialize<ClusterStatus>(jsonBody);
            if (clusterStatus == null)
            {
                throw new InvalidOperationException("Unable to parse cluster status json");
            }
            var config = MonitorService.HeathCheckConfiguration.EventStoreNode;
            var node = clusterStatus.Members?.SingleOrDefault(m => m.InternalHttpIp == config.InternalIpAddress && m.InternalHttpPort == config.IntHttpPort);
            return node;
        }

        private async Task<dynamic> GetClusterJson()
        {
            var result = await GetEventStoreResponse();

            if (!result.IsSuccessStatusCode)
            {
                return (int)result.StatusCode;
            }

            var jsonBody = await result.Content.ReadAsStringAsync();

            return Response.AsText(jsonBody, "application/json");
        }

        private async Task<dynamic> RunCheck()
        {
            var result = await GetEventStoreResponse();

            if (!result.IsSuccessStatusCode)
            {
                return (int)result.StatusCode;
            }

            var jsonBody = await result.Content.ReadAsStringAsync();
            var node = GetNode(jsonBody);
            if (node == null)
            {
                throw new InvalidOperationException("Unable to locate node with the same internal ip and port");
            }

            if (!node.IsAlive)
            {
                throw new InvalidOperationException("Node is not alive");
            }

            if (!node.State.Equals("Master", StringComparison.OrdinalIgnoreCase) &&
                !node.State.Equals("Slave", StringComparison.OrdinalIgnoreCase) &&
                !node.State.Equals("Clone", StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException($"Node is not ready: {node.State}");
            }
            
            return 200;
        }

        private async Task<HttpResponseMessage> GetEventStoreResponse()
        {
            var requestUrl = this.GetClusterStatsUrl();

            var httpClient = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(requestUrl, UriKind.Absolute),
                Headers =
                {
                    Accept = { new MediaTypeWithQualityHeaderValue("application/json") }
                }
            };
            var result = await httpClient.SendAsync(request);
            return result;
        }

        private string GetClusterStatsUrl()
        {
            var config = MonitorService.HeathCheckConfiguration;

            if (!string.IsNullOrEmpty(config.EventStoreNode.HttpPrefix))
            {
                return !config.EventStoreNode.HttpPrefix.Contains("*") ? 
                    $"{config.EventStoreNode.HttpPrefix}/gossip" : 
                    $"{config.EventStoreNode.HttpPrefix.Replace("*", config.EventStoreNode.InternalIpAddress)}/gossip";
            }

            return $"http://{config.EventStoreNode.InternalIpAddress}:{config.EventStoreNode.IntHttpPort}/gossip";
        }
    }
}