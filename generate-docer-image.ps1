# docker build -f ./Inclusive.HeatSensor.Web/Dockerfile -t inclusive-heat-sensor-web .

# Variables
$acrName = "icrheatsensor2409.azurecr.io"
$imageName = "inclusive-heat-sensor-web"
$imageTag = "0.2"  # You can change this to another tag if needed
$dockerfilePath = "./Inclusive.HeatSensor.Web/Dockerfile"

# Login to Azure
# az login --tenant 72f988bf-86f1-41af-91ab-2d7cd011db47
# az account set -s d21a525e-7c86-486d-a79e-a4f3622f639a

# Login to Azure Container Registry
az acr login --name $acrName

# Build the Docker image specifying the Dockerfile in the child folder
docker build -f $dockerfilePath -t "${imageName}:${imageTag}" .

# Tag the image for ACR
docker tag "${imageName}:${imageTag}" "${acrName}/${imageName}:${imageTag}"

# Push the image to Azure Container Registry
docker push "${acrName}/${imageName}:${imageTag}"

# Output the image URL
Write-Host "Docker image pushed to: ${acrName}/${imageName}:${imageTag}"
