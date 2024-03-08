output "translator_service_endpoint" {
  description = "The endpoint used to connect to the Cognitive Service Account."
  value       = azurerm_cognitive_account.translator_cognitive_service.endpoint
}

# it should be the same value as var.location
output "translator_service_location" {
  description = "The location of the Cognitive Service Account."
  value       = azurerm_cognitive_account.translator_cognitive_service.location
}

output "translator_service_primary_access_key" {
  description = "A primary access key which can be used to connect to the Cognitive Service Account."
  value       = azurerm_cognitive_account.translator_cognitive_service.primary_access_key
  sensitive   = true
}