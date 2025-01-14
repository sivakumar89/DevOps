provider "azurerm" {
  features {}
}

# Resource Group
resource "azurerm_resource_group" "aks_rg" {
  name     = "aks-monitor-rg"
  location = "East US"
}

# Log Analytics Workspace
resource "azurerm_log_analytics_workspace" "log_analytics" {
  name                = "aks-log-analytics"
  location            = azurerm_resource_group.aks_rg.location
  resource_group_name = azurerm_resource_group.aks_rg.name
  sku                 = "PerGB2018"
}

# AKS Cluster
resource "azurerm_kubernetes_cluster" "aks_cluster" {
  name                = "aks-cluster"
  location            = azurerm_resource_group.aks_rg.location
  resource_group_name = azurerm_resource_group.aks_rg.name
  dns_prefix          = "aksdns"

  default_node_pool {
    name       = "default"
    vm_size    = "Standard_DS2_v2"
    node_count = 2
  }

  identity {
    type = "SystemAssigned"
  }

  addon_profile {
    oms_agent {
      enabled                    = true
      log_analytics_workspace_id = azurerm_log_analytics_workspace.log_analytics.id
    }
  }
}

# CPU Alert Rule
resource "azurerm_monitor_metric_alert" "cpu_alert" {
  name                = "cpu-threshold-alert"
  resource_group_name = azurerm_resource_group.aks_rg.name
  scopes              = [azurerm_kubernetes_cluster.aks_cluster.id]
  description         = "Alert when CPU usage exceeds 80%"

  criteria {
    metric_namespace = "Microsoft.ContainerService/managedClusters"
    metric_name      = "CPUUsagePercentage"
    aggregation      = "Average"
    operator         = "GreaterThan"
    threshold        = 80
  }

  frequency  = "PT1M" # Check every minute
  severity   = 2      # Severity level (1 = critical, 2 = warning, etc.)
  enabled    = true

  action {
    action_group_id = azurerm_monitor_action_group.alert_action.id
  }
}

# Memory Alert Rule
resource "azurerm_monitor_metric_alert" "memory_alert" {
  name                = "memory-threshold-alert"
  resource_group_name = azurerm_resource_group.aks_rg.name
  scopes              = [azurerm_kubernetes_cluster.aks_cluster.id]
  description         = "Alert when memory usage exceeds 75%"

  criteria {
    metric_namespace = "Microsoft.ContainerService/managedClusters"
    metric_name      = "MemoryUsagePercentage"
    aggregation      = "Average"
    operator         = "GreaterThan"
    threshold        = 75
  }

  frequency  = "PT1M"
  severity   = 2
  enabled    = true

  action {
    action_group_id = azurerm_monitor_action_group.alert_action.id
  }
}

# Action Group for Notifications
resource "azurerm_monitor_action_group" "alert_action" {
  name                = "alert-action-group"
  resource_group_name = azurerm_resource_group.aks_rg.name
  short_name          = "AlertGrp"

  email_receiver {
    name          = "EmailReceiver"
    email_address = "admin@example.com"
  }
}
