## EventStore Cluster Host for OSS Clustering

### Introduction

The cluster host provides a simple way to configure and spin up [EventStore](http://www.geteventstore.com) OSS clusters as detailed in the [documentation](https://github.com/eventstore/eventstore/wiki/Setting-Up-OSS-Cluster). 

The host itself is a single Windows service that can contains any number of "internal" instances (running on that server), which will be restarted if they fail, and the configuration allows connection to "external" hosts for cross-machine clusters.

The host currently only supports "gossip seed" based clusters, rather than DNS based clusters.

### Installation

To install just build, copy the output to a directory on the server, update the configuration file as appropriate, copy the EventStore binaries to the correct location and run "Slicedbread.EventStore.ClusterHost install".

The host uses [TopShelf](http://topshelf-project.com/) for the Windows service configuration, for more information on commandline parameters please see their [documentation](http://docs.topshelf-project.com/en/latest/overview/commandline.html)

### Configuration
Configuration is split into four parts, logging, the global config, the internal node config and the external node config. You can dump the configuration to the screen by running Slicedbread.EventStore.ClusterHost with the --test parameter".

#### Logging

Logging uses [NLog](http://nlog-project.org/) and by default logs INFO messages and above to Slicedbread.EventStore.ClusterHost.log. The logging level (Debug, Info, Error etc.), and the targets (files, event log etc.) can be configured in Slicedbread.EventStore.ClusterHost.exe.config. For more information on logging configuration see the NLog [documentation](https://github.com/nlog/NLog/wiki).

#### Global Configuration
The global configuration options are:

**eventStorePath** *(optional)* - provides the path to the EventStore.ClusterNode.exe. If this is not specified then the host will look for it in an "event store" subdirectory. If it is specified it can be either an absolute or relative path, and shoudl include the executable name.

**restartDelayMs** *(optional)* - how long to wait (in milliseconds) before restarting a failed host process. Defaults to 3 seconds.

**restartWindowMs** *(optional)* - how long the "no restart" window is. If a host is started and fails within this window then it won't be restarted. This is to prevent a configuration change or other issue making the host constantly spin up new processes that will fail immediately. Defaults to 30 seconds.

**additionalFlags** *(optional)* - any additional flags to pass to **all** of the internal nodes.

#### Internal Node Configuration
Internal nodes are nodes that will be started up and managed by the host and are defined by a collection of "node" elements inside a "internalNodes" element. The configuration values for each node are:

**name** *(required)* - names the node, this will be the name that's used in all output messages.

**dbPath** *(required)* - specifies the database path for the node, can be relative or absolute.

**logPath** *(required)* - specified the log path for the node, can be relative or absolute.

**intHttpPort** *(required)* - specifies the internal http port.

**extHttpPort** *(required)* - specifies the external http port.

**intTcpPort** *(required)* - specifies the internal tcp port.

**extTcpPort** *(required)* - specifies the external tcp port.

**httpPrefix** *(optional)* - the http prefix to bind to - this is necessary if you need to listen on a particular hostname which some hosting providers (such as Azure) mandate.

**internalIpAddress** *(optional)* - the internal ip address of the node. Defaults to automatically detecting the IP - automatic detection will set the internal and external IPs to the same value.

**externalIpAddress** *(optional)* - the external ip address of the node. Defaults to automatically detecting the IP - automatic detection will set the internal and external IPs to the same value.

**additionalFlags** *(optional)* - any additional flags to pass to just this internal node.

For more information on internal/external ports and ips take a look at the EventStore [documentation](https://github.com/eventstore/eventstore/wiki/Setting-Up-OSS-Cluster)

#### External Node Configuration
External nodes are nodes that are not hosted inside this host instance, but form part of your cluster. They are defined by a collection of "node" elements inside an "externalNodes" element. The configuration values for each node are:

**name** *(required)* - names the node, this will be the name that's used in all output messages.

**ipAddress** *(required)* - the IP address the external node is available on.

**gossipPort** *(required)* - the gossip port for the node (normally the internal HTTP port).


