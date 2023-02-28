$rgName = "distributed-inventory-eus"
$region = "eastus"
$templateFile = ".\deploy.bicep"

New-AzResourceGroupDeployment `
  -Name "distributed-inventory-deployment" `
  -ResourceGroupName $rgName `
  -TemplateFile $templateFile `
    