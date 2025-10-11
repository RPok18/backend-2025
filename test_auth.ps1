# Test Authentication Flow
Write-Host "Testing Authentication Flow..." -ForegroundColor Green

# Step 1: Register a user
Write-Host "Step 1: Registering user..." -ForegroundColor Yellow
try {
    $registerBody = @{
        email = "john@email.com"
        password = "john1234"
        firstName = "john"
        lastName = "Doe"
        phone = "+79935656566"
        address = "145, street"
    } | ConvertTo-Json

    $registerResponse = Invoke-RestMethod -Uri "http://localhost:5000/api/Auth/register" -Method POST -ContentType "application/json" -Body $registerBody
    Write-Host "Registration successful!" -ForegroundColor Green
    Write-Host "Response: $($registerResponse | ConvertTo-Json -Depth 3)" -ForegroundColor Cyan
} catch {
    Write-Host "Registration failed: $($_.Exception.Message)" -ForegroundColor Red
    exit 1
}

# Step 2: Login to get JWT token
Write-Host "Step 2: Logging in..." -ForegroundColor Yellow
try {
    $loginBody = @{
        email = "john@email.com"
        password = "john1234"
    } | ConvertTo-Json

    $loginResponse = Invoke-RestMethod -Uri "http://localhost:5000/api/Auth/login" -Method POST -ContentType "application/json" -Body $loginBody
    Write-Host "Login successful!" -ForegroundColor Green
    Write-Host "Token: $($loginResponse.token)" -ForegroundColor Cyan
    
    $token = $loginResponse.token
} catch {
    Write-Host "Login failed: $($_.Exception.Message)" -ForegroundColor Red
    exit 1
}

# Step 3: Access Profile with token
Write-Host "Step 3: Accessing Profile with token..." -ForegroundColor Yellow
try {
    $headers = @{
        "Authorization" = "Bearer $token"
    }
    
    $profileResponse = Invoke-RestMethod -Uri "http://localhost:5000/api/Profile" -Method GET -Headers $headers
    Write-Host "Profile access successful!" -ForegroundColor Green
    Write-Host "Profile: $($profileResponse | ConvertTo-Json -Depth 3)" -ForegroundColor Cyan
} catch {
    Write-Host "Profile access failed: $($_.Exception.Message)" -ForegroundColor Red
    Write-Host "This might be expected if no profile exists yet" -ForegroundColor Yellow
}

Write-Host "Authentication flow test completed!" -ForegroundColor Green
