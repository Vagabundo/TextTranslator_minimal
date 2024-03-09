terraform {
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "~> 2.65"
    }
  }

  required_version = ">= 0.14.9"
}

provider "azurerm" {
  features {}
}

resource "azurerm_app_service_plan" "linuxfreeplan" {
  name                = "${var.project_name}-${var.environment}-linuxplan"
  location            = var.location
  resource_group_name = "${var.project_name}-${var.environment}"
  kind                = "Linux"
  reserved            = true

  sku {
    tier = "Free"
    size = "F1"
  }
}

resource "azurerm_app_service" "appservice" {
  name                = "${azurerm_app_service_plan.linuxfreeplan.resource_group_name}-appservice"
  location            = var.location
  resource_group_name = azurerm_app_service_plan.linuxfreeplan.resource_group_name
  app_service_plan_id = azurerm_app_service_plan.linuxfreeplan.id
  https_only = false
  
  site_config {
    linux_fx_version  = "DOCKER|vagabundocker/${var.project_name}:${var.imagebuild}"
    use_32_bit_worker_process = true
  }
}
