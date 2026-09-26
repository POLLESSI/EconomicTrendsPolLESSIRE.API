$ErrorActionPreference = "Stop"

$env:OLLAMA_HOST = "127.0.0.1:11435"
$env:OLLAMA_MODELS = "C:\Users\Pol PC\.ollama-economictrends\models"

Write-Host ""
Write-Host "========================================"
Write-Host " EconomicTrends Ollama"
Write-Host " Host   : $env:OLLAMA_HOST"
Write-Host " Models : $env:OLLAMA_MODELS"
Write-Host "========================================"
Write-Host ""

ollama serve
