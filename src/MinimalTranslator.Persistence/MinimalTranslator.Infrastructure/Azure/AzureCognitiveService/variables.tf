variable "project_name" {
  default = "mini-translator-api"
}

variable "location" {
  default = "westeurope"
}

variable "environment" {
  default = "dev"
}

variable "resource_group_name" {
  default = "${var.project_name}-${var.environment}"
}