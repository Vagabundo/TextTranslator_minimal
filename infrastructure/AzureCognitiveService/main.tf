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
  features {
    api_management {
      purge_soft_delete_on_destroy = true
    }
  }
}

resource "azurerm_resource_group" "rg" {
  name     = "${var.project_name}-${var.environment}"
  location = var.location

  tags = {
    Environment = "Translator"
    Team        = "VagaDevOps"
  }
}

resource "azurerm_cognitive_account" "translator_cognitive_service" {
  name                = "${azurerm_resource_group.rg.name}-translator-cognitiveservices"
  location            = azurerm_resource_group.rg.location
  resource_group_name = azurerm_resource_group.rg.name
  kind                = "TextTranslation"

  sku_name = "F0"
}